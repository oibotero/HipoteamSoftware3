using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models
{
    public class Usuarios
    {

        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Documento { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        public int Rol { get; set; }
        [ForeignKey("Rol")]
        public virtual Roles Roles { get; set; }
        public int confirmado { get; set; }
    }
}