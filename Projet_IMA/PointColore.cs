using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class PointColore
    {
        protected V3 SpatialLocator;
        protected V3 normale;
        protected Couleur Couleur;
        protected Formes proprietaire;

        public PointColore(V3 spatialLocator,V3 normale, Couleur couleur, Formes proprio)
        {
            SpatialLocator = spatialLocator;
            Couleur = couleur;
            this.normale = normale;
            this.proprietaire = proprio;
        }

        public Couleur GetCouleur() { return this.Couleur; }
        public V3 GetLoc() { return this.SpatialLocator; }
        public V3 GetNormale() { return this.normale; }
        public Formes getOwner() { return this.proprietaire; }
    }
}
