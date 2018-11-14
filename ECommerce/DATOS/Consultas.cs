using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.DATOS
{
    public class Consultas
    {
        public IList<Categoria> GetCategorias()
        {
            using (var context = new SEGEntities())
            {
                var cat = (from c in context.Categorias select c).ToList();
                return cat;
            }
        }
        public IList<Producto> GetProductos()
        {
            using (var context = new SEGEntities())
            {
                var pro = (from p in context.Productos select p).ToList();
                return pro;
            }
        }
        public IList<Categoria> GetCategoria(int Id)
        {
            using (var context = new SEGEntities())
            {
                return (
                    from c in context.Categorias
                    where c.IdCategoria == Id
                    select c
                    ).ToList();
            }
        }
        public IList<Producto> GetProducto(int Id)
        {
            using (var context = new SEGEntities())
            {
                return (
                    from c in context.Productos
                    where c.IdProducto == Id
                    select c
                    ).ToList();
            }
        }
        public IList<Producto> ProductosPorCategoria (int Id)
        {
            using (var context = new SEGEntities())
            {
                return (
                    from p in context.Productos
                    where p.IdCategoria == Id
                    select p
                    ).ToList();
            }

        }
        public IList<Producto> SearchProducto(string texto)
        {
            return (from p in new SEGEntities().Productos
                    where p.Nombre.Contains(texto)
                    select p
                   ).ToList();
        }

        public byte[] Imagen(int id)
        {
            return (
                    from i in new SEGEntities().Imagenes
                    where i.IdImagen == id
                    select i.Imagen
                   ).Single();
        }
    }
}
