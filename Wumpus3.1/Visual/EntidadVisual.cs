using System.Drawing;
using System.Windows.Forms;

namespace Wumpus3._1.Visual
{
    public static class EntidadVisual
    {
        public static PictureBox CrearEntidad(string rutaImagen, Size tamano, Point posicion, bool visible = true)
        {
            return new PictureBox
            {
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                Image = Image.FromFile(rutaImagen),
                Location = posicion,
                Size = tamano,
                SizeMode = PictureBoxSizeMode.Zoom,
                Visible = visible // 🔹 Agregamos la propiedad `Visible`

            };
        }
    }
}
