using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Model
    {
        public List<Vertex> vertex; 
        public List<Triangulo> triangles; 
        public Vertex position;
        public Model(List<Vertex> ver, List<Triangulo> tris)
        {
            this.vertex = ver;
            this.triangles = tris;
        }
    }
}
