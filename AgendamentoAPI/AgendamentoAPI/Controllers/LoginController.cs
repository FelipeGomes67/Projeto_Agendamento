using AgendamentoAPI.DTOs;
using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly AgendamentoDbContext _context;

    public LoginController(AgendamentoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == dto.Email);

        if (cliente != null)
        {
            bool senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, cliente.Senha);
            if (senhaValida)
            {
                return Ok(new
                {
                    mensagem = "Login realizado com sucesso!",
                    tipoUsuario = "Cliente",
                    usuario = new
                    {
                        id = cliente.IdCliente,
                        nome = cliente.Nome,
                        email = cliente.Email
                    }
                });
            }
        }

        var profissional = await _context.Profissionais
            .FirstOrDefaultAsync(p => p.Email == dto.Email);

        if (profissional != null)
        {
            bool senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, profissional.Senha);
            if (senhaValida)
            {
                return Ok(new
                {
                    mensagem = "Login realizado com sucesso!",
                    tipoUsuario = "Profissional",
                    usuario = new
                    {
                        id = profissional.IdProfissional,
                        nome = profissional.Nome,
                        email = profissional.Email
                    }
                });
            }
        }

        return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
    }
}