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

                    float offsetU = u / (float)(Math.PI * 2);
                    float offsetV = (v + (float)(Math.PI / 2)) / (float)(Math.PI);

                    V3 dMdu = new V3((float)(-Math.Sin(u) * Math.Cos(v) * rayon),(float)(rayon * (float)(Math.Cos(u) * Math.Cos(v))), 0);
                    V3 dMdv = new V3((float)(-Math.Sin(v) * Math.Cos(v) * rayon), (float)(rayon * (float)(-Math.Sin(u) * Math.Sin(v))), rayon*(float)Math.Cos(v));
                    float dhdu,dhdv;
                    BumpMap.Bump(offsetU, offsetV,out dhdu,out dhdv);

                    V3 point = new V3(localX, localY, localZ);
                    V3 normalPoint = new V3(point - position);
                    normalPoint.Normalize();

                    V3 T2 = dMdu ^ (dhdv * normalPoint);
                    V3 T3 = dMdv ^ (dhdu * normalPoint);
                    V3 normaleBump = normalPoint + 0.008f* (T2 + T3);
                    normaleBump.Normalize();

                    positions.Add(new PointColore(point,normaleBump, texture.LireCouleur(offsetU, offsetV)));

                }
            }

            return positions;
        }

    }

    class Quadrilatere : Formes
    {
        V3 pointB;
        V3 pointC;

        public Quadrilatere(string textureLocation, string bumpMapLocation, V3 pointA, V3 pointB, V3 pointC) : base(textureLocation, bumpMapLocation, pointA)
        {
            this.pointB = pointB;
            this.pointC = pointC;
        }

        public override List<PointColore> GeneratePositions(float pas)
        {
            V3 AB = new V3(pointB - position);
            V3 ABNormalise = new V3(AB);
            ABNormalise.Normalize();

            V3 AC = new V3(pointC - position);
            V3 ACNormalise = new V3(AC);
            ACNormalise.Normalize();

            V3 normal = new V3(AB ^ AC);
            normal.Normalize();

            List<PointColore> positions = new List<PointColore>();

            for (float u = 0; u < AB.Norm(); u += 50*pas)
            {
                for (float v = 0; v < AC.Norm(); v += 50*pas)
                {
                   

                    float offsetU = u / AB.Norm();
                    float offsetV = v / AC.Norm();


                    V3 dMdu = ABNormalise; //TODO
                    V3 dMdv = ACNormalise; //TODO

                    float dhdu, dhdv;
                    BumpMap.Bump(offsetU, offsetV, out dhdu, out dhdv);

                    V3 point = new V3(position + (u*ABNormalise) + (v*ACNormalise));
                    

                    V3 T2 = dMdu ^ (dhdv * normal);
                    V3 T3 = dMdv ^ (dhdu * normal);
                    V3 normaleBump = normal + 0.008f * (T2 + T3);
                    normaleBump.Normalize();

                    positions.Add(new PointColore(point, normaleBump, texture.LireCouleur(offsetU, offsetV)));
                }
            }
            return positions;
        }
    }
}
