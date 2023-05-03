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
        TrackBar cd,cr1,cr2, cr,movex,movey;
        Button grabar,play, parar,fill;
        Scene scene = new Scene();
        Scene scene2 = new Scene();
        Scene scene3 = new Scene();
        Model cubo,cube,cubaibi;
        Transform c,c2;
        int w, h;
        float posicionx1,posicionxfinal1,posiciony1,posicionyfinal1;
        float tamañoinicio1, tamañofinal1;
        float rotacionx1, rotacionfinalx1;
        float rotaciony1, rotacionfinaly1;
        float rotacionz1, rotacionfinalz1;
        public static float v_H = 1f, v_W, c_H, c_W;
        public static float distance = 1;
        Boolean fll = false;
        
        
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.Orange;
            AddOBJ();
            AddOBJ2();
            AddOBJ3();
            scene.instance.Add(new Instance(cubo, new Transform(3, new Vertex(0, 0, 0), new Vertex(0, 0, 0))));
            scene2.instance.Add(new Instance(cube, new Transform(3, new Vertex(0, 0, 0), new Vertex(-6, 0, 0))));
            scene3.instance.Add(new Instance(cubaibi, new Transform(3, new Vertex(0, 0, 0), new Vertex(-10, 0, 0))));
           
        }
         string path = "Gun.obj";
        string path2 = "Gun1.obj";
        string path3 = "Gun2.obj";

        private void timer1_Tick(object sender, EventArgs e)
        {
            canvas.FastClear();
            
            scene.instance[0].transform.X = 0;
            scene2.instance[0].transform.X = 0;
            scene3.instance[0].transform.X = 0;
           
   

            
            c.ZPosition = -cd.Value;
            c.Y = -cr2.Value;
            c.X = -cr1.Value;
            c.Z = -cr.Value;

            c.XPosition = movex.Value;
            c.YPosition = movey.Value;



            canvas.RenderScene(scene, fll, c);
            canvas.RenderScene(scene2, fll, c);
            canvas.RenderScene(scene3, fll, c);
            pictureBox.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer2.Interval = 80; // se dispara cada segundo (1000 milisegundos)
            timer2.Enabled = false;
            w = Form1.ActiveForm.Width - 90;
            h = Form1.ActiveForm.Height - 250;
            c_W = w;
            c_H = h;
            v_W = (float)w / (float)h;
            canvas = new Canvas(new Size(w, h));
            AddControls();
            
            this.Controls.Add(cd);
            this.Controls.Add(cr);
            this.Controls.Add(cr1);
            this.Controls.Add(cr2);
            this.Controls.Add(movex);
            this.Controls.Add(movey);           
            this.Controls.Add(grabar);
            this.Controls.Add(fill);
            this.Controls.Add(play);
            this.Controls.Add(parar);
            this.Controls.Add(pictureBox);
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

         private void AddOBJ2()
        {
            string[] lines = System.IO.File.ReadAllLines(path2), tex, f;
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
            cube = new Model(v, t);
            
        }


        private void AddOBJ3()
        {
            string[] lines = System.IO.File.ReadAllLines(path3), tex, f;
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
            cubaibi = new Model(v, t);
            
        }

        private void grabar1_Click(object sender, EventArgs e)
        {
          posicionx1 = c.XPosition;
          posiciony1 = c.YPosition;
          tamañoinicio1 = cd.Value;
          rotacionx1 = cr1.Value;
          rotaciony1 = cr2.Value;
          rotacionz1 = cr.Value;
        }
        private void parar1_Click(object sender, EventArgs e)
        {
          posicionxfinal1 = c.XPosition;
          posicionyfinal1 = c.YPosition;
          tamañofinal1 = cd.Value;
          rotacionfinalx1 = cr1.Value;
          rotacionfinaly1 = cr2.Value;
          rotacionfinalz1 = cr.Value;
     
        }
       
        private void play_Click(object sender, EventArgs e)
        {
          timer2.Enabled = true;
        }

        Timer timer2 = new Timer();
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            canvas.FastClear();
            if (posicionx1 <= posicionxfinal1)
            {
                posicionx1 ++;
                c.XPosition = posicionx1;
               
            }
            if (posiciony1 <= posicionyfinal1)
            {
                posiciony1 ++;
                c.YPosition = posiciony1;
                
            }
             if (tamañoinicio1 >= -13 && tamañoinicio1 <= tamañofinal1)
            {
                tamañoinicio1 ++;
                c.ZPosition = tamañoinicio1;
               
            }
             if (tamañoinicio1 <= -13 && tamañoinicio1 >= tamañofinal1)
            {
                tamañoinicio1 --;
                c.ZPosition = tamañoinicio1;
              
            }
              if (rotacionx1>= 0 && rotacionx1 <= rotacionfinalx1)
            {
                rotacionx1 +=5;
                c.X = rotacionx1;
              
            }
              if (rotacionx1 <= 0 && rotacionx1 >= rotacionfinalx1)
            {
                rotacionx1 -=5;
                c.X = rotacionx1;
              
            }
              if (rotaciony1>= 0 && rotaciony1 <= rotacionfinaly1)
            {
                rotaciony1 +=5;
                c.Y = rotaciony1;
              
            }
              if (rotaciony1 <= 0 && rotaciony1 >= rotacionfinaly1)
            {
                rotaciony1 -=5;
                c.Y = rotaciony1;
              
            }
              if (rotacionz1>= 0 && rotacionz1 <= rotacionfinalz1)
            {
                rotacionz1 +=5;
                c.Z = rotacionz1;
              
            }
              if (rotacionz1 <= 0 && rotacionz1 >= rotacionfinalz1)
            {
                rotacionz1 -=5;
                c.Z = rotacionz1;
              
            }
           
            canvas.RenderScene(scene, fll, c);
            canvas.RenderScene(scene2, fll, c);
            canvas.RenderScene(scene3, fll, c);
            pictureBox.Invalidate();
        }
        private void FILL_Click(object sender, EventArgs e)
        {
            fll = !fll;
            if (fll)
            {
                fill.Text = "WireFrame";
                fll = true;
                pictureBox.Invalidate();

            } else
            {
                fill.Text = "Texture";
            }
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
                Location = new Point(w + 30, 75),
                Orientation = Orientation.Vertical,
                Height = h,
                LargeChange = 1,
                Maximum = 50,
                Minimum = -50,
                Value = -13,
                BackColor = Color.Orange
            };
            
            cr = new TrackBar
            {
                Location = new Point(0, h + 75),
                Width = w,
                Maximum = 180,
                LargeChange = 1,
                Minimum = -180,
                Value = 0,
                BackColor = Color.Orange
            };
             cr1 = new TrackBar
            {
                Location = new Point(0, h + 125),
                Width = w,
                Maximum = 180,
                LargeChange = 1,
                Minimum = -180,
                Value = 0,
                BackColor = Color.Orange
            };
             cr2 = new TrackBar
            {
                Location = new Point(0, h + 175),
                Width = w,
                Maximum = 180,
                LargeChange = 1,
                Minimum = -180,
                Value = 0,
                BackColor = Color.Orange
            };

             movex = new TrackBar
            {
                Location = new Point(0,0),
                Width = w-150,
                Maximum = 300,
                LargeChange = 1,
                Minimum = -300,
                Value = 0,
                BackColor = Color.Orange,
                
            };
            movey = new TrackBar
            {
                Location = new Point(0,45),
                Width = w-150,
                Maximum = 300,
                LargeChange = 1,
                Minimum = -300,
                Value = 0,
                BackColor = Color.Orange
            };
            
            
            grabar = new Button
            {
                Location = new Point(w-140,0),
                BackColor = Color.Gray,
                Text = "GRABAR1",
                ForeColor = Color.Black
            };

             play = new Button
            {
                Location = new Point(w,45),
                BackColor = Color.Gray,
                Text = "PLAY",
                ForeColor = Color.Black
            };
            parar = new Button
            {
                Location = new Point(w-140,22),
                BackColor = Color.Gray,
                Text = "PARAR1",
                ForeColor = Color.Black
            };
            fill = new Button
            {
                Location = new Point(w , 18),
                Text = "Texture",
            };
            fill.Click += FILL_Click;
             
            grabar.Click += new EventHandler(grabar1_Click);
            parar.Click += new EventHandler(parar1_Click);
            play.Click += new EventHandler(play_Click);
            timer2.Tick += new EventHandler(timer2_Tick);

            
                      

        }
        
    }
}
