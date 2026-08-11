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

        [Required(ErrorMessage = "A data e hora de início são obrigatórias.")]
        public DateTime DataHoraInicio { get; set; }
    }

    public class AgendamentoRespostaDTO
    {
        public Guid IdAgendamento { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string NomeProfissional { get; set; } = string.Empty;
        public string NomeServico { get; set; } = string.Empty;
        public decimal PrecoServico { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}