using System.Diagnostics;

namespace Logica
{
    public class Enemigo : Entidad
    {
        public Enemigo(int x, int y) : base(x, y) { }

        public void Atacar(Personaje personaje)
        {
            if (personaje != null && personaje.Vida > 0)
            {
                personaje.Vida = 0; // ✅ Vida del personaje queda en 0
            }
        }
    }
}