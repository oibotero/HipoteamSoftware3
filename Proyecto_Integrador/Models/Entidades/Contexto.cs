using Proyecto_Integrador.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models
{
    public class Contexto : DbContext
    {
        
            public Contexto()
                : base("Integrador")
            {

            }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }


    }
}