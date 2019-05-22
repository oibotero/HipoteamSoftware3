using Proyecto_Integrador.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models.Dac_Entidades
{
    public class Dac_Productos
    {
        private Contexto db = new Contexto();

        public List<Productos> ListaProductos()
        {
            return db.Productos.ToList();
        }
        public void InsertaProducto(Productos p)
        {
            db.Productos.Add(p);
            db.SaveChanges();
        }

        public int NumberProducts() {
            return  db.Productos.ToList().Count();
            
        }

        public Productos EncontrarProducto(int? id)
        {
            return db.Productos.Find(id);
        }

        public void EliminarProducto(Productos producto)
        {
            db.Productos.Remove(producto);
            db.SaveChanges();
        }

        public void ActualizarProducto(Productos producto)
        {
            if (producto.Figura == null)
            {
                producto.Figura = db.Productos.Where(x => x.Id == producto.Id).Select(x => x.Figura).FirstOrDefault();
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
            }
            else {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
            }
           
        }

        public bool ExisteImagen(string imagen) {
            bool result = false;
            db.Productos.ToList().ForEach(X=> {
                if (X.Figura.Equals(imagen))
                {
                    result = true;
                }
            });
            return result;
        }

        public int ValorTotal(List<int> productos) {
            int result = 0;
            productos.ForEach(x =>
            {
                result += db.Productos.Where(y => y.Id == x).Select(y => y.Precio).FirstOrDefault();
            });
            return result;
        }

        public int ValorProducto(int productoId)
        {
            int result =db.Productos.Where(y => y.Id == productoId).Select(y => y.Precio).FirstOrDefault();
            return result;
        }

        public List<ModelProductoVendido> ProductosVendidos()
        {
            List<ModelProductoVendido> result = new List<ModelProductoVendido>();
            List<string> lista = new List<string>();
            db.Pedidos.ToList().ForEach(x => {
            x.Productos.ToList().ForEach(y =>  lista.Add(y.Nombre) );
            });
            var resp = lista.GroupBy(x => x);
            foreach (var grouping in resp)
            {
                result.Add(new ModelProductoVendido {label= grouping.Key,y = grouping.Count() });
               
            }
            
            return result;
        }
        public List<ModelProductoVendido> DisponibilidadProductos()
        {
            List<ModelProductoVendido> result = new List<ModelProductoVendido>();
            db.Productos.ToList().ForEach(x => {
                result.Add(new ModelProductoVendido {y=x.Cantidad,label=x.Nombre });
            });

            return result;
        }
    }
}