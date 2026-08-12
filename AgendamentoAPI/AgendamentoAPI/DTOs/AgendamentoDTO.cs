using System;
using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.DTOs
{
    public class AgendamentoDTO
    {
        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "O profissional é obrigatório.")]
        public Guid IdProfissional { get; set; }

        [Required(ErrorMessage = "O serviço é obrigatório.")]
        public Guid IdServico { get; set; }

        [Required(ErrorMessage = "A data/hora de início é obrigatória.")]
        public DateTime DataHoraInicio { get; set; }

        [Required(ErrorMessage = "A data/hora de término é obrigatória.")]
        public DateTime DataHoraFim { get; set; }
    }

    public class AgendamentoRespostaDTO
    {
        public Guid IdAgendamento { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Status { get; set; } = string.Empty;

        public ClienteResumoDTO? Cliente { get; set; }
        public ProfissionalResumoDTO? Profissional { get; set; }
        public ServicoResumoDTO? Servico { get; set; }
    }

    public class ClienteResumoDTO
    {
        public Guid IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ProfissionalResumoDTO
    {
        public Guid IdProfissional { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ServicoResumoDTO
    {
        public Guid IdServico { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}