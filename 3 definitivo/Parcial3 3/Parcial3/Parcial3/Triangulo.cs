using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Triangulo
    {
        public Color color;
        public int p0, p1, p2;
        public int[] v = new int[3];
        public Triangulo(int p0, int p1, int p2, Color c)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            color = c;
        }
        public void SortByY() { }
        private void Swap(ref Vertex a, ref Vertex b)
        { }
    }
}
