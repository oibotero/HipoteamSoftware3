using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models.Entidades
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int Usuario { get; set; }
        public string DireccionEntrega { get; set; }
        public int PrecioTotal { get; set; }
        public DateTime FechaSolicitud { get; set; }
        [StringLength(100)]
        public string Estado { get; set; }
        [ForeignKey("Usuario")]
        public virtual Usuarios Usuarios { get; set; }
        public virtual ICollection<Productos> Productos { get; set; }
    }
}