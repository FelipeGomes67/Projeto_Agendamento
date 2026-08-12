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
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IServicoRepository _servicoRepository;

        public AgendamentoController(
            IAgendamentoRepository agendamentoRepository,
            IClienteRepository clienteRepository,
            IProfissionalRepository profissionalRepository,
            IServicoRepository servicoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _clienteRepository = clienteRepository;
            _profissionalRepository = profissionalRepository;
            _servicoRepository = servicoRepository;
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var agendamentos = await _agendamentoRepository.ListarAsync();
            var resultado = agendamentos.Select(a => MapearParaRespostaDTO(a));

            return Ok(resultado);
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id);

            if (agendamento == null)
                return NotFound(new { mensagem = "Agendamento não encontrado." });

            return Ok(MapearParaRespostaDTO(agendamento));
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] AgendamentoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = await _clienteRepository.ObterPorIdAsync(dto.IdCliente);
            if (cliente == null)
                return BadRequest(new { mensagem = "Cliente não encontrado." });

            var profissional = await _profissionalRepository.ObterPorIdAsync(dto.IdProfissional);
            if (profissional == null)
                return BadRequest(new { mensagem = "Profissional não encontrado." });

            var servico = await _servicoRepository.ObterPorIdAsync(dto.IdServico);
            if (servico == null)
                return BadRequest(new { mensagem = "Serviço não encontrado." });

            if (dto.DataHoraInicio < DateTime.Now)
                return BadRequest(new { mensagem = "A data/hora de início deve ser futura." });

            if (dto.DataHoraFim <= dto.DataHoraInicio)
                return BadRequest(new { mensagem = "A data/hora de término deve ser posterior ao início." });

            var agendamentosExistentes = await _agendamentoRepository.ListarAsync();
            var conflito = agendamentosExistentes.Any(a =>
                a.IdProfissional == dto.IdProfissional &&
                a.Status != "Cancelado" &&
                dto.DataHoraInicio < a.DataHoraFim &&
                dto.DataHoraFim > a.DataHoraInicio);

            if (conflito)
                return BadRequest(new { mensagem = "O profissional já possui um agendamento neste horário." });

            var agendamento = new Agendamento
            {
                IdCliente = dto.IdCliente,
                IdProfissional = dto.IdProfissional,
                IdServico = dto.IdServico,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dto.DataHoraFim,
                Status = "Agendado"
            };

            await _agendamentoRepository.CadastrarAsync(agendamento);

            var agendamentoSalvo = await _agendamentoRepository.ObterPorIdAsync(agendamento.IdAgendamento) ?? agendamento;

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = agendamento.IdAgendamento },
                MapearParaRespostaDTO(agendamentoSalvo)
            );
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AgendamentoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id);
            if (agendamento == null)
                return NotFound(new { mensagem = "Agendamento não encontrado." });

            if (dto.DataHoraFim <= dto.DataHoraInicio)
                return BadRequest(new { mensagem = "A data/hora de término deve ser posterior ao início." });

            agendamento.IdCliente = dto.IdCliente;
            agendamento.IdProfissional = dto.IdProfissional;
            agendamento.IdServico = dto.IdServico;
            agendamento.DataHoraInicio = dto.DataHoraInicio;
            agendamento.DataHoraFim = dto.DataHoraFim;

            await _agendamentoRepository.AtualizarAsync(agendamento);

            return Ok(MapearParaRespostaDTO(agendamento));
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id);

            if (agendamento == null)
                return NotFound(new { mensagem = "Agendamento não encontrado." });

            if (agendamento.Status == "Cancelado")
                return BadRequest(new { mensagem = "Este agendamento já se encontra cancelado." });

            agendamento.Status = "Cancelado";
            await _agendamentoRepository.AtualizarAsync(agendamento);

            return Ok(new
            {
                mensagem = "Agendamento cancelado com sucesso!",
                idAgendamento = agendamento.IdAgendamento,
                agendamento.Status
            });
        }

        [Authorize(Roles = "Cliente, Profissional")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id);
            if (agendamento == null)
                return NotFound(new { mensagem = "Agendamento não encontrado." });

            await _agendamentoRepository.DeletarAsync(id);

            return Ok(new { mensagem = "Agendamento removido com sucesso!" });
        }

        private static AgendamentoRespostaDTO MapearParaRespostaDTO(Agendamento a)
        {
            return new AgendamentoRespostaDTO
            {
                IdAgendamento = a.IdAgendamento,
                DataHoraInicio = a.DataHoraInicio,
                DataHoraFim = a.DataHoraFim,
                Status = a.Status ?? "Agendado",
                Cliente = a.IdClienteNavigation != null ? new ClienteResumoDTO
                {
                    IdCliente = a.IdClienteNavigation.IdCliente,
                    Nome = a.IdClienteNavigation.Nome,
                    Email = a.IdClienteNavigation.Email
                } : null,
                Profissional = a.IdProfissionalNavigation != null ? new ProfissionalResumoDTO
                {
                    IdProfissional = a.IdProfissionalNavigation.IdProfissional,
                    Nome = a.IdProfissionalNavigation.Nome,
                    Email = a.IdProfissionalNavigation.Email
                } : null,
                Servico = a.IdServicoNavigation != null ? new ServicoResumoDTO
                {
                    IdServico = a.IdServicoNavigation.IdServico,
                    Nome = a.IdServicoNavigation.Nome,
                    Preco = a.IdServicoNavigation.Preco
                } : null
            };
        }
    }
}