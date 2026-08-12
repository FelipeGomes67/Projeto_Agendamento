using AgendamentoAPI.DTOs;
using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var clientes = await _clienteRepository.ListarAsync();

            var resultado = clientes.Select(c => new ClienteRespostaDTO
            {
                IdCliente = c.IdCliente,
                Nome = c.Nome,
                Telefone = c.Telefone,
                Email = c.Email
            });

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            var resposta = new ClienteRespostaDTO
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                Email = cliente.Email
            };

            return Ok(resposta);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Senha = senhaHash
            };

            await _clienteRepository.CadastrarAsync(cliente);

            var resposta = new ClienteRespostaDTO
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                Email = cliente.Email
            };

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = cliente.IdCliente },
                resposta
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ClienteDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteExistente = await _clienteRepository.ObterPorIdAsync(id);
            if (clienteExistente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            clienteExistente.Nome = dto.Nome;
            clienteExistente.Telefone = dto.Telefone;
            clienteExistente.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                clienteExistente.Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            }

            await _clienteRepository.AtualizarAsync(clienteExistente);

            var resposta = new ClienteRespostaDTO
            {
                IdCliente = clienteExistente.IdCliente,
                Nome = clienteExistente.Nome,
                Telefone = clienteExistente.Telefone,
                Email = clienteExistente.Email
            };

            return Ok(resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            await _clienteRepository.DeletarAsync(id);

            return Ok(new { mensagem = "Cliente removido com sucesso!" });
        }
    }
}