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

            V3 camera = new V3((float)BitmapEcran.GetWidth() / 2,(float) BitmapEcran.GetWidth() * -1.5f, (float)BitmapEcran.GetHeight() / 2);
            foreach (Formes forme in objets)
            {
                foreach (V3 point in forme.GeneratePositions(pas))
                {
                    if (point.x < BitmapEcran.GetWidth() && point.z < BitmapEcran.GetHeight() && (point.y < ZBuffer[(int)point.x, (int)point.z]))
                    {
                        ZBuffer[(int)point.x, (int)point.z] = point.y;
                        V3 normalPoint = new V3(point - forme.GetPosition());
                        normalPoint.Normalize();

                        V3 directionOculaire = new V3(camera - point);
                        directionOculaire.Normalize();
                        
                        Couleur lumiereTotale = new Couleur(0, 0, 0);
                        lumiereTotale += forme.GetCouleur() * couleurAmbiance * intensiteAmbiance;
                        foreach (Lumiere lampe in lampes)
                        {
                            V3 directionLampeNormale = new V3(lampe.GetDirection());
                            directionLampeNormale.Normalize();

                            float cosAlpha = normalPoint * directionLampeNormale;
                            float facteurDiffus = Math.Max(0, cosAlpha);
                            lumiereTotale += forme.GetCouleur() * lampe.GetCouleur() * facteurDiffus;

                            V3 reflet = (cosAlpha * (point - forme.GetPosition())) - directionLampeNormale;
                            reflet.Normalize();

                            float facteurSpeculaire = (float)Math.Pow(Math.Max((directionOculaire * reflet),0), puissanceSpeculaire);
                            lumiereTotale += lampe.GetCouleur() * facteurSpeculaire;
                        }
                        BitmapEcran.DrawPixel((int)point.x, (int)point.z, lumiereTotale);
                    }
                }
            }

        }
    }
}
