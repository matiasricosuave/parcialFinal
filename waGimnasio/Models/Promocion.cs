using System;
using System.Collections.Generic;

namespace waGimnasio.Models;

public partial class Promocion
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Descuento { get; set; }

    public DateTime FechaIni { get; set; }

    public DateTime FechaFin { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
