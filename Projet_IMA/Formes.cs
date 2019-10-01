using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    abstract class Formes
    {
        protected Couleur couleur;
        protected V3 position;

        protected Formes(Couleur couleur, V3 position)
        {
            this.couleur = couleur;
            this.position = position;
        }

        public abstract List<V3> GeneratePositions(float pas);
    }

    class Sphere : Formes
    {
        float rayon;

        public Sphere(Couleur couleur, V3 position, float rayon) : base(couleur,position)
        {
            this.rayon = rayon;
        }

        public override List<V3> GeneratePositions(float pas)
        {
            
            List<V3> positions = new List<V3>();

            for (float u = 0; u <= 2 * Math.PI; u += pas)
            {
                for (float v = -1 * ((float)Math.PI) / 2.0f; v <= Math.PI / 2; v += pas)
                {
                    float localX = rayon * (float)(Math.Cos(u) * Math.Cos(v)) + this.position.x;
                    float localY = rayon * (float)(Math.Cos(v) * Math.Sin(u)) + this.position.y;
                    float localZ = rayon * (float)Math.Sin(v) + this.position.z;

                    positions.Add(new V3(localX, localY, localZ));

                }
            }

            return positions;
        }

    }
}
