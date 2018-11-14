using ECommerce.DATOS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class ImagenesController : Controller
    {
        // GET: Imagenes
        public void Imagen(int idImagen)
        {
            var bytes = new Consultas().Imagen(idImagen);
            Response.ContentType = "image/png";
            Response.Clear();
            Response.BufferOutput = true;
            Response.BinaryWrite(bytes);
            Response.Flush();
        }
    }
}