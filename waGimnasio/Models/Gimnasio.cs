using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace waGimnasio.Models
{
    public partial class Gimnasio
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = null!;
        public int Telefono { get; set; }
        public string Direccion { get; set; } = null!;
        public int IdDepartamento { get; set; }

        [ForeignKey("IdDepartamento")]
        public virtual Departamento? IdDepartamentoNavigation { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
    }
}
