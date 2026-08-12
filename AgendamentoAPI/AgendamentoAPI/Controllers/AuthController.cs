using AgendamentoAPI.DTOs;
using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly ITokenService _tokenService;

    public AuthController(
        IClienteRepository clienteRepository,
        IProfissionalRepository profissionalRepository,
        ITokenService tokenService)
    {
        _clienteRepository = clienteRepository;
        _profissionalRepository = profissionalRepository;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var clientes = await _clienteRepository.ListarAsync();
        var cliente = clientes.FirstOrDefault(c => c.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

        if (cliente != null && BCrypt.Net.BCrypt.Verify(dto.Senha, cliente.Senha))
        {
            var token = _tokenService.GerarToken(cliente.IdCliente, cliente.Nome, cliente.Email, "Cliente");

            return Ok(new LoginRespostaDTO
            {
                Id = cliente.IdCliente,
                Nome = cliente.Nome,
                Email = cliente.Email,
                TipoUsuario = "Cliente",
                Token = token
            });
        }

        var profissionais = await _profissionalRepository.ListarAsync();
        var profissional = profissionais.FirstOrDefault(p => p.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

        if (profissional != null && BCrypt.Net.BCrypt.Verify(dto.Senha, profissional.Senha))
        {
            var token = _tokenService.GerarToken(profissional.IdProfissional, profissional.Nome, profissional.Email, "Profissional");

            return Ok(new LoginRespostaDTO
            {
                Id = profissional.IdProfissional,
                Nome = profissional.Nome,
                Email = profissional.Email,
                TipoUsuario = "Profissional",
                Token = token
            });
        }

        return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
    }
}