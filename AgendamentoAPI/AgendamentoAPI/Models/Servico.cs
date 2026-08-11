using System;
using System.Collections.Generic;

namespace AgendamentoAPI.Models;

public partial class Servico
{
    public Guid IdServico { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public int DuracaoMinutos { get; set; }

    public decimal Preco { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
