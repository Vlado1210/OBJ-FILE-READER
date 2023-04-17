using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Mtx
    {
        private float[,] m;
        private int filas;
        private int columnas;

        public Mtx(int filas, int columnas)
        {
            this.filas = filas;
            this.columnas = columnas;
            this.m = new float[filas, columnas];
        }

        public Mtx(float[,] datos)
        {
            this.filas = datos.GetLength(0);
            this.columnas = datos.GetLength(1);
            this.m = datos;
        }

        public float this[int fila, int columna]
        {
            get { return m[fila, columna]; }
            set { m[fila, columna] = value; }
        }
        public static Mtx operator *(Mtx a, Mtx b)
        {
            Mtx resultado = new Mtx(a.filas, b.columnas);

            for (int i = 0; i < a.filas; i++)
            {
                for (int j = 0; j < b.columnas; j++)
                {
                    float suma = 0.0f;
                    for (int k = 0; k < a.columnas; k++)
                    {
                        suma += a[i, k] * b[k, j];
                    }
                    resultado[i, j] = suma;
                }
            }

            return resultado;
        }

    }
}
