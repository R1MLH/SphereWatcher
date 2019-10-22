using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    abstract class Lumiere
    {
        protected Couleur Couleur;
        protected float intensite;

        public Lumiere(Couleur couleur, float intensite)
        {
            Couleur = couleur;
            this.intensite = intensite;
        }

        public virtual Couleur GetCouleur() { return Couleur; }
        public virtual float GetIntensite(V3 point) { return this.intensite; }
        public abstract V3 GetDirection(V3 point);
        public abstract bool isOccluded(PointColore point,List<Formes> objets);
    }

    class LampeDirectionelle : Lumiere
    {
        protected V3 direction;

        public LampeDirectionelle(Couleur couleur, float intensite,V3 direction) : base(couleur,intensite)
        {
            this.direction = direction;
        }

        public override V3 GetDirection(V3 point)
        {
            return this.direction;
        }

        public override bool isOccluded(PointColore point, List<Formes> objets)
        {
            foreach (Formes objet in objets)
            {
                if (objet != point.getOwner())
                {
                    float intersect = objet.IntersectRayon(point.GetLoc(), this.direction);
                    if (intersect >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class LampePonctuelle : Lumiere
    {
        protected V3 position;
        protected float decay;

        public LampePonctuelle(Couleur couleur, float intensite,V3 position, float decay) : base(couleur, intensite)
        {
            this.position = position;
            this.decay = decay;
        }

        public override V3 GetDirection(V3 point)
        {
            V3 sens =  this.position - point;
            sens.Normalize();
            return sens;
        }

        public override bool isOccluded(PointColore point, List<Formes> objets)
        {
            foreach (Formes objet in objets)
            {
                if (objet != point.getOwner())
                {
                    float intermax = (this.position - point.GetLoc()).Norm();
                    float intersect = objet.IntersectRayon(point.GetLoc(), this.GetDirection(point.GetLoc()));
                    if (intersect >= 0 && intersect <= intermax)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override float GetIntensite(V3 point)
        {
            float distance = (this.position - point).Norm();
            return Math.Max(0,(this.intensite - (decay*distance)));
        }
    }


}
