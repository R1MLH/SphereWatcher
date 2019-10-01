using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class Scene
    {
        protected Couleur couleurAmbiance;
        protected float intensiteAmbiance;
        protected List<Lumiere> lampes;
        protected List<Formes> objets;

        public Scene(Couleur couleurAmbiante,float intensiteAmbiante)
        {
            this.couleurAmbiance = couleurAmbiante;
            this.intensiteAmbiance = intensiteAmbiante;
            this.lampes = new List<Lumiere>();
            this.objets = new List<Formes>();
        }

        public void AddObjet(Formes objet) { objets.Add(objet); }
        public void AddLampe(Lumiere lampe) { lampes.Add(lampe); }

        public void DessineSansLumiere()
        {
            float[,] ZBuffer = new float[BitmapEcran.GetWidth(), BitmapEcran.GetHeight()];
            for (int xz = 0; xz < BitmapEcran.GetWidth(); xz++)
                for (int yz = 0; yz < BitmapEcran.GetHeight(); yz++)
                    ZBuffer[xz, yz] = float.MaxValue;
            float pas = 0.01f;
            foreach(Formes forme in objets)
            {
                foreach(V3 point in forme.GeneratePositions(pas))
                {
                    if (point.y < ZBuffer[(int)point.x, (int)point.z])
                    {
                        ZBuffer[(int)point.x, (int)point.z] = point.y;
                        BitmapEcran.DrawPixel((int)point.x, (int)point.z, forme.GetCouleur());
                    }
                }
            }

        }

        public void Dessine()
        {
            Lumiere monolumos = new Lumiere(new Couleur(1.0f, 1.0f, 1.0f), 0.8f, new V3(1.0f, -1.0f, 1.0f));
            float[,] ZBuffer = new float[BitmapEcran.GetWidth(), BitmapEcran.GetHeight()];
            for (int xz = 0; xz < BitmapEcran.GetWidth(); xz++)
                for (int yz = 0; yz < BitmapEcran.GetHeight(); yz++)
                    ZBuffer[xz, yz] = float.MaxValue;
            float pas = 0.01f;
            foreach (Formes forme in objets)
            {
                foreach (V3 point in forme.GeneratePositions(pas))
                {
                    if (point.y < ZBuffer[(int)point.x, (int)point.z])
                    {
                        ZBuffer[(int)point.x, (int)point.z] = point.y;
                        Couleur lumiereTotale = new Couleur(0, 0, 0);
                        V3 normalPoint = point;
                        normalPoint.Normalize();
                        lumiereTotale += forme.GetCouleur() * couleurAmbiance * intensiteAmbiance;
                        float facteurDiffus = monolumos.GetDirection() * normalPoint;
                        lumiereTotale += forme.GetCouleur() * monolumos.GetCouleur() * facteurDiffus;
                        //Couleur couleurPoint = forme.GetCouleur() *(monolumos.GetCouleur()*(point*monolumos.GetDirection())+(couleurAmbiance*intensiteAmbiance));
                        BitmapEcran.DrawPixel((int)point.x, (int)point.z, lumiereTotale);
                    }
                }
            }

        }
    }
}
