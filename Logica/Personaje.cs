using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

namespace Logica
{
    public class Personaje : Entidad
    {
        public int Dinero { get; set; }
        public int Vida { get; set; }
        public int Pokeball { get; set; }

        public Personaje(int x, int y, int dinero, int vida, int pokeball) : base(x, y)
        {
            Dinero = dinero;
            Vida = vida;
            Pokeball = pokeball;
        }
        public event Action<int> OroRecogido;
        public event Action<int> VidaPerdida;

        public void Ejecutar(Mapa mapa)
        {
            Entidad entidad = mapa.ObtenerEntidad(X, Y);

            if (entidad is Oro oro)
            {
                oro.Recoger(this);
                OroRecogido?.Invoke(Dinero); // ✅ Notificar cambio de oro
            }
            else if (entidad is Trampa trampa)
            {
                trampa.Activar(this);
                VidaPerdida?.Invoke(Vida); // ✅ Notificar pérdida de vida
            }
            else if (entidad is Enemigo enemigo)
            {
                enemigo.Atacar(this);
                VidaPerdida?.Invoke(Vida); // ✅ Notificar pérdida de vida
            }

            Debug.WriteLine($"🔹 Acción ejecutada en [{X}, {Y}] con entidad {entidad?.GetType().Name}");
        }

        public void Moverse(int deltaX, int deltaY, Mapa mapa)
        {
            Debug.WriteLine($"🔍 Antes de mover: X={X}, Y={Y}");
            int nuevoX = X + deltaX;
            int nuevoY = Y + deltaY;

            // Verificar si la nueva posición es válida dentro del mapa
            if (nuevoX >= 0 && nuevoX < mapa.x && nuevoY >= 0 && nuevoY < mapa.y)
            {
                X = nuevoX;
                Y = nuevoY;
                Debug.WriteLine($"✅ Después de mover: X={X}, Y={Y}");
                Ejecutar(mapa);
            }
            else
            {
                Debug.WriteLine($"🚫 Movimiento inválido a X={nuevoX}, Y={nuevoY}");
            }

        }

        public void Lanzar(Mapa mapa, Action<int, int> actualizarVisual, string direccion)
        {
            if (Pokeball <= 0)
            {
                Debug.WriteLine("⚠️ No hay bolas en el inventario.");
                return;
            }

            int dx = 0, dy = 0;
            switch (direccion)
            {
                case "Arriba": dy = -1; break;
                case "Abajo": dy = 1; break;
                case "Izquierda": dx = -1; break;
                case "Derecha": dx = 1; break;
            }

            int x = X, y = Y;
            Pokeball--; // 🔹 Restar una bola al inventario

            while (mapa.EsDentroDeLimite(x + dx, y + dy)) // 🔹 Mientras no choque con el borde
            {
                x += dx;
                y += dy;
                mapa.SimularBola(x, y, actualizarVisual); // ✅ Solo gestiona la lógica y envía la actualización visual
                Thread.Sleep(100);
            }

            Debug.WriteLine($"🎯 Bola lanzada en dirección {direccion}, finalizando en [{x}, {y}]");
        }
    }
}
