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
        public void Imagen(int id)
        {
            byte[] bytes = id == 0
                   ? System.IO.File.ReadAllBytes(Server.MapPath("/content/images/Xbox360.jpg"))
                   : new Consultas().Imagen(id);
            Response.ContentType = "image/png";
            Response.Clear();
            Response.BufferOutput = true;
            Response.BinaryWrite(bytes);
            Response.Flush();
        }
    }
}