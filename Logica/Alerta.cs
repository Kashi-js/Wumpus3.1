namespace Logica
{
    public class Alerta : Entidad
    {
        public string Tipo { get; } // 🔹 Agregamos la propiedad `Tipo`

        public Alerta(int x, int y, string tipo) : base(x, y) 
        {
            Tipo = tipo; // 🔹 Guardamos el tipo de alerta
        }
    }

}