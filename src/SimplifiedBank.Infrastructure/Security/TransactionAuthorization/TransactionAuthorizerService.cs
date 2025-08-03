using Newtonsoft.Json;
using SimplifiedBank.Application.Services.TransactionAuthorization;

namespace SimplifiedBank.Infrastructure.Security.TransactionAuthorization;

public class TransactionAuthorizerService : ITransactionAuthorizerService
{
    private readonly HttpClient _httpClient;

    public TransactionAuthorizerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsAuthorizedAsync()
    {
        var response = await _httpClient.GetAsync(InfrastructureConfiguration.TransactionAuthorizationUrl);

        var authorize =
            JsonConvert.DeserializeObject<TransactionAuthorizerOut>(await response.Content.ReadAsStringAsync()) ??
            new TransactionAuthorizerOut();

        return authorize.Data.Authorization;
    }
}