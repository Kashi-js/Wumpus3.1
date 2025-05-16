namespace Logica
{
    public class Personaje : Entidad
    {
        public int Dinero { get; set; }
        public int Vida { get; set; }

        public Personaje(int x, int y, int dinero, int vida) : base(x, y)
        {
            Dinero = dinero;
            Vida = vida;
        }

        public void Ejecutar(Mapa mapa)
        {
            //Entidad entidad = mapa.ObtenerEntidad(X, Y);

        }

        public void Moverse(int deltaX, int deltaY, Mapa mapa)
        {
            int nuevoX = X + deltaX;
            int nuevoY = Y + deltaY;

            // Verificar si la nueva posición es válida dentro del mapa
            if (nuevoX >= 0 && nuevoX < mapa.x && nuevoY >= 0 && nuevoY < mapa.y)
            {
                X = nuevoX;
                Y = nuevoY;
                Ejecutar(mapa);
            }
        }
    }
}
