using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    abstract class Formes
    {
        protected Texture texture;
        protected Texture BumpMap;
        protected V3 position;

        protected Formes(String textureLocation,String bumpMapLocation, V3 position)
        {
            this.texture = new Texture(textureLocation);
            this.BumpMap = new Texture(bumpMapLocation);
            this.position = position;
        }

        public abstract List<PointColore> GeneratePositions(float pas);
        public V3 GetPosition() { return position; }
    }

    class Sphere : Formes
    {
        protected float rayon;

        public Sphere(String textureLocation, String bumpMapLocation, V3 position, float rayon) : base(textureLocation,bumpMapLocation, position)
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

                    V3 dMdu = new V3((float)(-Math.Sin(u) * Math.Cos(v) * rayon),(float)(rayon * (float)(Math.Cos(u) * Math.Cos(v))), 0);
                    V3 dMdv = new V3((float)(-Math.Sin(v) * Math.Cos(v) * rayon), (float)(rayon * (float)(-Math.Sin(u) * Math.Sin(v))), rayon*(float)Math.Cos(v));
                    float dhdu,dhdv;
                    BumpMap.Bump(u, v,out dhdu,out dhdv);

                    V3 point = new V3(localX, localY, localZ);
                    V3 normalPoint = new V3(point - position);
                    normalPoint.Normalize();

                    V3 T2 = dMdu ^ (dhdv * normalPoint);
                    V3 T3 = dMdv ^ (dhdu * normalPoint);
                    V3 normaleBump = normalPoint + 0.008f* (T2 + T3);
                    normaleBump.Normalize();
                    float offsetU = u / (float)(Math.PI * 2);
                    float offsetV = (v+(float)(Math.PI/2)) / (float)(Math.PI);

                    positions.Add(new PointColore(point,normaleBump, texture.LireCouleur(offsetU, offsetV)));

                }
            }

            return positions;
        }

    }
}
