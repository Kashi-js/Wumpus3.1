using Logica;
using System.Diagnostics;

namespace Wumpus3._1
{
    partial class FormLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Bottom;
            richTextBox1.Location = new Point(0, 150);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(296, 373);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Location = new Point(97, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(80, 73);
            pictureBox1.Image = Image.FromFile("Recursos/hierba.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(100, 98);
            label1.Name = "label1";
            label1.Size = new Size(77, 15);
            label1.TabIndex = 2;
            label1.Text = "Celda Actual:";
            label1.Click += label1_Click;
            // 
            // FormLog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(296, 523);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(richTextBox1);
            Name = "FormLog";
            Text = "Log";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        public PictureBox pictureBox1;
        private Label label1;

        public void LimpiarLog()
        {
            richTextBox1.Clear(); // ✅ Vaciar el contenido del log
            Debug.WriteLine("📝 Historial del log limpiado.");
        }

        public void AgregarMensaje(string tipoCelda, string mensaje)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;

            // 🔹 Definir colores según el tipo de celda
            switch (tipoCelda)
            {
                case "S":
                case "W":
                    richTextBox1.SelectionColor = Color.Purple; // ☠️ Peligro: Rojo
                    break;
                case "B":
                case "T":
                    richTextBox1.SelectionColor = Color.Red; // 🌪 Brisa: Azul
                    break;
                case "G":
                case "K":
                    richTextBox1.SelectionColor = Color.DarkGoldenrod; // 💰 Oro: Dorado
                    break;
                default:
                    richTextBox1.SelectionColor = Color.Black; // 🔍 Celda normal
                    break;
            }

            richTextBox1.AppendText(mensaje + Environment.NewLine);
            richTextBox1.SelectionColor = richTextBox1.ForeColor; // 🔹 Restaurar color predeterminado
            richTextBox1.ScrollToCaret();

        }

    }
}