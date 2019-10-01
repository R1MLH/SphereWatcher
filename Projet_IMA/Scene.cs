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

            float[,] ZBuffer = new float[BitmapEcran.GetWidth(), BitmapEcran.GetHeight()];
            for (int xz = 0; xz < BitmapEcran.GetWidth(); xz++)
                for (int yz = 0; yz < BitmapEcran.GetHeight(); yz++)
                    ZBuffer[xz, yz] = float.MaxValue;
            float pas = 0.005f;
            foreach (Formes forme in objets)
            {
                foreach (V3 point in forme.GeneratePositions(pas))
                {
                    if (point.x < BitmapEcran.GetWidth() && point.z < BitmapEcran.GetHeight() && (point.y < ZBuffer[(int)point.x, (int)point.z]))
                    {
                        ZBuffer[(int)point.x, (int)point.z] = point.y;
                        V3 normalPoint = point-forme.GetPosition();
                        normalPoint.Normalize();
                        
                        Couleur lumiereTotale = new Couleur(0, 0, 0);
                        lumiereTotale += forme.GetCouleur() * couleurAmbiance * intensiteAmbiance;
                        foreach (Lumiere lampe in lampes)
                        {
                            lampe.GetDirection().Normalize();
                            float facteurDiffus = Math.Max(0, normalPoint * lampe.GetDirection());
                            lumiereTotale += forme.GetCouleur() * lampe.GetCouleur() * facteurDiffus;
                        }
                        BitmapEcran.DrawPixel((int)point.x, (int)point.z, lumiereTotale);
                    }
                }
            }

        }
    }
}
