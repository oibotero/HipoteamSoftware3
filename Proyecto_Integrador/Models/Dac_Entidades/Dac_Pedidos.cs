using Proyecto_Integrador.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Integrador.Models.Dac_Entidades
{
    public class Dac_Pedidos
    {
        private Contexto db = new Contexto();

        public bool insertar(List<int>idProductos,string usuario,string direccion) {
            Dac_Productos dac_Productos = new Dac_Productos();
            Pedidos pedido = new Pedidos();
            List<Productos> productos = new List<Productos>();
            idProductos.ForEach(x => {
                productos.Add(db.Productos.Where(y =>y.Id==x).FirstOrDefault());
            });
            bool resultado = false;
            try
            {
                pedido.DireccionEntrega = direccion;
                pedido.Usuario = int.Parse(usuario);
                pedido.Estado = "Pendiente";
                pedido.FechaSolicitud = DateTime.Now;
                pedido.Productos=productos;
                pedido.PrecioTotal = dac_Productos.ValorTotal(idProductos);
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                idProductos.ForEach(x => {
                    Productos p = db.Productos.Find(x);
                    p.Cantidad = p.Cantidad - 1;
                    db.SaveChanges();
                });
                resultado = true;
            }
            catch (Exception)
            {

                throw;
            }

            return resultado;
        }

        public List<Pedidos> listaPedidos(string rol,int? id) {
            List<Pedidos> resultado = new List<Pedidos>();
            if (rol.Equals("Administrador"))
            {
                resultado = db.Pedidos.ToList();
            }
            else {
                resultado = db.Pedidos.Where(x => x.Usuario == id.Value).ToList();

            }
            return resultado;
        }
        public bool ActualizarEstadoPedido(int pedidoId, string estado) {
            bool resultado;
            try
            {
                Pedidos pedido = db.Pedidos.Find(pedidoId);
                pedido.Estado = estado;
                db.SaveChanges();
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }

        public Pedidos ConsultarPedido(int pedidoId) {
            return db.Pedidos.Find(pedidoId);
        }
    }
}