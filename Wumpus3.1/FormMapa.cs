using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Wumpus3._1.Visual;

public class FormMapa : Form
{
    private Mapa mapa;
    private PictureBox personajeVisual, pictureBox1, pictureBox2, pictureBox3, pictureBox4;
    private Panel panel1;
    private TableLayoutPanel tablaMapa;

    public FormMapa()
    {
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        this.AutoScaleDimensions = new SizeF(6F, 13F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(550, 550);
        this.ForeColor = Color.AliceBlue;
        this.Text = "Wumpus 3.0";

        // 🔹 Configurar `panel1`
        panel1 = new Panel
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Location = new Point(0, 0),
            Size = new Size(550, 30),
            Name = "panel1"
        };

        pictureBox4 = EntidadVisual.CrearEntidad("Recursos/gold.png", new Size(52, 21), new Point(267, 4));
        panel1.Controls.Add(pictureBox4);
        this.Controls.Add(panel1);

        // 🔹 Configurar `TableLayoutPanel`
        tablaMapa = new TableLayoutPanel
        {
            BackColor = Color.Transparent,
            BackgroundImage = Image.FromFile("Recursos/fondo.jpg"),
            BackgroundImageLayout = ImageLayout.Stretch,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
            ColumnCount = 8,
            RowCount = 8,
            Dock = DockStyle.Bottom,
            Location = new Point(0, 31),
            Size = new Size(550, 519),
            Name = "tablaMapa"

        };

        for (int i = 0; i < 8; i++)
        {
            tablaMapa.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tablaMapa.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        }

        this.Controls.Add(tablaMapa);

        mapa = new Mapa(8, 8);
        mapa.InicializarMapa();
        PosicionarPersonaje();

        // 🔹 Generar `PictureBox` usando `EntidadVisual`
        //pictureBox1 = EntidadVisual.CrearEntidad("Recursos/personaje.png", new Size(60, 56), new Point(5, 5));
        pictureBox2 = EntidadVisual.CrearEntidad("Recursos/enemigo.png", new Size(60, 56), new Point(73, 5));
        pictureBox3 = EntidadVisual.CrearEntidad("Recursos/pokeball.png", new Size(60, 56), new Point(141, 5));

        pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage; // 🔹 Ajusta la imagen sin deformarla


        //tablaMapa.Controls.Add(pictureBox1, 0, 0);
        tablaMapa.Controls.Add(pictureBox2, 1, 0);
        tablaMapa.Controls.Add(pictureBox3, 2, 0);

        this.KeyDown += new KeyEventHandler(OnKeyDown);
        this.ResumeLayout(false);
    }

    // 🔹 Método para ubicar el personaje en la interfaz según el bloc de notas
    private void PosicionarPersonaje()
    {
        Personaje jugador = mapa.ObtenerPersonaje();

        if (jugador != null)
        {
            Debug.WriteLine($"Personaje en FormMapa.cs: X={jugador.X}, Y={jugador.Y}");

            personajeVisual = EntidadVisual.CrearEntidad("Recursos/personaje.png", new Size(60, 56), new Point(0, 0));
            tablaMapa.Controls.Add(personajeVisual, jugador.X, jugador.Y);
        }
        else
        {
            Debug.WriteLine("Error: `ObtenerPersonaje()` devolvió NULL en FormMapa.cs.");
        }
    }


    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        int deltaX = 0, deltaY = 0;

        if (e.KeyCode == Keys.W) deltaY = -1;
        if (e.KeyCode == Keys.S) deltaY = 1;
        if (e.KeyCode == Keys.A) deltaX = -1;
        if (e.KeyCode == Keys.D) deltaX = 1;

        Personaje jugador = mapa.ObtenerPersonaje();
        if (jugador != null)
        {
            tablaMapa.SuspendLayout(); // 🔹 Pausar actualizaciones antes de mover

            jugador.Moverse(deltaX, deltaY, mapa);
            tablaMapa.SetCellPosition(personajeVisual, new TableLayoutPanelCellPosition(jugador.X, jugador.Y));

            tablaMapa.ResumeLayout(); // 🔹 Reactivar actualizaciones después del movimiento
        }
    }


}
