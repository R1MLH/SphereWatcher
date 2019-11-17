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

        protected Formes(Texture t, String bp, V3 position)
        {
            this.texture = t;
            this.BumpMap = new Texture(bp);
            this.position = position;
        }

        protected Formes(String textureLocation,String bumpMapLocation, V3 position)
        {
            this.texture = new Texture(textureLocation);
            this.BumpMap = new Texture(bumpMapLocation);
            this.position = position;
        }

        protected Formes(Couleur chroma,String bumpMapLocation,V3 position)
        {
            this.texture = new MonoTexture(chroma);
            this.BumpMap = new Texture(bumpMapLocation);
            this.position = position;
        }

        public abstract List<PointColore> GeneratePositions();
        public abstract float IntersectRayon(V3 camera, V3 directionOculaire);
        public abstract PointColore GetCouleurIntersect(V3 camera,V3 directionOculaire, float intersection);
        public V3 GetPosition() { return position; }
    }

    class Sphere : Formes
    {
        protected float rayon;

        public Sphere(String textureLocation, String bumpMapLocation, V3 position, float rayon) : base(textureLocation,bumpMapLocation, position)
        {
            this.rayon = rayon;
        }

        public Sphere(Couleur chroma, String bumpMapLocation, V3 position, float rayon) : base(chroma,bumpMapLocation,position)
        {
            this.rayon = rayon;
        }

        public override float IntersectRayon(V3 camera, V3 directionOculaire)
        {
            //(R0 + tRd -C)² = r²
            // t²Rd² + t(2RdR0-2CRd) + (C² - 2CR0 -r² + R0²) = 0

            float A = (directionOculaire * directionOculaire); // (Rd²)

            float B = ((2 * (camera * directionOculaire)) - (2*(this.position * directionOculaire))); //(2RdR0 -2CRd)
            float C = ((camera * camera) + (this.position * this.position) +(-2* (this.position * camera)) + (-1*(rayon * rayon))); // (C²+R0²-r² -2CR0)
            float delta = (B * B) - (4 * A * C);

            if(delta > 0)
            {
                float t1 = ((-B) - ((float)Math.Sqrt(delta))) / (2 * A);
                float t2 = ((-B) + ((float)Math.Sqrt(delta))) / (2 * A);
                if(t1 > 0 && t2 > 0)
                {
                    return (float)Math.Min(t1,t2);
                }
                else if (t1>0 && t2 <=0)
                {
                    return t1;
                }
                else if (t2 > 0 && t1 <= 0)
                {
                    return t2;
                }

            }
            return -1;
            

            throw new NotImplementedException();

        }

        

        public override List<PointColore> GeneratePositions()
        {
            float pas = 1 / (this.rayon);
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
                    V3 T3 = (dhdu * normalPoint) ^ dMdv;
                    V3 normaleBump = normalPoint + 0.008f* (T2 + T3);
                    normaleBump.Normalize();

                    positions.Add(new PointColore(point, normaleBump, texture.LireCouleur(offsetU, offsetV),this));

                }
            }

            return positions;
        }
        public override PointColore GetCouleurIntersect(V3 camera, V3 directionOculaire, float intersection)
        {
            V3 point = camera + (intersection * directionOculaire);
            
            IMA.Invert_Coord_Spherique(point-this.position, this.rayon, out float u, out float v);

            float offsetU = u / (float)(Math.PI * 2);
            float offsetV = (v + (float)(Math.PI / 2)) / (float)(Math.PI);

            V3 dMdu = new V3((float)(-Math.Sin(u) * Math.Cos(v) * rayon), (float)(rayon * (float)(Math.Cos(u) * Math.Cos(v))), 0);
            V3 dMdv = new V3((float)(-Math.Sin(v) * Math.Cos(v) * rayon), (float)(rayon * (float)(-Math.Sin(u) * Math.Sin(v))), rayon * (float)Math.Cos(v));
            float dhdu, dhdv;
            BumpMap.Bump(offsetU, offsetV, out dhdu, out dhdv);

            V3 normalPoint = new V3(point - position);
            normalPoint.Normalize();

            V3 T2 = dMdu ^ (dhdv * normalPoint);
            V3 T3 = (dhdu * normalPoint) ^ dMdv;
            V3 normaleBump = normalPoint + 0.008f * (T2 + T3);
            normaleBump.Normalize();
            return new PointColore(point, normaleBump, texture.LireCouleur(offsetU, offsetV),this);
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

        public Quadrilatere(Couleur chroma, string bumpMapLocation, V3 pointA, V3 pointB, V3 pointC) : base(chroma, bumpMapLocation, pointA)
        {
            this.pointB = pointB;
            this.pointC = pointC;
        }

        public override float IntersectRayon(V3 camera, V3 directionOculaire)
        {
            // A + alpha AB + beta AC
            V3 AB = new V3(pointB - position);
            V3 ABNormalise = new V3(AB);
            ABNormalise.Normalize();

            V3 AC = new V3(pointC - position);
            V3 ACNormalise = new V3(AC);
            ACNormalise.Normalize();

            V3 normal = new V3(AB ^ AC);
            normal.Normalize();

            float t = ((position - camera) * normal) / (directionOculaire * normal);
            V3 I = new V3(camera + t * directionOculaire);
            V3 AI = new V3(I - position);

            float alpha = (AI * AB) / (AB * AB);
            float beta = (AI * AC) / (AC * AC);

            if( alpha >=0 && alpha <= 1 && beta >= 0 && beta <= 1)
            {
                return t;
            }
            return -1;

        }

        public override List<PointColore> GeneratePositions()
        {
            float pas = 1;
            V3 AB = new V3(pointB - position);
            V3 ABNormalise = new V3(AB);
            ABNormalise.Normalize();

            V3 AC = new V3(pointC - position);
            V3 ACNormalise = new V3(AC);
            ACNormalise.Normalize();

            V3 normal = new V3(AB ^ AC);
            normal.Normalize();

            List<PointColore> positions = new List<PointColore>();

            for (float u = 0; u < AB.Norm(); u += pas)
            {
                for (float v = 0; v < AC.Norm(); v += pas)
                {
                   

                    float offsetU = u / AB.Norm();
                    float offsetV = v / AC.Norm();


                    V3 dMdu = AB; 
                    V3 dMdv = AC; 

                    float dhdu, dhdv;
                    BumpMap.Bump(offsetU, offsetV, out dhdu, out dhdv);

                    V3 point = new V3(position + (u*ABNormalise) + (v*ACNormalise));
                    

                    V3 T2 = dMdu ^ (dhdv * normal);
                    V3 T3 = (dhdu * normal) ^ dMdv;
                    V3 normaleBump = normal + 0.08f * (T2 + T3);
                    normaleBump.Normalize();

                    positions.Add(new PointColore(point, normaleBump, texture.LireCouleur(offsetU, offsetV),this));
                }
            }
            return positions;
        }

        public override PointColore GetCouleurIntersect(V3 camera, V3 directionOculaire, float intersection)
        {
            V3 point = new V3(camera + intersection * directionOculaire);
            V3 AI = new V3(point - position);

            V3 AB = new V3(pointB - position);
            V3 ABNormalise = new V3(AB);
            ABNormalise.Normalize();

            V3 AC = new V3(pointC - position);
            V3 ACNormalise = new V3(AC);
            ACNormalise.Normalize();

            V3 normal = new V3(AB ^ AC);
            normal.Normalize();

            float alpha = (AI * AB) / (AB * AB);
            float beta = (AI * AC) / (AC * AC);

            V3 dMdu = AB;
            V3 dMdv = AC;

            float dhdu, dhdv;
            BumpMap.Bump(alpha, beta, out dhdu, out dhdv);

            V3 T2 = dMdu ^ (dhdv * normal);
            V3 T3 = (dhdu * normal) ^ dMdv;
            V3 normaleBump = normal + 0.008f * (T2 + T3);
            normaleBump.Normalize();

            return new PointColore(point, normaleBump, texture.LireCouleur(alpha, beta),this);

        }

    }

    class Triangle : Formes
    {
        V3 pointB;
        V3 pointC;
        V3 textureA;
        V3 textureB;
        V3 textureC;
        V3 AB;
        V3 AC;
        V3 ABNormalise;
        V3 ACNormalise;
        V3 normal;
        V3 ABprojector;
        V3 ACprojector;



        public Triangle(Texture tex, V3 pointA, V3 pointB, V3 pointC, V3 texA,V3 texB, V3 texC) : base(tex,"n",pointA) {
            this.pointB = pointB;
            this.pointC = pointC;
            this.textureA = texA;
            this.textureB = texB;
            this.textureC = texC;
            this.resetVectors();

        }

        private void resetVectors()
        {
            this.AB = new V3(pointB - this.position);
            this.AC = new V3(pointC - this.position);
            this.ABNormalise = new V3(AB);
            ABNormalise.Normalize();
            this.ACNormalise = new V3(AC);
            ACNormalise.Normalize();
            this.ABprojector = AC ^ normal;
            this.ACprojector = normal ^ AB;
            this.normal = new V3(AB ^ AC);
            normal.Normalize();
        }

        public void Translate(V3 translator)
        {
            this.position = this.position + translator;
            this.pointB = this.pointB + translator;
            this.pointC = this.pointC + translator;
            this.resetVectors();

        }

        public void Rescale(float factor)
        {
            this.position = this.position * factor;
            this.pointB = this.pointB * factor;
            this.pointC = this.pointC * factor;
            this.resetVectors();
        }

        public override float IntersectRayon(V3 camera, V3 directionOculaire)
        {
            // A + alpha AB + beta AC

            float t = ((position - camera) * normal) / (directionOculaire * normal);
            V3 I = new V3(camera + t * directionOculaire);
            V3 AI = new V3(I - position);

            

            float alpha = (AI * ABprojector) / (AB*ABprojector);
            float beta = (AI * ACprojector) / (AC*ACprojector);

           
            if ( ((alpha + beta) >= 0) && ((alpha +beta) <= 1) && (beta >= 0) && (beta <= 1) && (alpha >= 0) &&( alpha < 1))
            {
                return t;
            }
            return -1;

        }

        public override List<PointColore> GeneratePositions()
        {
            float pas = 1;

            List<PointColore> positions = new List<PointColore>();

            for (float u = 0; u < AB.Norm(); u += pas)
            {
                for (float v = 0; v < AC.Norm(); v += pas)
                {


                    float offsetU = u / AB.Norm();
                    float offsetV = v / AC.Norm();


                    V3 dMdu = AB;
                    V3 dMdv = AC;

                    float dhdu, dhdv;
                    BumpMap.Bump(offsetU, offsetV, out dhdu, out dhdv);

                    V3 point = new V3(position + (u * ABNormalise) + (v * ACNormalise));


                    V3 T2 = dMdu ^ (dhdv * normal);
                    V3 T3 = (dhdu * normal) ^ dMdv;
                    V3 normaleBump = normal + 0.08f * (T2 + T3);
                    normaleBump.Normalize();

                    positions.Add(new PointColore(point, normaleBump, texture.LireCouleur(offsetU, offsetV), this));
                }
            }
            return positions;
        }

        public override PointColore GetCouleurIntersect(V3 camera, V3 directionOculaire, float intersection)
        {
            V3 point = new V3(camera + intersection * directionOculaire);
            V3 AI = new V3(point - position);

            float beta = (AI * AC) / (AC.Norm() * AC.Norm());
            float alpha = (AI * AB) / (AB.Norm() * AB.Norm());


            V3 dMdu = AB;
            V3 dMdv = AC;

            float dhdu, dhdv;
            BumpMap.Bump(alpha, beta, out dhdu, out dhdv);

            V3 T2 = dMdu ^ (dhdv * normal);
            V3 T3 = (dhdu * normal) ^ dMdv;
            V3 normaleBump = normal + 0.08f * (T2 + T3);
            normaleBump.Normalize();

            V3 positionCouleur = textureA + (alpha * (textureB - textureA) + beta * (textureC - textureA));

            return new PointColore(point, normaleBump, texture.LireCouleur(positionCouleur.x, positionCouleur.y), this);

        }
    }
    }
