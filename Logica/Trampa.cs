using System.Diagnostics;

namespace Logica
{
    public class Trampa : Entidad
    {
        public Trampa(int x, int y) : base(x, y){}
        public void Activar(Personaje personaje)
        {
            if (personaje != null && personaje.Vida > 0)
            {
                personaje.Vida -= 1; // ✅ Quitar 1 de vida
            }
        }

    }

}
