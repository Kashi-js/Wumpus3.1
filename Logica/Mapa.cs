using System;
using System.Diagnostics;
using System.IO;

namespace Logica
{
    public class Mapa
    {
        public Entidad[,] mapa; // Matriz de entidades
        public int x, y;

        public Mapa(int x, int y)
        {
            this.x = x;
            this.y = y;
            mapa = new Entidad[x, y]; // Inicializar matriz de entidades
        }

        int personajeX = 0;
        int personajeY = 0;
        public void InicializarMapa()
        {
            string rutaMapa = RandomMapa();
            string[,] celdas = Bloc.LeerBloc(rutaMapa);

            Debug.WriteLine("Revisando contenido del mapa..." + rutaMapa);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Debug.WriteLine($"Celda [{i},{j}]: {celdas[i, j]}"); // 🔹 Imprime cada celda

                    if (celdas[i, j] == "P") // 🔹 Buscamos el personaje en el bloc de notas
                    {
                        personajeX = j;
                        personajeY = i;
                        mapa[j, i] = new Personaje(j, i, 0, 3);

                        Debug.WriteLine($"Personaje guardado en mapa[X={j}, Y={i}] = {mapa[j, i] != null}");
                    }
                }
            }
        }


        public Personaje ObtenerPersonaje()
        {
            return mapa[personajeX, personajeY] as Personaje;
        }


        public static string RandomMapa()
        {
            Random random = new Random();
            int numeroAleatorio = random.Next(1, 6); // Genera número entre 1 y 5

            // 🔹 Usar `Directory.GetCurrentDirectory()` en lugar de `AppDomain`
            string rutaMapa = Path.Combine(Directory.GetCurrentDirectory(), "Mapas", $"{numeroAleatorio}.txt");

            return rutaMapa;
        }

    }
}
