using System;
using System.Collections.Generic;

namespace waGimnasio.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public DateTime FechaIni { get; set; }

    public DateTime FechaFin { get; set; }

    public string? Ocupacion { get; set; }

    public int TelefonoEmergencia { get; set; }

    public int CodigoPlan { get; set; }

    public int? CodigoPromocion { get; set; }

    public int NumBoleta { get; set; }

    public virtual PlanOfertado CodigoPlanNavigation { get; set; } = null!;

    public virtual Promocion? CodigoPromocionNavigation { get; set; }

    public virtual Persona IdNavigation { get; set; } = null!;
}
