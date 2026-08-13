using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace AgendamentoAPI.Tests
{
    public class AgendamentoControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AgendamentoControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            // Geramos um token JWT válido para a role "Cliente"
            string token = GerarTokenDeTeste("Cliente");

            // Injetamos o token no cabeçalho de todas as requisições do teste
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Método auxiliar para criar tokens JWT válidos durante a execução dos testes
        private string GerarTokenDeTeste(string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // ATENÇÃO: Use a mesma chave que está configurada no seu appsettings/Program.cs
            var key = Encoding.ASCII.GetBytes("SuaChaveSecretaSuperSeguraComPeloMenos32Caracteres!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "UsuarioTeste"),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [Fact]
        public async Task POST_CriarAgendamentoValido_DeveRetornarCreated()
        {
            // Arrange
            var inicio = DateTime.Now.AddDays(2);
            var novoAgendamento = new
            {
                IdCliente = _factory.ClienteId,
                IdProfissional = _factory.ProfissionalId,
                IdServico = _factory.ServicoId,
                DataHoraInicio = inicio,
                DataHoraFim = inicio.AddHours(1)
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/agendamento", novoAgendamento);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task POST_CriarAgendamento_DataNoPassado_DeveRetornarBadRequest()
        {
            // Arrange
            var inicio = DateTime.Now.AddDays(-1);
            var agendamentoDataInvalida = new
            {
                IdCliente = _factory.ClienteId,
                IdProfissional = _factory.ProfissionalId,
                IdServico = _factory.ServicoId,
                DataHoraInicio = inicio,
                DataHoraFim = inicio.AddHours(1)
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/agendamento", agendamentoDataInvalida);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task POST_CriarAgendamento_HorarioOcupado_DeveRetornarBadRequest()
        {
            // Arrange
            var inicio = DateTime.Now.AddDays(5);

            var primeiroAgendamento = new
            {
                IdCliente = _factory.ClienteId,
                IdProfissional = _factory.ProfissionalId,
                IdServico = _factory.ServicoId,
                DataHoraInicio = inicio,
                DataHoraFim = inicio.AddHours(1)
            };

            var agendamentoConflitante = new
            {
                IdCliente = _factory.ClienteId,
                IdProfissional = _factory.ProfissionalId,
                IdServico = _factory.ServicoId,
                DataHoraInicio = inicio.AddMinutes(30), // Sobreposição
                DataHoraFim = inicio.AddHours(1).AddMinutes(30)
            };

            // Act
            await _client.PostAsJsonAsync("/api/agendamento", primeiroAgendamento);
            var response = await _client.PostAsJsonAsync("/api/agendamento", agendamentoConflitante);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GET_ObterAgendamentoPorIdInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/agendamento/{idInexistente}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_RemoverAgendamentoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/api/agendamento/{idInexistente}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PUT_CancelarAgendamentoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var response = await _client.PutAsync($"/api/agendamento/{idInexistente}/cancelar", null);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}