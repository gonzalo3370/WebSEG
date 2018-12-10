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
    public class ProductoController : Controller
    {
        public ActionResult Producto( int Id)
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.GetProducto(Id)
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdProducto,
                          IdCategoria = p.IdCategoria,
                          Nombre = p.Nombre,
                          DescripcionBreve = p.DescripcionBreve,
                          Precio = (double?)p.Precio,
                          IdImagen = (
                                from i in p.Imagenes
                                orderby i.Orden
                                select i
                            ).FirstOrDefault()?.IdImagen ?? 1,
                      };
            return View(viewName: "index", model: res.FirstOrDefault());
        }
    }
}