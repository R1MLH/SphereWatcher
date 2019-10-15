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

        public Couleur GetCouleur() { return Couleur*intensite; }
        public abstract V3 GetDirection(V3 point);
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
            V3 sens = point - this.position;
            sens.Normalize();
            return sens;
        }
    }


}
