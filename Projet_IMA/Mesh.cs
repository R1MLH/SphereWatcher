using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class Mesh
    {
        protected List<Triangle> Polygons;

        public Mesh(String ff)
        {
            this.Polygons = new List<Triangle>();
            string s = System.IO.Path.GetFullPath("..\\..");
            string path = System.IO.Path.Combine(s, "mesh", ff);
            StreamReader reader = File.OpenText(path);
            string line;
            List<V3> vertices = new List<V3>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(' ');
                if (items[0] == "#" || items[0] == "") continue;
                if (items[0] == "v") vertices.Add(new V3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3])));
                if (items[0] == "f") Polygons.Add(new Triangle(new Couleur(0.5f, 0.5f, 0.5f), "n", vertices[int.Parse(items[1])], vertices[int.Parse(items[2])], vertices[int.Parse(items[3])]));

            }
        }


    }
}
