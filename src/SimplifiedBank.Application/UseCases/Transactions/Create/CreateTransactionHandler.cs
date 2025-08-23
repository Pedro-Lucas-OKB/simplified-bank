using MediatR;
using SimplifiedBank.Application.Services.Notification;
using SimplifiedBank.Application.Services.TransactionAuthorization;
using SimplifiedBank.Application.Shared.Exceptions;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.Create;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionRequest, TransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionAuthorizerService _transactionAuthorizerService;
    private readonly IEmailService _emailService;

    public CreateTransactionHandler(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ITransactionAuthorizerService transactionAuthorizerService,
        IEmailService emailService)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _transactionAuthorizerService = transactionAuthorizerService;
        _emailService = emailService;
    }

    public async Task<TransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var sender = await _userRepository.GetByIdAsync(request.SenderId, cancellationToken);
        var receiver = await _userRepository.GetByIdAsync(request.ReceiverId, cancellationToken);

        if (sender is null)
            throw new UserNotFoundException("Não foi possível encontrar o Pagador.");
        if (receiver is null)
            throw new UserNotFoundException("Não foi possível encontrar o Recebedor.");

        if (sender.Type == EUserType.Shopkeeper)
            throw new ShopkeeperCannotTransferException(
                "Usuários lojistas não podem realizar transações, apenas recebê-las.");

        var value = Math.Round(request.Value, 2); // arredondando o valor para 2 casas decimais

        //Realizando transação
        sender.Debit(value);
        receiver.Credit(value);

        var transaction = Transaction.Create(
            sender.Id,
            receiver.Id,
            value);
        await _transactionRepository.CreateAsync(transaction, cancellationToken);

        var authorized = await _transactionAuthorizerService.IsAuthorizedAsync();

        if (!authorized)
            throw new TransactionNotAuthorizedException("Transação não autorizada.");

        await _userRepository.UpdateAsync(sender, cancellationToken);
        await _userRepository.UpdateAsync(receiver, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        
        await NotifySentTransaction(sender, receiver.FullName, transaction);
        await NotifyReceivedTransaction(receiver, sender.FullName, transaction);       

        return transaction;
    }

    /// <summary>
    /// Envia notificação via e-mail para o pagador informando-o sobre a transferência realizada.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="receiverFullName"></param>
    /// <param name="transaction"></param>
    private async Task NotifySentTransaction(User sender, string receiverFullName, Transaction transaction)
    {
        try
        {
            _emailService.SendEmailAsync(
                sender.FullName,
                sender.Email,
                subject: "Nova transação realizada com sucesso!",
                body: 
$@"Olá, <strong>{sender.FullName}</strong>! <br><br>

Sua transferência para <strong>{receiverFullName}</strong> foi realizada com sucesso! <br><br>

<strong>Valor:</strong> {transaction.Value:C} <br>
<strong>Data da transação:</strong> {transaction.DateCreated:F} <br><br>

Abraços, <br>
<strong>Equipe Simplified Bank.</strong> <br>");
        }
        catch (Exception e)
        {
            new SmtpServerException(e, sender.Email, "E-mail de nova transação enviada"); // para logar o erro
        }
    }
    
    /// <summary>
    /// Envia notificação via e-mail para o recebedor informando-o sobre a transferência recebida.
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="senderFullName"></param>
    /// <param name="transaction"></param>
    private async Task NotifyReceivedTransaction(User receiver, string senderFullName, Transaction transaction)
    {
        try
        {
            _emailService.SendEmailAsync(
                receiver.FullName,
                receiver.Email,
                subject: "Você recebeu uma nova transferência para sua conta!",
                body: 
$@"Olá, <strong>{receiver.FullName}</strong>! <br><br>

Você recebeu uma nova transferência e o valor já está na sua conta! <br><br>

<strong>Quem enviou:</strong> {senderFullName} <br>
<strong>Valor:</strong> {transaction.Value:C} <br>
<strong>Data da transação:</strong> {transaction.DateCreated:F} <br><br>

Abraços, <br>
<strong>Equipe Simplified Bank.</strong> <br>");
        }
        catch (Exception e)
        {
            new SmtpServerException(e, receiver.Email, "E-mail de nova transação recebida"); // para logar o erro
        }
    }
}