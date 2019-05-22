using Proyecto_Integrador.Models.Dac_Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult QuienesSomos()
        {
            return View();
        }
        public ActionResult Mision()
        {
            return View();
        }
        public ActionResult Contacto()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Confirmacion(int iduser)
        {
            Dac_Usuarios dac_Usuarios = new Dac_Usuarios();
            dac_Usuarios.Confirmar(iduser);
            return View();
        }
    }
}