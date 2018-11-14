using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }        
        public int IdImagen { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}