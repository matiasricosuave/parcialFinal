using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace waGimnasio.Models;

public partial class Usuario
{
    public int Id { get; set; }

    [Display(Name = "Correo Electronico")]
    [Required(ErrorMessage = "El campo Correo Electrónico es obligatorio.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
    public string Password { get; set; } = null!;

    public bool? Estado { get; set; }

    public virtual Persona IdNavigation { get; set; } = null!;
}
