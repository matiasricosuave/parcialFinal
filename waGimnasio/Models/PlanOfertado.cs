using System;
using System.Collections.Generic;

namespace waGimnasio.Models;

public partial class PlanOfertado
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int Precio { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
