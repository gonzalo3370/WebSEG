using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.DATOS
{
    public class CategoriasConsultas
    {
        public IEnumerable<Categoria> ObtenerCategorias()
        {
            using (var context = new SEGEntities())
            {
                var res = (from c in context.Categorias select c).ToList();
                return res;
            }
        }
    }
}