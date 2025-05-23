using System.Diagnostics;

namespace Logica
{
    public class Oro : Entidad
    {
        public Oro(int x, int y) : base(x, y){}
        public void Recoger(Personaje personaje)
        {
            if (personaje != null)
            {
                personaje.Dinero += 1; // ✅ Sumar 1 al dinero del personaje
                Debug.WriteLine($"💰 Oro recogido. Nuevo dinero: {personaje.Dinero}");

            }
        }


    }

}