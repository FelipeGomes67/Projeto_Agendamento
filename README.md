# 📅 Agendamento API

Uma API RESTful desenvolvida em **.NET 10** e **Entity Framework Core** para gerenciamento de agendamentos entre clientes e profissionais, com suporte a autenticação JWT e documentação interativa via Swagger UI.

---

## 🚀 Tecnologias Utilizadas

* **C# / .NET 10**
* **Entity Framework Core** (SQL Server)
* **JWT (JSON Web Token)** (Autenticação e Autorização por Roles)
* **BCrypt.Net-Next** (Criptografia de senhas)
* **Swashbuckle / Swagger UI** (Com autorização Bearer configurada)
* **JSON Serializer** (Tratamento de loops em relacionamentos)

---

## 🛠️ Funcionalidades Implementadas

- [x] Arquitetura em camadas com Padrão Repository (Interfaces & Implementation).
- [x] Injeção de Dependência (DI) configurada para todos os serviços.
- [x] Mapeamento de relacionamentos complexos (`Include`) para `Agendamento`, `Cliente`, `Profissional` e `Serviço`.
- [x] Configuração de banco de dados SQL Server via `DbContext`.
- [x] Autenticação via Token JWT e controle de acesso baseado em Roles (`Cliente` e `Profissional`).
- [x] Política de **CORS** para integração com aplicações web/mobile.
- [x] Documentação interativa via **Swagger UI** com suporte ao botão Bearer Authorize.

---

## ⚙️ Configuração do Ambiente

### 1. Pré-requisitos
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* SQL Server (ou SQL Server Express / LocalDB)
* Visual Studio 2022+ ou VS Code

### 2. Configurar a String de Conexão e JWT
No arquivo `appsettings.json`, ajuste a chave `ConexaoPadrao` e a chave secreta do JWT:

```json
{
  "ConnectionStrings": {
    "ConexaoPadrao": "Server=(localdb)\\MSSQLLocalDB;Database=AgendamentoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Chave": "SuaChaveSecretaSuperSeguraComPeloMenos32Caracteres!"
  }
}

🏎️ Como Executar a Aplicação

    Clone o repositório:

Bash

git clone [https://github.com/SeuUsuario/AgendamentoAPI.git](https://github.com/SeuUsuario/AgendamentoAPI.git)

    Acesse a pasta do projeto:

Bash

cd AgendamentoAPI

    Restaure as dependências:

Bash

dotnet restore

    Execute a aplicação:

Bash

dotnet run

    Acesse a documentação do Swagger UI na raiz do navegador:

    https://localhost:XXXX/

📁 Estrutura do Projeto
Plaintext

AgendamentoAPI/
├── Controllers/      # Endpoints da API (Auth, Profissional, Agendamento, etc.)
├── DTOs/             # Objetos de transferência de dados
├── Interfaces/       # Contratos dos Repositories e Services
├── Models/           # Entidades do Entity Framework Core
├── Repositories/     # Implementação de acesso aos dados
├── Services/         # Serviços de negócio (Geração de Token JWT, etc.)
├── Program.cs        # Configuração da aplicação e middlewares
└── appsettings.json  # Configurações de ambiente, JWT e banco de dados

✒️ Autor

Desenvolvido por Felipe Gomes.
