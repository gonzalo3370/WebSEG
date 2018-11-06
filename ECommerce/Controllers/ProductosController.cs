using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Listado
        public ActionResult Listado(int idCategoria)
        {
            var productos = new List<Models.ProductoViewModel>();
            productos.Add(new Models.ProductoViewModel
            {
                Nombre = "Porton automático"
            });
            return View(model: productos);
        }
    }
}