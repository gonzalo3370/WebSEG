using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.DATOS;

namespace ECommerce.Controllers
{
    public class ConsultasController : Controller
    {
        public ActionResult TodasLasCategorias()
        {
            var Consultas = new Consultas();
            var res = from c in Consultas.GetCategorias()
                      select new CategoriaViewModel
                      {
                          IdCategoria = c.IdCategoria,
                          Nombre = c.Nombre
                      };
            return View(viewName:"Categorias", model: res.LastOrDefault());
        }
        public ActionResult TodosLosProductos()
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.GetProductos()
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdProducto,
                          IdCategoria = p.IdCategoria,
                          Nombre = p.Nombre,
                          Descripcion = p.Descripcion
                      };
            return View(viewName: "Productos", model: res.LastOrDefault());
        }
        public ActionResult Categoria (int Id)
        {
            var Consultas = new Consultas();
            var res = from c in Consultas.GetCategoria(Id)
                      select new CategoriaViewModel
                      {                                                
                          IdCategoria = c.IdCategoria,
                          Nombre = c.Nombre
                      };
            return View(viewName: "Categoria", model: res.FirstOrDefault());
        }
        public ActionResult Producto(int Id)
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.GetProducto(Id)
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdProducto,
                          IdCategoria = p.IdCategoria,
                          Nombre = p.Nombre,
                          Descripcion = p.Descripcion
                      };
            return View(viewName: "Producto", model: res.FirstOrDefault());
        }
        public ActionResult Search (string Texto)
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.SearchProducto(Texto)
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdProducto,
                          IdCategoria = p.IdCategoria,
                          Nombre = p.Nombre,
                          Descripcion = p.Descripcion
                      };
            return View(viewName: "Search", model: res.FirstOrDefault());

        }

        public ActionResult Productos (int Id)
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.ProductosPorCategoria(Id)
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdProducto,
                          IdCategoria = p.IdCategoria,
                          Nombre = p.Nombre,
                          Descripcion = p.Descripcion
                      };
            return View(viewName: "Producto", model: res.FirstOrDefault());
        }

        public void Imagen(int id)
        {
            var bytes = new Consultas().Imagen(id);
            Response.ContentType = "image/png";
            Response.Clear();
            Response.BufferOutput = true;
            Response.BinaryWrite(bytes);
            Response.Flush();
        }
    }
}

