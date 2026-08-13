using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendamentoAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace AgendamentoAPI.Tests
{
    public class ServicoControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ServicoControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GET_ListarServicos_SemToken_DeveRetornar_401Unauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/Servico");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task POST_CadastrarServico_ComoProfissional_DeveRetornar_201Created()
        {
            // Arrange: Apenas Profissional pode cadastrar um serviço
            AutenticarCliente("Profissional");

            var dto = new ServicoDTO
            {
                Nome = "Corte de Cabelo Teste",
                Preco = 50.00m,
                DuracaoMinutos = 45
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Servico", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private void AutenticarCliente(string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SuaChaveSecretaSuperSeguraComPeloMenos32Caracteres!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "UsuarioTeste"),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHandler.WriteToken(token));
        }
    }
}