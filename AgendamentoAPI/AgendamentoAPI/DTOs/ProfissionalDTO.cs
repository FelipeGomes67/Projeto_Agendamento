using System.ComponentModel.DataAnnotations;

namespace AgendamentoAPI.DTOs
{
    public class ProfissionalDTO
    {
        [Required(ErrorMessage = "O nome do profissional é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        public bool Disponivel { get; set; } = true;

        [Required(ErrorMessage = "O e-mail do profissional é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do profissional é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }

    public class ProfissionalRespostaDTO
    {
        public Guid IdProfissional { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Disponivel { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}