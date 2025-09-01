# Simplified Bank

[![Versão](https://img.shields.io/badge/version-1.0.0-blue.svg)](https://img.shields.io/badge/version-1.0.0-blue.svg)
[![.NET](https://img.shields.io/badge/.NET-8.0-5C2D91)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-12.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![Docker](https://img.shields.io/badge/Docker-enabled-2496ED?logo=docker)](https://www.docker.com/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?logo=swagger)](https://swagger.io/)

Bem-vindo(a) ao **Simplified Bank**, uma API de transações financeiras entre usuários, desenvolvida em C# com foco em arquitetura limpa e boas práticas modernas.

---

## 📚 Propósito
Este projeto visa simular operações bancárias essenciais (como transferências entre usuários), servindo como laboratório para estudo de padrões arquiteturais robustos, validação de dados sensíveis (CPF/CNPJ) e implementação de regras de negócio reais do universo financeiro.

Este projeto é baseado no desafio Back-End do PicPay, está **em desenvolvimento** e, portanto, ainda não está completo.

---

## 🛠️ Como rodar o projeto
Para rodar o projeto localmente e testar as funcionalidades, siga estas [INSTRUÇÕES](INSTRUCTIONS.md).

### Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/downloads) (para clonagem do repo)

---

## 🚀 Stack 

### Core
- C# / ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022

### Bibliotecas
- MediatR 12.5.0
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- [SecureIdentity](https://github.com/andrebaltieri/SecureIdentity) (Hashing de senhas)

### Ferramentas de Desenvolvimento
- Postman (Testes de API)
- Swagger (Documentação da API)
- Docker (Dockerfile e Docker Compose)
- [smtp4dev](https://github.com/rnwood/smtp4dev) (Servidor SMTP para envio de e-mails)

---

## 📄 Regras de Negócio
- O sistema deve possuir dois tipos de usuário, sendo o usuário **Comum** e o usuário **Lojista**.
- A senha dos usuários deve ser criptografada e armazenada se forma segura.
- O E-mail e o documento (CPF/CNPJ) de um usuário devem ser **únicos** no sistema, não sendo possível realizar o cadastro caso uma ou ambas as propriedades já existam na base de dados.
- Os novos usuários do sistema recebem um bônus de R$ 100,00, sendo que esse valor pode ser alterado ou removido a qualquer momento.
- Apenas os usuários do tipo **Comum** podem enviar transferências. 
- Ambos os usuários **Comum** e **Lojista** podem receber transferências.
- Para realizar e/ou consultar uma transferência, o usuário deve estar registrado e autenticado no sistema.
- Ao realizar a transferência, o usuário deverá ter saldo suficiente para concluir a transação.
- A transação possui um valor mínimo (R\$ 0,01) e máximo (R\$ 10.000.000,00) permitido.
- A transação deverá ser validada por um serviço verificador antes de ser concluída.
- A transação deve ser segura em casos de inconsistência, fazendo com que o saldo dos usuários seja preservado.
- Os usuários envolvidos numa transação (pagador e recebedor) devem ser notificados sobre a mesma.

---

## 🛞 Padrões e Boas Práticas
O projeto aplica **_design patterns_** e outras boas práticas de desenvolvimento:
- Clean Architecture
- CQRS parcial e Mediator
- Repositórios genéricos
- Unit of Work
- Dependency Injection
- Domínio rico de entidades
- Mapeamento nativo de entidades
- Validações e exceções personalizadas no domínio (Email, CPF e CNPJ Alfanumérico; unicidade de usuários)
- Respostas padronizadas e paginação de dados (planejado)
- Mapeamento de tabelas com Fluent API
- Hashing da senha dos usuários
- Documentação da API com Swagger
- Arquivo _Program.cs_ limpo e organizado com as configurações definidas em cada camada

---

## 🎯 Funcionalidades Implementadas
- [x] Criação inicial de entidades: `User` e `Transaction`
- [x] Estrutura de camadas: Domain, Application, Infrastructure e API
- [x] Repositórios e Unit of Work definidos
- [x] Validações de CPF e CNPJ no domínio
- [x] Exceções personalizadas criadas no domínio
- [x] CRUD de usuários e transações
- [x] Hashing da senha dos usuários
- [x] Validações de unicidade de usuário
- [x] Bônus de novo usuário
- [x] Validação das regras de Transação
- [x] Serviços externos de autorização de transação
- [x] Notificações via e-mail 
- [x] Segurança do saldo dos usuários contra inconsistência e concorrência
- [x] Autenticação JWT Bearer
- [x] Dockerização com Docker Compose

---

## 📂 Estrutura do Projeto
```plaintext
SimplifiedBank.Domain/       # Entidades, enums, validadores e exceções
SimplifiedBank.Application/  # UseCases, Services, Requests/Responses, Validators
SimplifiedBank.Infrastructure/ # Repositórios concretos, EF Core
SimplifiedBank.API/          # Controllers e configuração da aplicação
SimplifiedBank.Tests/        # Testes de software (planejado)
```

---

## 📌 Status do Projeto
Este projeto está em **estágio avançado de desenvolvimento** e está sendo atualizado continuamente.

---

## ✨ Contribuições
Contribuições são bem-vindas!  

Este projeto faz parte do meu portfólio de aprendizado como desenvolvedor back-end.

---

## 🗃️ Licença
Este projeto está licenciado sob a licença MIT.
* [LICENSE](LICENSE)
