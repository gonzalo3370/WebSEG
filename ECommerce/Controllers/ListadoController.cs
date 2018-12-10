using ECommerce.DATOS;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class ListadoController : Controller
    {
        /// <summary>
        /// Obtiene los productos según el idCategoria indicado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Lista de productos</returns>
        public ActionResult Productos(int Id)
        {
            var Consultas = new Consultas();
            var res = (from p in Consultas.ProductosPorCategoria(Id)
                       select new ProductoViewModel
                       {
                           IdProducto = p.IdProducto,
                           IdCategoria = p.IdCategoria,
                           Nombre = p.Nombre,
                           DescripcionBreve = p.DescripcionBreve,
                           IdImagen = (
                                from i in p.Imagenes
                                orderby i.Orden
                                select i
                            ).FirstOrDefault()?.IdImagen ?? 0,
                           Precio = (double?)p.Precio
                       }).ToList();
            return View(viewName: "Index", model: res);
        }

        public ActionResult Search(string Texto)
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.SearchProducto(Texto)
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdProducto,
                          IdCategoria = p.IdCategoria,
                          Nombre = p.Nombre,
                          DescripcionBreve = p.DescripcionBreve
                      };
            return View(viewName: "Index", model: res);

        }
    }
}