using AgendamentoAPI.DTOs;
using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly IServicoRepository _servicoRepository;

        public ServicoController(IServicoRepository servicoRepository)
        {
            _servicoRepository = servicoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var servicos = await _servicoRepository.ListarAsync();

            var resultado = servicos.Select(s => new
            {
                s.IdServico,
                s.Nome,
                s.Descricao,
                s.Preco
            });

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);

            if (servico == null)
                return NotFound(new { mensagem = "Serviço não encontrado." });

            return Ok(new
            {
                servico.IdServico,
                servico.Nome,
                servico.Descricao,
                servico.Preco
            });
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ServicoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var servico = new Servico
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco
            };

            await _servicoRepository.CadastrarAsync(servico);

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = servico.IdServico },
                new
                {
                    mensagem = "Serviço cadastrado com sucesso!",
                    idServico = servico.IdServico,
                    servico.Nome,
                    servico.Descricao,
                    servico.Preco
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ServicoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var servicoExistente = await _servicoRepository.ObterPorIdAsync(id);
            if (servicoExistente == null)
                return NotFound(new { mensagem = "Serviço não encontrado." });

            servicoExistente.Nome = dto.Nome;
            servicoExistente.Descricao = dto.Descricao;
            servicoExistente.Preco = dto.Preco;

            await _servicoRepository.AtualizarAsync(servicoExistente);

            return Ok(new
            {
                mensagem = "Serviço atualizado com sucesso!",
                servicoExistente.IdServico,
                servicoExistente.Nome,
                servicoExistente.Descricao,
                servicoExistente.Preco
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound(new { mensagem = "Serviço não encontrado." });

            await _servicoRepository.DeletarAsync(id);

            return Ok(new { mensagem = "Serviço removido com sucesso!" });
        }
    }
}