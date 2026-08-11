using System;
using System.Collections.Generic;

namespace AgendamentoAPI.Models;

public partial class Agendamento
{
    public Guid IdAgendamento { get; set; }

    public Guid IdCliente { get; set; }

    public Guid IdProfissional { get; set; }

    public Guid IdServico { get; set; }

    public DateTime DataHoraInicio { get; set; }

    public DateTime DataHoraFim { get; set; }

    public string Status { get; set; } = null!;

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Profissional IdProfissionalNavigation { get; set; } = null!;

    public virtual Servico IdServicoNavigation { get; set; } = null!;
}
