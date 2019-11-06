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
        protected Texture texture;
        

        public Mesh(String meshLoc, String TexLoc)
        {
            this.Polygons = new List<Triangle>();
            List<V3> vTex = new List<V3>();
            List<V3> vertices = new List<V3>();


            string s = System.IO.Path.GetFullPath("..\\..");
            string path = System.IO.Path.Combine(s, "mesh", meshLoc);
            StreamReader reader = File.OpenText(path);
            string line;

            this.texture = new Texture(TexLoc);
            
            while ((line = reader.ReadLine()) != null)
            {
                char[] separator = { ' ' };
                string[] items = line.Split(separator , StringSplitOptions.RemoveEmptyEntries);
                if (items == null || items.Length == 0) continue;
                if (items[0] == "#" || items[0] == "") continue;
                if (items[0] == "v") {
                    float a = float.Parse(items[1], CultureInfo.InvariantCulture);
                    float b = float.Parse(items[2], CultureInfo.InvariantCulture);
                    float c = float.Parse(items[3], CultureInfo.InvariantCulture);
                    vertices.Add(new V3(a,-c,b));
                }
                if (items[0] == "vt")
                {
                    float a = float.Parse(items[1], CultureInfo.InvariantCulture);
                    if (a > 1) a -= 1;
                    float b = float.Parse(items[2], CultureInfo.InvariantCulture);
                    if (b < 0) b += 1;
                    vTex.Add(new V3(a, b, 0.0f));
                }
                if (items[0] == "f")
                {
                    string[] item1 = items[1].Split('/');
                    string[] item2 = items[2].Split('/');
                    string[] item3 = items[3].Split('/');
                    if (item1[1] != "" && item2[1] != "" && item2[3] != "")
                    {
                        Polygons.Add(new Triangle(this.texture,
                                              vertices[(int.Parse(item1[0])) - 1], vertices[(int.Parse(item2[0])) - 1], vertices[(int.Parse(item3[0]) - 1)],
                                              vTex[(int.Parse(item1[1])) - 1], vTex[(int.Parse(item2[1])) - 1], vTex[(int.Parse(item3[1]) - 1)]));

                    }
                    else
                    {
                        Polygons.Add(new Triangle(this.texture,
                                            vertices[(int.Parse(item1[0])) - 1], vertices[(int.Parse(item2[0])) - 1], vertices[(int.Parse(item3[0]) - 1)],
                                            new V3(0,0,0), new V3(0, 1, 0), new V3(1, 0, 0)));

                    }
                }
            }
        }

        public void Translate(V3 translator) { foreach (Triangle t in this.Polygons) t.Translate(translator); }

        public void Rescale(float factor){ foreach(Triangle t in Polygons) t.Rescale(factor); }

        public List<Triangle> GetPolygons() { return Polygons; }
    }
}
