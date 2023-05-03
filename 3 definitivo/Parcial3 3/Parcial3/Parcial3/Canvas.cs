using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcial3
{
    public class Canvas
    {
        public Bitmap bitmap;
        public static float Width, Height;
        public byte[] bits;
        Graphics g;
        int pixelFormatSize, stride;
        public static float d = Form1.distance;
        public Canvas(Size size)
        {
            Init(size.Width, size.Height);
        }
        public void Init(int width, int height)
        {
            PixelFormat format;
            GCHandle handle;
            IntPtr bitPtr;
            int padding;

            format = PixelFormat.Format32bppArgb;
            Width = width;
            Height = height;
            pixelFormatSize = Image.GetPixelFormatSize(format) / 8;
            stride = width * pixelFormatSize;
            padding = (stride % 4);
            stride += padding == 0 ? 0 : 4 - padding;
            bits = new byte[stride * height];
            handle = GCHandle.Alloc(bits, GCHandleType.Pinned);
            bitPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bits, 0);
            bitmap = new Bitmap(width, height, stride, format, bitPtr);

            g = Graphics.FromImage(bitmap);
        }
        public List<float> Interpolate(int i0, int d0, int i1, int d1)
        {
            List<float> values = new List<float>();
            if (i0 == i1)
            {
                values.Add(d0);
                return values;
            }
            float a = ((float)d1 - (float)d0) / ((float)i1 - (float)i0);
            float d = d0;
            for (int i = i0; i <= i1; i++)
            {
                values.Add(d);
                d = d + a;
            }
            return values;
        }
        public List<float> Interpolate(float i0, float d0, float i1, float d1)
        {
            List<float> values = new List<float>();
            if (i0 == i1)
            {
                values.Add(d0);
                return values;
            }
            float a = ((float)d1 - (float)d0) / ((float)i1 - (float)i0);
            float d = d0;
            for (int i = (int)i0; i <= i1; i++)
            {
                values.Add(d);
                d = d + a;
            }
            return values;
        }
        public void DrawPixel(int x, int y, Color c)
        {
            int res = (int)((x * pixelFormatSize) + (y * stride));

            if (x < 0 || x >= Width || y < 0 || y >= Height) return;

            bits[res + 0] = c.B;
            bits[res + 1] = c.G;
            bits[res + 2] = c.R;
            bits[res + 3] = c.A;
        }
        public void FastClear()
        {
            unsafe
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* bits = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        bits[x + 0] = 0;
                        bits[x + 1] = 0;
                        bits[x + 2] = 0;
                        bits[x + 3] = 0;
                    }
                });
                bitmap.UnlockBits(bitmapData);
            }
        }
        public void DrawLine(Point p0, Point p1, Color c)
        {
            if (Math.Abs(p1.X - p0.X) > Math.Abs(p1.Y - p0.Y))
            {
                if (p0.X > p1.X)
                {
                    Point p = p0;
                    p0 = p1;
                    p1 = p;
                }
                List<float> ys = Interpolate(p0.X, p0.Y, p1.X, p1.Y);
                for (int i = p0.X; i <= p1.X; i++)
                {
                    DrawPixel(i, (int)ys[i - p0.X], c);
                }
            }
            else
            {
                if (p0.Y > p1.Y)
                {
                    Point p = p0;
                    p0 = p1;
                    p1 = p;
                }
                List<float> xs = Interpolate(p0.Y, p0.X, p1.Y, p1.X);
                for (int i = p0.Y; i <= p1.Y; i++)
                {
                    DrawPixel((int)xs[i - p0.Y], i, c);
                }
            }
        }
        public void DrawWireFrameTriangle(Point p0, Point p1, Point p2, Color c)
        {
            DrawLine(p0, p1, c);
            DrawLine(p1, p2, c);
            DrawLine(p2, p0, c);
        }
        public void DrawTriangle(Point p0, Point p1, Point p2, Color c)
        {
            List<float> x_left;
            List<float> x_right;

            if (p1.Y < p0.Y)
            {
                Point p = p0;
                p0 = p1;
                p1 = p;
            }
            if (p2.Y < p0.Y)
            {
                Point p = p0;
                p0 = p2;
                p2 = p;
            }
            if (p2.Y < p1.Y)
            {
                Point p = p2;
                p2 = p1;
                p1 = p;
            }

            List<float> x01 = Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            List<float> x12 = Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            List<float> x02 = Interpolate(p0.Y, p0.X, p2.Y, p2.X);

            x01.RemoveAt(x01.Count - 1);
            List<float> x012 = new List<float>();
            x012.AddRange(x01);
            x012.AddRange(x12);

            int m = x02.Count / 2;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;
            }
            else
            {
                x_left = x012;
                x_right = x02;
            }

            for (int y = p0.Y; y < p2.Y; y++)
            {
                for (float x = x_left[y - p0.Y]; x < x_right[y - p0.Y]; x++)
                {
                    DrawPixel((int)x, y, c);
                }
            }
        }
        public void DrawShadedTriangleOH(Point p0, float h0, Point p1, float h1, Point p2, float h2, Color c)
        {
            List<float> x_left;
            List<float> x_right;
            List<float> h_left;
            List<float> h_right;

            if (p1.Y < p0.Y)
            {
                Point p = p0;
                p0 = p1;
                p1 = p;
                float t = h0;
                h0 = h1;
                h1 = t;
            }
            if (p2.Y < p0.Y)
            {
                Point p = p0;
                p0 = p2;
                p2 = p;
                float t = h0;
                h0 = h2;
                h2 = t;
            }
            if (p2.Y < p1.Y)
            {
                Point p = p2;
                p2 = p1;
                p1 = p;
                float t = h2;
                h2 = h1;
                h1 = t;
            }

            List<float> x01 = Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            List<float> x12 = Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            List<float> x02 = Interpolate(p0.Y, p0.X, p2.Y, p2.X);

            List<float> h01 = Interpolate(p0.Y, h0, p1.Y, h1);
            List<float> h12 = Interpolate(p1.Y, h1, p2.Y, h2);
            List<float> h02 = Interpolate(p0.Y, h0, p2.Y, h2);

            x01.RemoveAt(x01.Count - 1);
            List<float> x012 = new List<float>();
            x012.AddRange(x01);
            x012.AddRange(x12);

            h01.RemoveAt(h01.Count - 1);
            List<float> h012 = new List<float>();
            h012.AddRange(h01);
            h012.AddRange(h12);

            int m = x02.Count / 2;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                h_left = h02;
                h_right = h012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                h_left = h012;
                h_right = h02;
            }

            for (int y = p0.Y; y < p2.Y; y++)
            {
                float x_l = x_left[y - p0.Y];
                float x_r = x_right[y - p0.Y];
                List<float> h_segment = Interpolate(x_l, h_left[y - p0.Y], x_r, h_right[y - p0.Y]);
                for (float x = x_left[y - p0.Y]; x < x_right[y - p0.Y]; x++)
                {
                    Color shaded_color = Color.FromArgb((int)(c.R * h_segment[(int)(x - x_l)]), (int)(c.G * h_segment[(int)(x - x_l)]), (int)(c.B * h_segment[(int)(x - x_l)]));
                    DrawPixel((int)x, y, shaded_color);
                }
            }

        }
        public void DrawShadedTriangle(Point p0, Color c0, Point p1, Color c1, Point p2, Color c2)
        {
            List<float> x_left;
            List<float> x_right;
            List<float> r_left;
            List<float> r_right;
            List<float> g_left;
            List<float> g_right;
            List<float> b_left;
            List<float> b_right;

            if (p1.Y < p0.Y)
            {
                Point p = p0;
                p0 = p1;
                p1 = p;
                Color c = c0;
                c0 = c1;
                c1 = c;
            }
            if (p2.Y < p0.Y)
            {
                Point p = p0;
                p0 = p2;
                p2 = p;
                Color c = c0;
                c0 = c2;
                c2 = c;
            }
            if (p2.Y < p1.Y)
            {
                Point p = p2;
                p2 = p1;
                p1 = p;
                Color c = c2;
                c2 = c1;
                c1 = c;
            }

            List<float> x01 = Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            List<float> x12 = Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            List<float> x02 = Interpolate(p0.Y, p0.X, p2.Y, p2.X);

            List<float> r01 = Interpolate(p0.Y, c0.R, p1.Y, c1.R);
            List<float> r12 = Interpolate(p1.Y, c1.R, p2.Y, c2.R);
            List<float> r02 = Interpolate(p0.Y, c0.R, p2.Y, c2.R);

            List<float> g01 = Interpolate(p0.Y, c0.G, p1.Y, c1.G);
            List<float> g12 = Interpolate(p1.Y, c1.G, p2.Y, c2.G);
            List<float> g02 = Interpolate(p0.Y, c0.G, p2.Y, c2.G);

            List<float> b01 = Interpolate(p0.Y, c0.B, p1.Y, c1.B);
            List<float> b12 = Interpolate(p1.Y, c1.B, p2.Y, c2.B);
            List<float> b02 = Interpolate(p0.Y, c0.B, p2.Y, c2.B);

            x01.RemoveAt(x01.Count - 1);
            List<float> x012 = new List<float>();
            x012.AddRange(x01);
            x012.AddRange(x12);

            r01.RemoveAt(r01.Count - 1);
            List<float> r012 = new List<float>();
            r012.AddRange(r01);
            r012.AddRange(r12);

            g01.RemoveAt(g01.Count - 1);
            List<float> g012 = new List<float>();
            g012.AddRange(g01);
            g012.AddRange(g12);

            b01.RemoveAt(b01.Count - 1);
            List<float> b012 = new List<float>();
            b012.AddRange(b01);
            b012.AddRange(b12);

            int m = x02.Count / 2;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                r_left = r02;
                r_right = r012;

                g_left = g02;
                g_right = g012;

                b_left = b02;
                b_right = b012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                r_left = r012;
                r_right = r02;

                g_left = g012;
                g_right = g02;

                b_left = b012;
                b_right = b02;
            }

            for (int y = p0.Y; y < p2.Y; y++)
            {
                float x_l = x_left[y - p0.Y];
                float x_r = x_right[y - p0.Y];
                List<float> r_segment = Interpolate(x_l, r_left[y - p0.Y], x_r, r_right[y - p0.Y]);
                List<float> g_segment = Interpolate(x_l, g_left[y - p0.Y], x_r, g_right[y - p0.Y]);
                List<float> b_segment = Interpolate(x_l, b_left[y - p0.Y], x_r, b_right[y - p0.Y]);
                for (float x = x_left[y - p0.Y]; x < x_right[y - p0.Y]; x++)
                {
                    Color shaded_color = Color.FromArgb((int)(r_segment[(int)(x - x_l)]), (int)(g_segment[(int)(x - x_l)]), (int)(b_segment[(int)(x - x_l)]));
                    DrawPixel((int)x, y, shaded_color);
                }
            }

        }
        public static PointF TranslateToO(PointF p)
        {
            p.X = p.X + Width / 2;
            p.Y = p.Y + Height / 2;
            return p;
        }
        public void RenderTriangle(Triangulo triangle, List<PointF> projected, List<Vertex> v, Boolean f)
        {

            if (f)
            {
                DrawShadedTriangle(Point.Round(projected[triangle.p0]), v[triangle.p0].C,
                Point.Round(projected[triangle.p1]), v[triangle.p1].C,
                Point.Round(projected[triangle.p2]), v[triangle.p2].C);
            }
            DrawWireFrameTriangle(Point.Round(projected[triangle.p0]),
                Point.Round(projected[triangle.p1]),
                Point.Round(projected[triangle.p2]),
                triangle.color);
        }
        public void RenderScene(Scene scene, Boolean f, Transform t)
        {
            for (int i = 0; i < scene.instance.Count; i++)
            {
                RenderInstance(scene.instance[i], f, t);
            }
        }
        public void RenderInstance(Instance instance, Boolean f, Transform t) {
            List<PointF> projected = new List<PointF>();
            Model model = instance.model;
            for (int V = 0; V < model.vertex.Count; V++)
            {
                projected.Add(TranslateToO(t.TransformationU(model.vertex[V], instance.transform)));
            }
            for (int T = 0; T < model.triangles.Count; T++)
            {
                RenderTriangle(model.triangles[T], projected, model.vertex, f);
            }
        }
    }
}
