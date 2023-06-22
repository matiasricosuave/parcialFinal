using System;
using System.Collections.Generic;

namespace waGimnasio.Models;

public partial class Departamento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Gimnasio> Gimnasios { get; set; } = new List<Gimnasio>();
}
