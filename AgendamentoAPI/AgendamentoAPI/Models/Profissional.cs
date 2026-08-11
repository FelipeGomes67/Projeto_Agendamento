using System;
using System.Collections.Generic;

namespace AgendamentoAPI.Models;

public partial class Profissional
{
    public Guid IdProfissional { get; set; }

    public string Nome { get; set; } = null!;

    public decimal? Preco { get; set; }

    public bool Disponivel { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
