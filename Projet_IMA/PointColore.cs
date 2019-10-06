using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class PointColore
    {
        protected V3 SpatialLocator;
        protected Couleur Couleur;

        public PointColore(V3 spatialLocator, Couleur couleur)
        {
            SpatialLocator = spatialLocator;
            Couleur = couleur;
        }

        public Couleur GetCouleur() { return this.Couleur; }
        public V3 GetLoc() { return this.SpatialLocator; }
    }
}
