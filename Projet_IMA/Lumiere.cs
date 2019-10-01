using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class Lumiere
    {
        Couleur Couleur;
        readonly float intensite;
        V3 direction;

        public Lumiere(Couleur couleur, float intensite, V3 direction)
        {
            Couleur = couleur;
            this.intensite = intensite;
            this.direction = direction;
        }

        public Couleur GetCouleur() { return Couleur*intensite; }
        public V3 GetDirection() { return direction; }
    }
}
