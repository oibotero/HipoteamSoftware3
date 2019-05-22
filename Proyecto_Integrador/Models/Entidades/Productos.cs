using Proyecto_Integrador.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models
{
    public class Productos
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        public int Precio { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public string Figura { get; set; }
        [Required]
        public string Descripcion { get; set; }

        public virtual ICollection<Pedidos>Pedido { get; set; }
    }
}