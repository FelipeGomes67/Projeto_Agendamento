using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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