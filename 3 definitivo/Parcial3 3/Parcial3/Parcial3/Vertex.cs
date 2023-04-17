using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Vertex
    {
        private float w, x, y, z, a, r, g, b;
        private Color c;

        public static readonly Vertex Empty;

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (x == 0f)
                {
                    if (y == 0f)
                        return z == 0f;
                }

                return false;
            }
        }
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public float Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }
        public float W
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
            }
        }
        public Color C
        {
            get
            {
                return c;
            }
            set
            {
                c = value;
                a = c.A;
                r = c.R;
                g = c.G;
                b = c.B;
            }
        }
        public Vertex(float x, float y, float z, Color c)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
            C = c;
        }
        public Vertex(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
        }
        public Vertex()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 1;
        }
        public static Vertex operator +(Vertex v1, Vertex v2)
        {
            Vertex v3 = new Vertex();
            v3.x = v1.x + v2.x;
            v3.y = v1.y + v2.y;
            v3.z = v1.z + v2.z;
            return v3;
        }
        public static Vertex operator *(Vertex v1, int v2)
        {
            Vertex v3 = new Vertex();
            v3.x = v1.x * v2;
            v3.y = v1.y * v2;
            v3.z = v1.z * v2;
            return v3;
        }
        public static Vertex operator *(Vertex v1, float v2)
        {
            Vertex v3 = new Vertex();
            v3.x = v1.x * v2;
            v3.y = v1.y * v2;
            v3.z = v1.z * v2;
            return v3;
        }

    }
}
