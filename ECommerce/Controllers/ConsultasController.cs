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
        /*public IEnumerable<CategoriaDTO> GetCategorias()
        {
            var categoriasConsultas = new CategoriasConsultas();
            var res = from c in categoriasConsultas.ObtenerCategorias()
                      select new CategoriaDTO
                      {
                          IdCategoria = c.IdCategoria,
                          Nombre = c.Nombre
                      };
            return res?.ToList();
        }*/

        public ActionResult Categoria()
        {
            var categoria = new List<Models.ProductoDTO>();
            categoria.Add(new Models.ProductoDTO
                      {
                          IdCategoria = 2,
                          Nombre = "Nombre"
                      });
            return View(model: categoria);
        }
    }
}