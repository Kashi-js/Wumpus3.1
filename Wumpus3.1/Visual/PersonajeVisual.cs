using System.Drawing;
using System.Windows.Forms;

namespace Wumpus3._1.Visual
{
    public class PersonajeVisual
    {
        public static PictureBox CrearPersonaje(int x, int y)
        {
            PictureBox pb = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(x * 40, y * 40),
                Image = Image.FromFile("Recursos/personaje.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            return pb;
        }
    }
}
