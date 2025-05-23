using System.Diagnostics;

namespace Logica
{
    public class Bloc
    {
        public static string[,] LeerBloc(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"No se encontró el archivo de mapa en: {rutaArchivo}");
            }

            string[] lineas = File.ReadAllLines(rutaArchivo);
            int filas = lineas.Length;
            int columnas = lineas[0].Length;
            string[,] matriz = new string[filas, columnas];

            Debug.WriteLine($"📜 Leyendo archivo: {rutaArchivo}");

            for (int i = 0; i < filas; i++)
            {
                Debug.WriteLine($"Línea {i}: {lineas[i]}"); // 🔍 Verifica lectura correcta
                for (int j = 0; j < columnas; j++)
                {
                    matriz[i, j] = lineas[i][j].ToString();
                }
            }

            return matriz;
        }


    }
}