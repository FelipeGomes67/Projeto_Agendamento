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
    public class ProfissionalControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProfissionalControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task POST_CadastrarProfissional_SemToken_DevePermitir_201Created()
        {
            // Arrange (Cadastro é AllowAnonymous)
            var dto = new ProfissionalDTO
            {
                Nome = "Profissional Teste",
                Email = $"pro{Guid.NewGuid()}@teste.com",
                Senha = "Senha123SuperSegura!",
                Telefone = "11888888888",
                Disponivel = true
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Profissional", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PUT_AtualizarProfissional_ComoCliente_DeveRetornar_403Forbidden()
        {
            // Arrange: Loga como "Cliente" e tenta alterar um Profissional (Deve dar 403 Forbidden!)
            AutenticarCliente("Cliente");

            var dto = new ProfissionalDTO
            {
                Nome = "Novo Nome",
                Email = "email@teste.com",
                Senha = "Senha123SuperSegura!",
                Telefone = "11888888888",
                Disponivel = true
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Profissional/{Guid.NewGuid()}", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_RemoverProfissional_ComoProfissional_ComIdInexistente_DeveRetornar_404NotFound()
        {
            // Arrange: Loga como "Profissional" (tem permissão, mas o ID não existe)
            AutenticarCliente("Profissional");

            // Act
            var response = await _client.DeleteAsync($"/api/Profissional/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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