using System;
using System.Collections.Generic;

namespace AgendamentoAPI.Models;

public partial class Profissional
{
    public Guid IdProfissional { get; set; }

    public string Nome { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public bool Disponivel { get; set; }

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
