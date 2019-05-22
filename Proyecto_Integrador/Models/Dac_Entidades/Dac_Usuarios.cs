using Proyecto_Integrador.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models.Dac_Entidades
{
    public class Dac_Usuarios
    {
        private Contexto db = new Contexto();
        private  Usuarios usuarios = new Usuarios();
        private ServiceUsuarios ServiceUsuario = new ServiceUsuarios();

        public List<Usuarios> ListaUsuarios()
        {
            var usuarios = db.Usuarios.Include(u => u.Roles);
            return db.Usuarios.ToList();
        }

        public Usuarios EncontrarUsuario(int? id)
        {
            return db.Usuarios.Find(id);
        }

        public void GuardarUsuario(Usuarios Usuario)
        {
            Usuario.confirmado = 0;
            db.Usuarios.Add(Usuario);
            db.SaveChanges();
        }

        public void ActualizarUsuario(Usuarios Usuario)
        {
            db.Entry(Usuario).State = EntityState.Modified;
            db.SaveChanges();
            
  
        }

        public void EliminarUsuario(Usuarios Usuario)
        {
            db.Usuarios.Remove(Usuario);
            db.SaveChanges();
        }

        public Usuarios LoginUsuario(Login log)
        {
            
            log.Password = ServiceUsuario.Encripta(log.Password);
            Usuarios result = (Usuarios)(from u in db.Usuarios
                                         where u.Correo == log.Correo && u.Password == log.Password && u.confirmado==1
                                         select u).SingleOrDefault();
            return result;
        }

        public void Cerrar() {
            db.Dispose();
        }
        public void Confirmar(int id)
        {
           Usuarios usuario= db.Usuarios.Find(id);
            usuario.confirmado = 1;
            db.SaveChanges();

        }
    }
}
