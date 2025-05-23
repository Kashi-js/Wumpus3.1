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
        private Dictionary<(int, int), PictureBox> elementosOcultos; // 🔹 Agregar propiedad
        private PictureBox personajeVisual; // 🔹 Agregar propiedad


        public LogJuego(Mapa mapa, FormLog log, TableLayoutPanel tablaMapa, Dictionary<(int, int), PictureBox> elementosOcultos, PictureBox personajeVisual)
        {
            this.formLog = log;
            this.mapa = mapa;
            this.tablaMapa = tablaMapa;
            this.elementosOcultos = elementosOcultos; // ✅ Guardar referencia a elementos ocultos
            this.personajeVisual = personajeVisual;   // ✅ Guardar referencia a la imagen del personaje
        }

        public void ActualizarLog(int x, int y)
        {
            if (formLog == null)
            {
                Debug.WriteLine("⚠️ Error: `formLog` es null.");
                return;
            }

            // 🔹 Obtener la imagen desde `elementosOcultos`
            if (!elementosOcultos.TryGetValue((x, y), out PictureBox entidadVisual) || entidadVisual.Image == null)
            {
                formLog.pictureBox1.Image = Image.FromFile("Recursos/hierba.png"); // ✅ Fallback a "hierba.png"
                Debug.WriteLine($"🌿 Imagen por defecto mostrada en [{x}, {y}]");
            }
            else
            {
                formLog.pictureBox1.Image = entidadVisual.Image; // ✅ Usar imagen de la entidad si está disponible
                Debug.WriteLine($"✅ Imagen recuperada desde `elementosOcultos` en [{x}, {y}]");
            }


            // 🔹 Obtener mensaje de la celda
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

        public void ResetearLog()
        {
            formLog.LimpiarLog(); // ✅ Método que vacía el historial en la interfaz
            Debug.WriteLine("📝 Historial de movimientos reseteado.");
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