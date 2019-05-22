using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models.Dac_Entidades
{
    public class Dac_Roles
    {
        private Contexto db = new Contexto();

        public string EncontrarRol(Usuarios usuario) {
            string rol= (from r in db.Roles
                         where r.Id == usuario.Rol
                         select r.Nombre).SingleOrDefault();
            return rol;
        }
    }
}