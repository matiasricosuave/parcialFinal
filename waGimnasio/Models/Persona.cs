using System;
using System.Collections.Generic;

namespace waGimnasio.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string Ci { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int Telefono { get; set; }

    public string? Email { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public int CodigoGimnasio { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Gimnasio CodigoGimnasioNavigation { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
