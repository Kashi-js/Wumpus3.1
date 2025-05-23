using Logica;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Wumpus3._1.Visual
{
    public class LogJuego
    {
        private FormLog formLog; // 🔹 Referencia a la ventana del log
        private Mapa mapa;
        private TableLayoutPanel tablaMapa;

        public LogJuego(Mapa mapa, FormLog log, TableLayoutPanel tablaMapa)
        {
            this.formLog = log;
            this.mapa = mapa;
            this.tablaMapa = tablaMapa;
        }

        public void ActualizarLog(int x, int y)
        {
            if (formLog == null || tablaMapa == null)
            {
                Debug.WriteLine("⚠️ Error: `formLog` o `tablaMapa` es null.");
                return;
            }

            // 🔹 Obtener la imagen desde `tablaMapa`
            Control entidadVisual = tablaMapa.GetControlFromPosition(x, y);

            if (entidadVisual is PictureBox pictureBox)
            {
                formLog.pictureBox1.Image = pictureBox.Image; // 🔹 Asignar imagen directamente al log
                Debug.WriteLine($"✅ Imagen extraída desde `tablaMapa` en [{x}, {y}]");
            }
            else
            {
                Debug.WriteLine($"⚠️ No se encontró un PictureBox en [{x}, {y}].");
                formLog.pictureBox1.Image = null; // 🔹 Limpiar imagen si no hay entidad
            }

            // 🔹 Obtener la posición actual del personaje y la letra en el bloc
            Personaje jugador = mapa.ObtenerPersonaje();
            if (jugador == null)
            {
                Debug.WriteLine("⚠️ Error: No se encontró al personaje en `mapa`.");
                return;
            }

            string letraCelda = mapa.ObtenerCelda(jugador.X, jugador.Y);

            if (string.IsNullOrEmpty(letraCelda))
            {
                Debug.WriteLine($"⚠️ No se encontró información en el bloc para la celda [{jugador.X}, {jugador.Y}].");
                return;
            }

            // 🔹 Generar mensaje según la letra en el bloc
            string mensaje = ObtenerMensajePorLetra(letraCelda);

            formLog.AgregarMensaje(letraCelda, mensaje);
            Debug.WriteLine($"✅ Mensaje agregado al log según la celda [{jugador.X}, {jugador.Y}]: [{letraCelda}] {mensaje}");
        }

        private string ObtenerMensajePorLetra(string letraCelda)
        {
            return letraCelda switch
            {
                "S" => "[LOG]: ☠️ Peligro: Se siente un fuerte olor!",
                "B" => "[LOG]: 🌪 Sientes una fuerte brisa.",
                "T" => "[LOG]: ⚠️ Has caído en una trampa, perdiste una vida!",
                "W" => "[LOG]: ⚠️ OH NO!, El wumpus te ha eliminado!",
                "G" => "[LOG]: 💰 Oro encontrado!",
                "K" => "[LOG]: ✨ Ves un resplandor cercano!",
                _ => "[LOG]: Todo parece tranquilo"
            };
        }
    }
}