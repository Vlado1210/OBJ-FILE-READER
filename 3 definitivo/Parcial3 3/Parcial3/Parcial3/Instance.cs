using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Instance
    {
        public Model model;
        public Transform transform;
        public Instance(Model m, Transform t) 
        {
            model = m;
            transform = t;
        }
    }
}
