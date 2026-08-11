using System;
using System.Collections.Generic;

namespace AgendamentoAPI.Models;

public partial class Cliente
{
    public Guid IdCliente { get; set; }

    public string Nome { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
