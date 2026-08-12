using AgendamentoAPI.DTOs;
using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfissionalController : ControllerBase
    {
        private readonly IProfissionalRepository _profissionalRepository;

        public ProfissionalController(IProfissionalRepository profissionalRepository)
        {
            _profissionalRepository = profissionalRepository;
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var profissionais = await _profissionalRepository.ListarAsync();

            var resultado = profissionais.Select(p => new ProfissionalRespostaDTO
            {
                IdProfissional = p.IdProfissional,
                Nome = p.Nome,
                Email = p.Email,
                Disponivel = p.Disponivel,
                Telefone = p.Telefone
            });

            return Ok(resultado);
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var profissional = await _profissionalRepository.ObterPorIdAsync(id);

            if (profissional == null)
                return NotFound(new { mensagem = "Profissional não encontrado." });

            var resposta = new ProfissionalRespostaDTO
            {
                IdProfissional = profissional.IdProfissional,
                Nome = profissional.Nome,
                Email = profissional.Email,
                Disponivel = profissional.Disponivel,
                Telefone = profissional.Telefone
            };

            return Ok(resposta);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ProfissionalDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var profissional = new Profissional
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = senhaHash,
                Disponivel = dto.Disponivel,
                Telefone = dto.Telefone 
            };

            await _profissionalRepository.CadastrarAsync(profissional);

            var resposta = new ProfissionalRespostaDTO
            {
                IdProfissional = profissional.IdProfissional,
                Nome = profissional.Nome,
                Email = profissional.Email,
                Disponivel = profissional.Disponivel,
                Telefone = profissional.Telefone
            };

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = profissional.IdProfissional },
                resposta
            );
        }

        [Authorize(Roles = "Profissional")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProfissionalDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profissionalExistente = await _profissionalRepository.ObterPorIdAsync(id);
            if (profissionalExistente == null)
                return NotFound(new { mensagem = "Profissional não encontrado." });

            profissionalExistente.Nome = dto.Nome;
            profissionalExistente.Email = dto.Email;
            profissionalExistente.Disponivel = dto.Disponivel;
            profissionalExistente.Telefone = dto.Telefone; 

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                profissionalExistente.Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            }

            await _profissionalRepository.AtualizarAsync(profissionalExistente);

            var resposta = new ProfissionalRespostaDTO
            {
                IdProfissional = profissionalExistente.IdProfissional,
                Nome = profissionalExistente.Nome,
                Email = profissionalExistente.Email,
                Disponivel = profissionalExistente.Disponivel,
                Telefone = profissionalExistente.Telefone
            };

            return Ok(resposta);
        }

        [Authorize(Roles = "Profissional")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var profissional = await _profissionalRepository.ObterPorIdAsync(id);
            if (profissional == null)
                return NotFound(new { mensagem = "Profissional não encontrado." });

            await _profissionalRepository.DeletarAsync(id);

            return Ok(new { mensagem = "Profissional removido com sucesso!" });
        }
    }
}