using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;

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
                if (items[0] == "v") {
                    float a = float.Parse(items[1], CultureInfo.InvariantCulture);
                    float b = float.Parse(items[2], CultureInfo.InvariantCulture);
                    float c = float.Parse(items[3], CultureInfo.InvariantCulture);
                    vertices.Add(new V3(a,b,c));
                }
                if (items[0] == "f")
                {
                    string[] item1 = items[1].Split('/');
                    string[] item2 = items[2].Split('/');
                    string[] item3 = items[3].Split('/');
                    Polygons.Add(new Triangle(new Couleur(0.5f, 0.5f, 0.5f), "n", vertices[(int.Parse(item1[0]))-1], vertices[(int.Parse(item2[0]))-1], vertices[(int.Parse(item3[0])-1)]));
                }
            }
        }

        public void Rescale(float factor)
        {
            foreach(Triangle t in Polygons)
            {
                t.Rescale(factor);
            }
        }
        public List<Triangle> GetPolygons() { return Polygons; }
    }
}
