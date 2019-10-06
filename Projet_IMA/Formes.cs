using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    abstract class Formes
    {
        protected Texture texture;
        protected V3 position;

        protected Formes(String textureLocation, V3 position)
        {
            this.texture = new Texture(textureLocation);
            this.position = position;
        }

        public abstract List<PointColore> GeneratePositions(float pas);
        public V3 GetPosition() { return position; }
    }

    class Sphere : Formes
    {
        protected float rayon;

        public Sphere(String textureLocation, V3 position, float rayon) : base(textureLocation,position)
        {
            this.rayon = rayon;
        }

        public override List<PointColore> GeneratePositions(float pas)
        {
            
            List<PointColore> positions = new List<PointColore>();

            for (float u = 0; u <= 2 * Math.PI; u += pas)
            {
                for (float v = -1 * ((float)Math.PI) / 2.0f; v <= Math.PI / 2; v += pas)
                {
                    float localX = rayon * (float)(Math.Cos(u) * Math.Cos(v)) + this.position.x;
                    float localY = rayon * (float)(Math.Cos(v) * Math.Sin(u)) + this.position.y;
                    float localZ = rayon * (float)Math.Sin(v) + this.position.z;

                    float offsetU = u / (float)(Math.PI * 2);
                    float offsetV = (v+(float)(Math.PI/2)) / (float)(Math.PI);
                    positions.Add(new PointColore(new V3(localX, localY, localZ), texture.LireCouleur(offsetU, offsetV)));

                }
            }

            return positions;
        }

    }
}
