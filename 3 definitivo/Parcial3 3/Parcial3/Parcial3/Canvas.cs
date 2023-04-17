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
       
        public static PointF TranslateToO(PointF p)
        {
            p.X = p.X + Width / 2;
            p.Y = p.Y + Height / 2;
            return p;
        }
        public void RenderTriangle(Triangulo triangle, List<PointF> projected, List<Vertex> v, Boolean f)
        {

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
