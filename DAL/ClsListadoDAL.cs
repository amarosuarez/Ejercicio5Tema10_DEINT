using ENT;

namespace DAL
{
    public class ClsListadoDAL
    {
        public static List<clsPersona> listadoPersonas()
        {
            List<clsPersona> lista = new List<clsPersona>();

            lista.Add(new clsPersona("Juan", "Martínez López", DateTime.Parse("10/07/1985"),
                "https://thispersondoesnotexist.com", "Calle del Sol", 611223344));
            lista.Add(new clsPersona("Laura", "González Pérez", DateTime.Parse("15/02/1992"),
                "https://thispersondoesnotexist.com", "Avenida de la Paz", 654321678));
            lista.Add(new clsPersona("Carlos", "Sánchez Rodríguez", DateTime.Parse("03/11/1987"),
                "https://thispersondoesnotexist.com", "Calle de la Luna", 676789012));
            lista.Add(new clsPersona("Isabel", "García Martínez", DateTime.Parse("20/06/1995"),
                "https://thispersondoesnotexist.com", "Calle del Mar", 623456789));
            lista.Add(new clsPersona("Fernando", "López García", DateTime.Parse("02/05/1982"),
                "https://thispersondoesnotexist.com", "Plaza Mayor", 612345987));
            lista.Add(new clsPersona("Marta", "Torres Ruiz", DateTime.Parse("12/09/1990"),
                "https://thispersondoesnotexist.com", "Avenida de los Reyes", 634567543));


            return lista;
        }
    }
}
