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
        public ActionResult Productos()
        {
            var Consultas = new Consultas();
            var res = from p in Consultas.GetProductos()
                      select new ProductoViewModel
                      {
                          IdProducto = p.IdCategoria,
                          Nombre = p.Nombre,
                          Descripcion = p.Descripcion,
                          
                      };
            return View(viewName: "../Productos/index", model: res.LastOrDefault());
        }
        /// <summary>
        /// TODO: Hacer el action y crear la vista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /*public ActionResult Index (int id)
        {
        }*/
    }
}