using Microsoft.AspNet.Identity;
using Proyecto_Integrador.Models;
using Proyecto_Integrador.Models.Dac_Entidades;
using Proyecto_Integrador.Models.Entidades;
using Rotativa;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        Dac_Pedidos Dac_Pedidos = new Dac_Pedidos();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(List<int>productos,string direccion)
        {
           string usuario= User.Identity.GetUserId();
            bool result=Dac_Pedidos.insertar(productos, usuario,direccion);
            return Json(result);
        }

        public ActionResult listaPedidos()
        {
            List<Pedidos> resultado = new List<Pedidos>();
            var rol = User.Identity.GetUserName().Split('-');
            if (rol[1].Equals("Administrador"))
            {
                resultado= Dac_Pedidos.listaPedidos("Administrador",null);
            }
            else {
                resultado = Dac_Pedidos.listaPedidos("Administrador", int.Parse(User.Identity.GetUserId()));
            }

            return View(resultado);
        }
        [HttpPost]
        public ActionResult ActualizarEstadoPedido(int pedidoId, string estado) {
           bool resultado= Dac_Pedidos.ActualizarEstadoPedido(pedidoId,estado);
            return Json(resultado);
        }

        public ActionResult DetallePedido(int pedidoId) {
            Dac_Usuarios dac_Usuarios = new Dac_Usuarios();
            Pedidos resultado = Dac_Pedidos.ConsultarPedido(pedidoId);
            Usuarios usuario = dac_Usuarios.EncontrarUsuario(resultado.Usuario);
            ViewData["usuario"] = usuario;
            return View(resultado);
        }

        public ActionResult PrintPartialViewToPdf(int pedidoId)
        {
            Dac_Usuarios dac_Usuarios = new Dac_Usuarios();
            Pedidos resultado = Dac_Pedidos.ConsultarPedido(pedidoId);
            Usuarios usuario = dac_Usuarios.EncontrarUsuario(resultado.Usuario);
            ViewData["usuario"] = usuario;
            var report = new PartialViewAsPdf("~/Views/Pedidos/PrintPartialViewToPdf.cshtml", resultado);
            return report;


        }
    }
}