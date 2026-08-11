using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.DTOs
{
    public class ServicoDTO
    {
        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [StringLength(255, ErrorMessage = "O nome do serviço deve ter no máximo 255 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A duração do serviço é obrigatória.")]
        [Range(1, 1440, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "O preço do serviço é obrigatório.")]
        [Range(0.01, 10000.00, ErrorMessage = "Informe um preço válido maior que zero.")]
        public decimal Preco { get; set; }
    }

    public class ServicoRespostaDTO
    {
        public Guid IdServico { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal Preco { get; set; }
    }
}