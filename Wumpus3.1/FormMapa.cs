using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using Wumpus3._1;
using Wumpus3._1.Visual;

public class FormMapa : Form
{
    private Mapa mapa;
    private PictureBox personajeVisual, pictureBoxBola, pictureBoxOro;
    private TableLayoutPanel tablaMapa, tablaInventario; 
    private Label labelOro, labelBola;
    private PictureBox[] corazones;
    private FormLog formLog;
    private LogJuego logJuego;
    public FormMapa()
    {
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        this.ClientSize = new Size(550, 550);
        this.Text = "Wumpus 3.0";

        // 🔹 Configurar `tablaInventario` con tres columnas principales
        tablaInventario = new TableLayoutPanel
        {
            ColumnCount = 3,
            RowCount = 1,
            Dock = DockStyle.Top,
            Size = new Size(550, 40),
            CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble
        };

        // 🔹 Distribuir el espacio en proporciones
        tablaInventario.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); // 🔹 Oro (25%)
        tablaInventario.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // 🔹 Vidas (50%)
        tablaInventario.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

        // 🔹 Configurar `layoutOro` con dos columnas (oro y su label)
        TableLayoutPanel layoutOro = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 1,
            AutoSize = true,
            Dock = DockStyle.Fill
        };

        pictureBoxOro = EntidadVisual.CrearEntidad("Recursos/gold.png", new Size(35, 35), new Point(0, 0));
        labelOro = new Label { Text = "0", Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.DarkGoldenrod, AutoSize = true };

        // 🔹 Agregar elementos al `layoutOro`
        layoutOro.Controls.Add(pictureBoxOro, 0, 0);
        layoutOro.Controls.Add(labelOro, 1, 0);

        // 🔹 Configurar `layoutVidas` con tres columnas (cada corazón en su propia celda)
        TableLayoutPanel layoutVidas = new TableLayoutPanel
        {
            ColumnCount = 3,
            RowCount = 1,
            AutoSize = true,
            Dock = DockStyle.Fill
        };
        layoutVidas.ColumnStyles.Clear(); // 🔹 Limpiar estilos previos
        layoutVidas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33)); // 🔹 Espacio proporcional para cada corazón
        layoutVidas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
        layoutVidas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

        // 🔹 Configurar `layoutVidas`
        corazones = new PictureBox[3];
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i] = EntidadVisual.CrearEntidad("Recursos/vida.png", new Size(30, 30), new Point(0, 0));
            corazones[i].Anchor = AnchorStyles.None; // 🔹 Asegurar que cada corazón se mantenga centrado
            corazones[i].Margin = new Padding(5); // 🔹 Eliminar cualquier separación extra
            layoutVidas.Controls.Add(corazones[i], i, 0);

        }

        // 🔹 Configurar `layoutBola` con dos columnas (bola y su label)
        TableLayoutPanel layoutBola = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 1,
            AutoSize = true,
            Dock = DockStyle.Right
        };

        pictureBoxBola = EntidadVisual.CrearEntidad("Recursos/pokeball.png", new Size(30, 30), new Point(0, 0));
        labelBola = new Label { Text = "3", Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.Black, AutoSize = true };

        // 🔹 Agregar elementos al `layoutBola`
        layoutBola.Controls.Add(pictureBoxBola, 0, 0);
        layoutBola.Controls.Add(labelBola, 1, 0);

        // 🔹 Agregar `layoutOro`, `layoutVidas` y `layoutBola` a `tablaInventario`
        tablaInventario.Controls.Add(layoutOro, 0, 0);
        tablaInventario.Controls.Add(layoutVidas, 1, 0);
        tablaInventario.Controls.Add(layoutBola, 2, 0);

        // 🔹 Agregar `tablaInventario` al `Form`
        this.Controls.Add(tablaInventario);

        // 🔹 Configurar `tablaMapa`
        tablaMapa = new TableLayoutPanel
        {
            BackgroundImage = Image.FromFile("Recursos/fondo.jpg"),
            BackgroundImageLayout = ImageLayout.Stretch,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
            ColumnCount = 8,
            RowCount = 8,
            Dock = DockStyle.Bottom, // 🔹 Ocupa el resto del espacio
            Size = new Size(550, 510)
        };

        for (int i = 0; i < 8; i++)
        {
            tablaMapa.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tablaMapa.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        }

        // 🔹 Agregar los paneles al `Form` en el orden correcto
        this.Controls.Add(tablaInventario);
        this.Controls.Add(tablaMapa);
        // 🔹 Inicializar mapa y entidades
        mapa = new Mapa(8, 8);
        formLog = new FormLog();
        logJuego = new LogJuego(mapa, formLog, tablaMapa, elementosOcultos, personajeVisual); // ✅ Pasamos `elementosOcultos`
        formLog.Show();
        mapa.InicializarMapa();
        PosicionarEntidades();

        // 🔹 Obtener referencia del personaje y suscribir eventos
        Personaje jugador = mapa.ObtenerPersonaje();
        if (jugador != null)
        {
            jugador.OroRecogido += ActualizarOro;
            jugador.VidaPerdida += ActualizarVida;
        }


        this.KeyDown += new KeyEventHandler(OnKeyDown);
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

    private void PosicionarEntidades()
{
    tablaMapa.Controls.Clear(); // 🔹 Limpiar controles previos
    elementosOcultos.Clear();  // 🔹 Limpiar referencias ocultas antes de inicializar

    for (int i = 0; i < mapa.x; i++)
    {
        for (int j = 0; j < mapa.y; j++)
        {
            Entidad entidad = mapa.mapa[i, j];

            if (entidad != null)
            {
                string rutaImagen = ObtenerRutaImagen(entidad);
                bool esPersonaje = entidad is Personaje;
                bool visible = esPersonaje; // 🔹 Solo el personaje es visible al inicio

                PictureBox entidadVisual = EntidadVisual.CrearEntidad(rutaImagen, new Size(60, 56), new Point(0, 0), visible);
                tablaMapa.Controls.Add(entidadVisual, i, j);

                if (esPersonaje)
                {
                    personajeVisual = entidadVisual;
                    Debug.WriteLine($"✅ Personaje asignado en X={i}, Y={j}");
                }
                else
                {
                    elementosOcultos[(i, j)] = entidadVisual; // 🔹 Guardar entidad oculta
                }

                Debug.WriteLine($"Entidad {entidad.GetType().Name} insertada en: X={i}, Y={j}, Visible={visible}");
            }
        }
    }
}

    public string ObtenerRutaImagen(Entidad entidad)
    {
        if (entidad is Personaje) return "Recursos/personaje.png";
        if (entidad is Enemigo) return "Recursos/enemigo.png";
        if (entidad is Oro) return "Recursos/gold.png";
        if (entidad is Trampa) return "Recursos/trampa.png";
        if (entidad is Alerta alerta)
        {
            switch (alerta.Tipo)
            {
                case "K": return "Recursos/brillo.png";  // 🔹 Brillo alrededor del oro
                case "B": return "Recursos/warning.png"; // 🔹 Advertencia alrededor de la trampa
                case "S": return "Recursos/olor.png";    // 🔹 Olor alrededor del enemigo
            }
        }


        return "Recursos/default.png"; // 🔹 Imagen por defecto en caso de que falte alguna entidad
    }




    private Dictionary<(int, int), PictureBox> elementosOcultos = new(); // 🔹 Almacena entidades ocultas temporalmente

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        int deltaX = 0, deltaY = 0;

        if (e.KeyCode == Keys.W) deltaY = -1;
        if (e.KeyCode == Keys.S) deltaY = 1;
        if (e.KeyCode == Keys.A) deltaX = -1;
        if (e.KeyCode == Keys.D) deltaX = 1;

        Personaje jugador = mapa.ObtenerPersonaje();
        //if (e.KeyCode == Keys.Up) jugador.Lanzar(mapa, ActualizarBolaVisual, "Arriba");
        //if (e.KeyCode == Keys.Down) jugador.Lanzar(mapa, ActualizarBolaVisual, "Abajo");
        //if (e.KeyCode == Keys.Left) jugador.Lanzar(mapa, ActualizarBolaVisual, "Izquierda");
        //if (e.KeyCode == Keys.Right) jugador.Lanzar(mapa, ActualizarBolaVisual, "Derecha");

        if (jugador != null)
        {
            Debug.WriteLine($"📍 Mapa lógico -> X={jugador.X}, Y={jugador.Y}");


            int prevX = jugador.X;
            int prevY = jugador.Y;

            jugador.Moverse(deltaX, deltaY, mapa);

            // 🔹 Verificar si hay una entidad en la nueva celda
            Control entidadVisual = tablaMapa.GetControlFromPosition(jugador.X, jugador.Y);
            if (entidadVisual != null && entidadVisual != personajeVisual)
            {
                Debug.WriteLine($"🎨 Mapa visual -> X={jugador.X}, Y={jugador.Y}");
                entidadVisual.Visible = false; // 🔹 Ocultar entidad temporalmente
                elementosOcultos[(jugador.X, jugador.Y)] = (PictureBox)entidadVisual;
            }

            // 🔹 Mover al personaje sin eliminar la entidad
            tablaMapa.SetCellPosition(personajeVisual, new TableLayoutPanelCellPosition(jugador.X, jugador.Y));

            // 🔹 Restaurar entidad al dejar la celda
            if (elementosOcultos.TryGetValue((prevX, prevY), out PictureBox entidadGuardada))
            {
                entidadGuardada.Visible = true;
                elementosOcultos.Remove((prevX, prevY)); // 🔹 Eliminarla del diccionario una vez restaurada
            }
            logJuego.ActualizarLog(jugador.X, jugador.Y);

        }
    }

    private void ActualizarOro(int cantidad)
    {
        labelOro.Text = $"{cantidad}";
        Debug.WriteLine($"💰 Oro actualizado: {cantidad}");
    }

    private void ActualizarVida(int vidasRestantes)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].Visible = i < vidasRestantes;
        }
        Debug.WriteLine($"❤️ Vidas restantes: {vidasRestantes}");
    }

   

    public void ActualizarBolaVisual(int x, int y)
    {
        if (!elementosOcultos.ContainsKey((x, y)))
        {
            elementosOcultos[(x, y)] = new PictureBox
            {
                Image = Image.FromFile("Recursos/pokeball.png"),
                Size = new Size(60, 56),
                Visible = true
            };

            tablaMapa.Controls.Add(elementosOcultos[(x, y)], x, y);
        }

        Debug.WriteLine($"⚡ Bola visualizada en [{x}, {y}]");
        logJuego.ActualizarLog(x,y);
    }



}
