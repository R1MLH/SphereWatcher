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
        protected int puissanceSpeculaire;
        protected List<Lumiere> lampes;
        protected List<Formes> objets;

        public Scene(Couleur couleurAmbiante,float intensiteAmbiante,int puissanceSpeculaire)
        {
            this.couleurAmbiance = couleurAmbiante;
            this.intensiteAmbiance = intensiteAmbiante;
            this.lampes = new List<Lumiere>();
            this.objets = new List<Formes>();
            this.puissanceSpeculaire = puissanceSpeculaire;
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
                foreach(PointColore point in forme.GeneratePositions(pas))
                {
                    if (point.GetLoc().y < ZBuffer[(int)point.GetLoc().x, (int)point.GetLoc().z])
                    {
                        ZBuffer[(int)point.GetLoc().x, (int)point.GetLoc().z] = point.GetLoc().y;
                        BitmapEcran.DrawPixel((int)point.GetLoc().x, (int)point.GetLoc().z, point.GetCouleur());
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

            V3 camera = new V3((float)BitmapEcran.GetWidth() / 2,(float) BitmapEcran.GetWidth() * -1.5f, (float)BitmapEcran.GetHeight() / 2);
            foreach (Formes forme in objets)
            {
                foreach (PointColore point in forme.GeneratePositions(pas))
                {
                    if (point.GetLoc().x < BitmapEcran.GetWidth() && point.GetLoc().z < BitmapEcran.GetHeight() && (point.GetLoc().y < ZBuffer[(int)point.GetLoc().x, (int)point.GetLoc().z]))
                    {
                        ZBuffer[(int)point.GetLoc().x, (int)point.GetLoc().z] = point.GetLoc().y;

                        V3 normalPoint = point.GetNormale();
                        V3 directionOculaire = new V3(camera - point.GetLoc());
                        directionOculaire.Normalize();
                        
                        Couleur lumiereTotale = new Couleur(0, 0, 0);
                        lumiereTotale += point.GetCouleur() * couleurAmbiance * intensiteAmbiance;
                        foreach (Lumiere lampe in lampes)
                        {
                            V3 directionLampeNormale = new V3(lampe.GetDirection());
                            directionLampeNormale.Normalize();

                            float cosAlpha = normalPoint * directionLampeNormale;
                            float facteurDiffus = Math.Max(0, cosAlpha);
                            lumiereTotale += point.GetCouleur() * lampe.GetCouleur() * facteurDiffus;

                            V3 reflet = (cosAlpha * (point.GetLoc() - forme.GetPosition())) - directionLampeNormale;
                            reflet.Normalize();

                            float facteurSpeculaire = (float)Math.Pow(Math.Max((directionOculaire * reflet),0), puissanceSpeculaire);
                            lumiereTotale += lampe.GetCouleur() * facteurSpeculaire;
                        }
                        BitmapEcran.DrawPixel((int)point.GetLoc().x, (int)point.GetLoc().z, lumiereTotale);
                    }
                }
            }

        }
    }
}
