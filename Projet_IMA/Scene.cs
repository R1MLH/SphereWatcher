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
            
            foreach(Formes forme in objets)
            {
                foreach(PointColore point in forme.GeneratePositions())
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

            V3 camera = new V3((float)BitmapEcran.GetWidth() / 2,(float) BitmapEcran.GetWidth() * -1.5f, (float)BitmapEcran.GetHeight() / 2);
            foreach (Formes forme in objets)
            {
                foreach (PointColore point in forme.GeneratePositions())
                {
                    if (point.GetLoc().x < BitmapEcran.GetWidth() && point.GetLoc().z < BitmapEcran.GetHeight() && (point.GetLoc().y < ZBuffer[(int)point.GetLoc().x, (int)point.GetLoc().z]))
                    {
                        ZBuffer[(int)point.GetLoc().x, (int)point.GetLoc().z] = point.GetLoc().y;

                        V3 normalPoint = point.GetNormale();
                        V3 directionOculaire = new V3(camera - point.GetLoc());
                        //directionOculaire.Normalize();
                        
                        Couleur lumiereTotale = new Couleur(0, 0, 0);
                        lumiereTotale += point.GetCouleur() * couleurAmbiance * intensiteAmbiance;
                        foreach (Lumiere lampe in lampes)
                        {
                            V3 directionLampeNormale = new V3(lampe.GetDirection());
                            directionLampeNormale.Normalize();

                            float cosAlpha =  normalPoint * directionLampeNormale;
                            float facteurDiffus = Math.Max(0, cosAlpha);
                            lumiereTotale += point.GetCouleur() * lampe.GetCouleur() * facteurDiffus;

                            V3 reflet = (2 * cosAlpha * normalPoint) - directionLampeNormale;
                            float produitSpeculaire = Math.Max(0,(reflet * directionOculaire)/(reflet.Norm()*directionOculaire.Norm()));
                            float facteurSpeculaire = (float)Math.Pow(produitSpeculaire, puissanceSpeculaire);
                            lumiereTotale += lampe.GetCouleur() * facteurSpeculaire;
                        }
                        BitmapEcran.DrawPixel((int)point.GetLoc().x, (int)point.GetLoc().z, lumiereTotale);
                    }
                }
            }

        }

        public void DessineRaycast()
        {
            V3 camera = new V3((float)BitmapEcran.GetWidth() / 2, (float)BitmapEcran.GetWidth() * -1.5f, (float)BitmapEcran.GetHeight() / 2);

            int xmax = BitmapEcran.GetWidth();
            int ymax = BitmapEcran.GetHeight();
            for ( int x_ecran = 0; x_ecran < BitmapEcran.GetWidth(); x_ecran++)
            {
                for( int y_ecran = 0; y_ecran < BitmapEcran.GetHeight(); y_ecran++)
                {
                    V3 pixel = new V3((float)x_ecran, 0, (float)y_ecran);
                    V3 rayon = pixel - camera;
                    rayon.Normalize();
                    Couleur c = Raycast(camera,rayon);
                    BitmapEcran.DrawPixel(x_ecran, y_ecran, c);

                }
            }
        }

        public Couleur Raycast(V3 camera, V3 rayon)
        {
            Dictionary<float, Formes> intersections = new Dictionary<float, Formes>();
            foreach(Formes objet in objets){
                float intersect = objet.IntersectRayon(camera, rayon);
                if( intersect >= 0 && !(intersections.Keys.Contains(intersect)))
                {
                    intersections.Add(intersect, objet);
                }
            }

            if (intersections.Keys.Count == 0)
            {
                return new Couleur(0, 0, 0);
            }
            else return new Couleur(1, 1, 1);

        }
    }
}
