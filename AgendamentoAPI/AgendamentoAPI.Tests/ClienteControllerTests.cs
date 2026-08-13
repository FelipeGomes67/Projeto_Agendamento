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
    public class ClienteControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ClienteControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task POST_CadastrarCliente_SemToken_DevePermitir_201Created()
        {
            // Arrange (Cadastrar é AllowAnonymous)
            var dto = new ClienteDTO
            {
                Nome = "Cliente Teste",
                Email = $"cliente{Guid.NewGuid()}@teste.com",
                Senha = "Senha123SuperSegura!",
                Telefone = "11999999999"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Cliente", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GET_ListarClientes_SemToken_DeveRetornar_401Unauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/Cliente");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GET_ListarClientes_ComTokenCliente_DeveRetornar_200OK()
        {
            // Arrange
            AutenticarCliente("Cliente");

            // Act
            var response = await _client.GetAsync("/api/Cliente");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private void AutenticarCliente(string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SuaChaveSecretaSuperSeguraComPeloMenos32Caracteres!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "ClienteTeste"),
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