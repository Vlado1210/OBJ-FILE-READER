using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcial3
{
    public partial class Form1 : Form
    {
         public Canvas canvas;
        PictureBox pictureBox;
        TrackBar cd,cr1,cr2, cr;
        Scene scene = new Scene();
        Model cubo;
        Transform c;
        int w, h;
        public static float v_H = 1f, v_W, c_H, c_W;
        public static float distance = 1;
        Boolean fll = false;
        
        
        public Form1()
        {
            InitializeComponent();
            AddOBJ();
            scene.instance.Add(new Instance(cubo, new Transform(1, new Vertex(0, 0, 0), new Vertex(0, 0, 0))));
           
        }
         string path = "C:\\Users\\vlady\\Downloads\\sphere.obj";

        private void timer1_Tick(object sender, EventArgs e)
        {
            canvas.FastClear();
            
            scene.instance[0].transform.X = 0;
            
            c.ZPosition = -cd.Value;
            c.Y = -cr2.Value;
            c.X = -cr1.Value;
            c.Z = -cr.Value;

            canvas.RenderScene(scene, fll, c);
            pictureBox.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            w = Form1.ActiveForm.Width - 90;
            h = Form1.ActiveForm.Height - 250;
            c_W = w;
            c_H = h;
            v_W = (float)w / (float)h;
            canvas = new Canvas(new Size(w, h));
            AddControls();
            this.Controls.Add(pictureBox);
            this.Controls.Add(cd);
            this.Controls.Add(cr);
            this.Controls.Add(cr1);
            this.Controls.Add(cr2);
            c = new Transform(1, new Vertex(0, 0, 0), new Vertex(0, 0, 0));
            c.FOV = 110;
        }
      

        private void AddOBJ()
        {
            string[] lines = System.IO.File.ReadAllLines(path), tex, f;
            int[] tri = new int[3];
            List<Vertex> v = new List<Vertex>();
            List<Triangulo> t = new List<Triangulo>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "")
                {
                    if (lines[i][0] == 'v')
                    {
                        if (lines[i][1] == ' ')
                        {
                            tex = lines[i].Split(' ');
                            v.Add(new Vertex(float.Parse(tex[1]), float.Parse(tex[2]), float.Parse(tex[3])));
                        }
                    }
                    if (lines[i][0] == 'f')
                    {
                        tex = lines[i].Split(' ');
                        for (int j = 1; j < 4; j++)
                        {
                            f = tex[j].Split('/');
                            tri[j - 1] = int.Parse(f[0]) - 1;
                        }
                        t.Add(new Triangulo(tri[0], tri[1], tri[2], Color.Purple));
                    }
                }
            }
            cubo = new Model(v, t);
            
        }

       
        private void AddControls()
        {
            pictureBox = new PictureBox
            {
                Image = canvas.bitmap,
                Size = new Size(w, h),
                Location = new Point(0, 50),
                BackColor = Color.Orange
            };
            
            cd = new TrackBar
            {
                Location = new Point(w + 5, 50),
                Orientation = Orientation.Vertical,
                Height = h,
                LargeChange = 1,
                Maximum = 50,
                Minimum = -50,
                Value = -13,
            };
            
            cr = new TrackBar
            {
                Location = new Point(0, h + 50),
                Width = w,
                Maximum = 180,
                LargeChange = 1,
                Minimum = -180,
                Value = 0,
            };
             cr1 = new TrackBar
            {
                Location = new Point(0, h + 100),
                Width = w,
                Maximum = 180,
                LargeChange = 1,
                Minimum = -180,
                Value = 0,
            };
             cr2 = new TrackBar
            {
                Location = new Point(0, h + 150),
                Width = w,
                Maximum = 180,
                LargeChange = 1,
                Minimum = -180,
                Value = 0,
            };
            

        }
        
    }
}
