using Newtonsoft.Json;
using Proyecto_Integrador.Models.Dac_Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        Dac_Productos Dac_Pedidos = new Dac_Productos();
        public ActionResult ProductoVendido()
        {
            ViewBag.Disponibilidad = JsonConvert.SerializeObject(Dac_Pedidos.DisponibilidadProductos());
            ViewBag.DataPoints = JsonConvert.SerializeObject(Dac_Pedidos.ProductosVendidos());
            return View();
        }
    }
}