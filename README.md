# 📅 Agendamento API

Uma API RESTful desenvolvida em **.NET 10** e **Entity Framework Core** para gerenciamento de agendamentos entre clientes e profissionais, com suporte a autenticação e documentação interativa via Swagger UI.

---

## 🚀 Tecnologias Utilizadas

* **C# / .NET 10**
* **Entity Framework Core** (SQL Server)
* **BCrypt.Net-Next** (Criptografia de senhas)
* **Swashbuckle / Swagger UI** (Com tema escuro via CDN)
* **JSON Serializer** (Tratamento de loops em relacionamentos)

---

## 🛠️ Funcionalidades Implementadas

- [x] Arquitetura em camadas com Padrão Repository (Interfaces & Implementation).
- [x] Injeção de Dependência (DI) configurada para todos os serviços.
- [x] Mapeamento de relacionamentos complexos (`Include`) para `Agendamento`, `Cliente`, `Profissional` e `Serviço`.
- [x] Configuração de banco de dados SQL Server via `DbContext`.
- [x] Política de **CORS** para integração com aplicações web/mobile.
- [x] Documentação interativa via **Swagger UI** customizada em Modo Escuro (*Dark Mode*).

---

## ⚙️ Configuração do Ambiente

### 1. Pré-requisitos
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* SQL Server (ou SQL Server Express / LocalDB)
* Visual Studio 2022+ ou VS Code

### 2. Configurar a String de Conexão
No arquivo `appsettings.json`, ajuste a chave `ConexaoPadrao` para apontar para seu servidor de banco de dados:

```json
{
  "ConnectionStrings": {
    "ConexaoPadrao": "Server=(localdb)\\MSSQLLocalDB;Database=AgendamentoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}