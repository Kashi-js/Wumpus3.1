using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

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
            this.mapa = new Entidad[x, y];
        }



        int personajeX = 0;
        int personajeY = 0; 
        private string[,] mapaBloc;
        public void InicializarMapa()
        {
            string rutaMapa = RandomMapa();
            mapaBloc = Bloc.LeerBloc(rutaMapa);

            Debug.WriteLine("📜 Mapa cargado desde el bloc: " + rutaMapa);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Debug.WriteLine($"✅ Celda [{i},{j}]: {mapaBloc[i, j]}"); // ✅ Ahora `i, j` coincide con el mapa lógico
                    Debug.WriteLine($"🧐 Comparación: mapaBloc[{i},{j}] = {mapaBloc[i, j]} / mapa[{i},{j}] = {mapa[i, j]?.GetType().Name}");
                    string celda = mapaBloc[i, j]; // ✅ Accediendo correctamente a la celda

                    // 🔹 Identificar elementos del mapa
                    switch (celda)
                    {
                        case "P":
                            personajeX = i;
                            personajeY = j;
                            mapa[i, j] = new Personaje(i, j, 0, 3,3);
                            break;
                        case "W":
                            mapa[i, j] = new Enemigo(i, j);
                            break;
                        case "G":
                            mapa[i, j] = new Oro(i, j);
                            break;
                        case "T":
                            mapa[i, j] = new Trampa(i, j);
                            break;
                    }
                }
            }
            GenerarAlertas();
        }

        private void GenerarAlertas()
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Entidad entidad = mapa[i, j];

                    if (entidad is Oro)
                        ColocarAlerta(i, j, "K"); // 🔹 Brillo alrededor del oro
                    else if (entidad is Trampa)
                        ColocarAlerta(i, j, "B"); // 🔹 Advertencia alrededor de la trampa
                    else if (entidad is Enemigo)
                        ColocarAlerta(i, j, "S"); // 🔹 Olor alrededor del enemigo
                }
            }
        }

        private void ColocarAlerta(int x, int y, string tipoAlerta)
        {
            int[][] direcciones = { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 } };

            foreach (int[] dir in direcciones) // 🔹 Iteramos sobre las direcciones
            {
                int nuevoX = x + dir[0]; // ✅ Ahora `nuevoX` y `nuevoY` están correctamente definidos
                int nuevoY = y + dir[1];

                if (nuevoX >= 0 && nuevoX < this.x && nuevoY >= 0 && nuevoY < this.y)
                {
                    if (mapa[nuevoX, nuevoY] == null) // 🔹 Solo ubicamos alertas en casillas vacías
                    {
                        mapa[nuevoX, nuevoY] = new Alerta(nuevoX, nuevoY, tipoAlerta);
                        Debug.WriteLine($"Alerta {tipoAlerta} colocada en: X={nuevoX}, Y={nuevoY}");
                    }
                }
            }
        }

        public Personaje ObtenerPersonaje()
        {
            return mapa[personajeX, personajeY] as Personaje;
        }

        public string ObtenerCelda(int x, int y)
        {
            if (x >= 0 && x < this.x && y >= 0 && y < this.y)
            {
                string letra = mapaBloc[x, y];
                Debug.WriteLine($"🧐 ObtenerCelda({x}, {y}) -> {letra}"); // 🔍 Verificar letra de la celda
                return letra;
            }
            else
            {
                return "X"; // 🔹 Código para indicar celda fuera de los límites
            }
        }

        public Entidad ObtenerEntidad(int x, int y)
        {
            if (x >= 0 && x < this.x && y >= 0 && y < this.y) // 🔹 Verificar límites
            {
                return mapa[x, y]; // ✅ Retornar entidad en la celda especificada
            }

            Debug.WriteLine($"⚠️ Coordenadas fuera de rango: [{x}, {y}]");
            return null; // 🔹 Retorna `null` si está fuera del mapa
        }

        public static string RandomMapa()
        {
            Random random = new Random();
            int numeroAleatorio = random.Next(1, 6); // Genera número entre 1 y 5

            // 🔹 Usar `Directory.GetCurrentDirectory()` en lugar de `AppDomain`
            string rutaMapa = Path.Combine(Directory.GetCurrentDirectory(), "Mapas", $"{numeroAleatorio}.txt");

            return rutaMapa;
        }

        public bool EsDentroDeLimite(int x, int y)
        {
            return x >= 0 && x < this.x && y >= 0 && y < this.y;
        }

        public void SimularBola(int x, int y, Action<int, int> actualizarVisual)
        {
            if (!EsDentroDeLimite(x, y)) return;

            Debug.WriteLine($"⚡ Trayectoria de bola en [{x}, {y}]");

            // 🔹 Notificar a la capa visual que debe actualizar la imagen de la bola
            actualizarVisual?.Invoke(x, y);
        }
    }
}
