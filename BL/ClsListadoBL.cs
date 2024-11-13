using ENT;
using DAL;

namespace BL
{
    public class ClsListadoBL
    {
        public static List<clsPersona> listadoPersonas()
        {
            return ClsListadoDAL.listadoPersonas();
        }
    }
}
