using Newtonsoft.Json;
using Proyecto_Integrador.Models;
using Proyecto_Integrador.Models.Dac_Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        Dac_Productos Dac_Productos = new Dac_Productos();
        public ActionResult Index()
        {
            return View(Dac_Productos.ListaProductos());
        }

        
        public ActionResult Crear()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string nombre,int precio,int cantidad,string descripcion, HttpPostedFileBase photo)
        {
            Productos producto = new Productos
            {
                Nombre = nombre,
                Precio = precio,
                Cantidad = cantidad,
                Descripcion= descripcion
            };
            if (photo != null)
            {
                   producto.Figura= SaveFile(photo);
                   Dac_Productos.InsertaProducto(producto);
            }
            
             return RedirectToAction("Index");

        }


       public  string SaveFile(HttpPostedFileBase file)
        {
            var targetLocation = Server.MapPath("~/Images/");
            string path = "";
            try
            {
                file.ContentType.ToString();
                path = Path.Combine(targetLocation,file.FileName);
                if (!Dac_Productos.ExisteImagen(path))
                {
                    file.SaveAs(path);
                }
                
            }
            catch
            {
                Response.StatusCode = 400;
            }
            return file.FileName;
        }

        public int IndexProduct() {
            return Dac_Productos.NumberProducts();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos producto = Dac_Productos.EncontrarProducto(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos producto = Dac_Productos.EncontrarProducto(id);
            Dac_Productos.EliminarProducto(producto);
            return RedirectToAction("Index");
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos producto = Dac_Productos.EncontrarProducto(id); 
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = id;
            return View(producto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string nombre, int precio, int cantidad, int id, HttpPostedFileBase photo)
        {
            Productos producto = null;
            if (ModelState.IsValid)
            {

                if (photo != null)
                {
                    producto=new Productos
                    {
                        Id = id,
                        Nombre = nombre,
                        Precio = precio,
                        Cantidad = cantidad,
                        Figura = photo.FileName

                    };
                    Dac_Productos.ActualizarProducto(producto);
                    SaveFile(photo);
                }
                else {
                    producto = new Productos
                    {
                        Id = id,
                        Nombre = nombre,
                        Precio = precio,
                        Cantidad = cantidad

                    };
                    Dac_Productos.ActualizarProducto(producto);
                }

                
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        [AllowAnonymous]
        public ActionResult IndexUsuario() {

            return View(Dac_Productos.ListaProductos());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos producto = Dac_Productos.EncontrarProducto(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

    }
}