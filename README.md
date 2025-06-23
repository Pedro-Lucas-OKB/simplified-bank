# SimplifiedBank

## 📚 Visão Geral
O **SimplifiedBank** é uma API para simular transferências financeiras entre usuários, desenvolvida com ASP.NET Core seguindo o padrão Clean Architecture.

O projeto aplica boas práticas de desenvolvimento, como:
- Repositórios genéricos
- Unit of Work
- Mapeamento nativo de entidades
- Validações personalizadas de CPF e CNPJ Alfanumérico
- Exceções específicas do domínio

Este projeto é baseado no desafio Back-End do PicPay, está **em desenvolvimento** e, portanto, ainda não está completo.

---

## 🚀 Tecnologias Utilizadas
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker Compose (planejado)
- FluentValidation
- Clean Architecture simplificada

---

## 🎯 Funcionalidades Implementadas
- [x] Criação inicial de entidades: `User` e `Transaction`
- [x] Estrutura de camadas: Domain, Application, Infrastructure e API
- [x] Repositórios e Unit of Work definidos
- [x] Validações de CPF e CNPJ no domínio
- [x] Exceções personalizadas criadas no domínio
- [ ] Validações de unicidade (em andamento)
- [ ] Serviços externos de autorização e notificação (a implementar)
- [ ] Autenticação JWT (planejado)
- [ ] Dockerização (planejado)

---

## 🛠️ Como rodar o projeto (em breve)
```bash
# Clone o repositório
git clone https://github.com/Pedro-Lucas-OKB/simplified-bank.git

# Abra no VS Code, no Visual Studio ou no Jetbrains Rider
# Configure a string de conexão no appsettings.json
# Rode as migrations manualmente (ou aguarde dockerização)

# Execute a API
dotnet run --project SimplifiedBank.API
```

> 🚧 Instruções completas de execução e dockerização serão incluídas futuramente.

---

## 📂 Estrutura do Projeto
```plaintext
SimplifiedBank.Domain/       # Entidades, enums, validadores e exceções
SimplifiedBank.Application/  # UseCases, Services, Requests/Responses, Validators
SimplifiedBank.Infrastructure/ # Repositórios concretos, EF Core
SimplifiedBank.API/          # Controllers e configuração da aplicação
```

---

## 📌 Status do Projeto
Este projeto está **em construção** e será atualizado continuamente.

---

## ✨ Contribuições
Contribuições são bem-vindas!  
Este projeto faz parte do meu portfólio de aprendizado como desenvolvedor backend.
