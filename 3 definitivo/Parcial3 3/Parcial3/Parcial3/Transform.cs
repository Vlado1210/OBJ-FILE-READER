using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Transform
    {
        private float size, a;
        private Vertex angle = new Vertex();
        private Vertex position = new Vertex();
        Mtx p, s, rz, ry, rx, fov,
            d = new Mtx(new float[,]
            {
                {(Form1.distance * Form1.c_W)/Form1.v_W, 0},
                {0, (Form1.distance * Form1.c_H)/Form1.v_H},
            });
        public float FOV
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
                float zNear = 1;
                float zFar = 10000;
                float ap = (float)(value * (System.Math.PI / 180));
                float tanHalfFOV = (float)(Math.Tan(ap/2));
                float zRange = zNear - zFar;

                float f = 1 / tanHalfFOV;
                float q = (zNear + zFar) / zRange;

                fov = new Mtx(new float[,]
                {
                    {f*1,0,0,0},
                    {0,f,0,0},
                    {0,0,-q,2*zNear*q},
                    {0,0,1,0}
                });
            }
        }
        public float Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                s = new Mtx(new float[,]
                {
                    {size,0,0,0 },
                    {0,size,0,0 },
                    {0,0,size,0 },
                    {0,0,0,1},
                });
            }
        }
        public Vertex Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                p = new Mtx(new float[,]
                {
                    {1,0,0,position.X},
                    {0,1,0,position.Y},
                    {0,0,1,position.Z},
                    {0,0,0,1}
                });
            }
        }
        public float XPosition
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
                p = new Mtx(new float[,]
                {
                    {1,0,0,position.X},
                    {0,1,0,position.Y},
                    {0,0,1,position.Z},
                    {0,0,0,1}
                });
            }
        }
        public float YPosition
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
                p = new Mtx(new float[,]
                {
                    {1,0,0,position.X},
                    {0,1,0,position.Y},
                    {0,0,1,position.Z},
                    {0,0,0,1}
                });
            }
        }
        public float ZPosition
        {
            get
            {
                return position.Z;
            }
            set
            {
                position.Z = value;
                p = new Mtx(new float[,]
                {
                    {1,0,0,position.X},
                    {0,1,0,position.Y},
                    {0,0,1,position.Z},
                    {0,0,0,1}
                });
            }
        }
        public Vertex Angle
        {
            get
            {
                return angle;
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
        public float X
        {
            get
            {
                return angle.X;
            }
            set
            {
                angle.X = (float)(value * (System.Math.PI / 180));
                rx = new Mtx(new float[,]
                {
                    {1,0,0,0},
                    {0,(float)Math.Cos(angle.X),-(float)Math.Sin(angle.X),0},
                    {0,(float)Math.Sin(angle.X),(float)Math.Cos(angle.X),0},
                    {0,0,0,1}
                });

            }
        }
        public float Y
        {
            get
            {
                return angle.Y;
            }
            set
            {
                angle.Y = (float)(value * (System.Math.PI / 180));
                ry = new Mtx(new float[,]
                {
                    {(float)Math.Cos(angle.Y),0,(float)Math.Sin(angle.Y),0},
                    {0,1,0,0},
                    {-(float)Math.Sin(angle.Y),0,(float) Math.Cos(angle.Y),0},
                    {0,0,0,1}
                });
            }
        }
        public float Z
        {
            get
            {
                return angle.Z;
            }
            set
            {
                angle.Z = (float)(value * (System.Math.PI / 180));
                rz = new Mtx(new float[,]
                {
                    {(float) Math.Cos(angle.Z),-(float) Math.Sin(angle.Z),0, 0},
                    {(float) Math.Sin(angle.Z),(float) Math.Cos(angle.Z),0, 0},
                    {0,0,1,0},
                    {0,0,0,1}
                });
            }
        }
        public Transform(float s, Vertex a, Vertex p)
        {
            Size = s;
            Angle = a;
            Position = p;
        }
        public Vertex Transformation(Vertex v)
        {
            Mtx vm = new Mtx(new float[,]
            {
                {v.X},
                {v.Y}, 
                {v.Z},
                {v.W}
            });
            vm = p*rz*ry*rx*s*vm;
            Vertex v1 = new Vertex(vm[0,0], vm[1,0], vm[2,0]);
            return v1;
        }
        public PointF TransformationU(Vertex o, Transform c)
        {
            Vertex v = c.Transformation(o);
            Mtx vm1 = new Mtx(new float[,]
            {
                {v.X},
                {v.Y},
                {v.Z},
                {v.W}
            });
            vm1 = fov * p * rz * ry * rx * s * vm1;
            Mtx vm2 = new Mtx(new float[,]
            {
                {vm1[0, 0]/vm1[2, 0]},
                {vm1[1, 0]/vm1[2, 0]}
            });
            vm2 = d * vm2;
            PointF v1 = new PointF(vm2[0, 0], vm2[1, 0]);
            return v1;
        }
    }
}
