using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    static class ProjetEleve
    {
        public struct SpatialLocator
        {
            public float x;
            public float y;
            public float z;

        }
        public static void Go()
        {

            Texture T1 = new Texture("brick01.jpg");
           
            int larg = 600;
            int haut = 300;
            float r_x = 1.5f;   // repetition de la texture en x
            float r_y = 1.0f;   // repetition de la texture en y
            float pas = 0.001f;

            for (float u = 0 ; u < 1 ; u+=pas)  // echantillonage fnt paramétrique
            for (float v = 0 ; v < 1 ; v+=pas)
                {
                    int x = (int) (u * larg + 10); // calcul des coordonnées planes
                    int y = (int) (v * haut + 15);

                    Couleur c = T1.LireCouleur(u * r_x, v * r_y);
                    
                    BitmapEcran.DrawPixel(x,y,c );
                   
                }

            // dessin sur l'image pour comprendre l'orientation axe et origine du Bitmap
            
            Couleur Red = new Couleur(1.0f, 0.0f, 0.0f);
            for (int i = 0; i < 1000; i++)
                BitmapEcran.DrawPixel(i, i, Red);

            Couleur Green = new Couleur(0.0f, 1.0f, 0.0f);
            for (int i = 0; i < 1000; i++)
                BitmapEcran.DrawPixel(i, 1000-i, Green);

            // test des opérations sur les vecteurs

            V3 t = new V3(1, 0, 0);
            V3 r = new V3(0, 1, 0);
            V3 k = t + r;
            float p = k * t * 2;
            V3 n = t ^ r;
            V3 m = -t;

      
        }

        public static void Sphere(float xpos, float ypos, float zpos, int rayon, Couleur color)
        {

            float pas = 0.02f;
            for(float u = 0; u <= 2 * Math.PI; u += pas)
            {
                for (float v = -1* ((float)Math.PI)/2.0f; v <= Math.PI/2; v+= pas)
                {
                    int x = (int)(rayon * Math.Cos(u) * Math.Cos(v) + xpos);
                    int z = (int)(rayon * Math.Sin(v) + zpos);
                    BitmapEcran.DrawPixel(x, z, color);
                }
            }
        }

        public static void SphereZBuffer()
        {

            Couleur Red = new Couleur(1.0f, 0.0f, 0.0f);
            Couleur Green = new Couleur(0.0f, 1.0f, 0.0f);

            float[,] ZBuffer = new float[BitmapEcran.GetWidth(), BitmapEcran.GetHeight()];

            for (int xz = 0; xz < BitmapEcran.GetWidth(); xz++)
                for (int yz = 0; yz < BitmapEcran.GetHeight(); yz++)
                    ZBuffer[xz, yz] = float.MaxValue;

            float pas = 0.005f;
            int rayon = 200;
            int xpos = 200, ypos = 0, zpos = 200;
            for (float u = 0; u <= 2 * Math.PI; u += pas)
            {
                for (float v = -1 * ((float)Math.PI) / 2.0f; v <= Math.PI / 2; v += pas)
                {
                    int x = (int)(rayon * Math.Cos(u) * Math.Cos(v)) + xpos;
                    float y = (rayon * ((float)Math.Cos(v)) * (float)Math.Sin(u)) + ypos;
                    int z = (int)(rayon * Math.Sin(v)) + zpos;
                    if (y < ZBuffer[x, z])
                    {
                        ZBuffer[x, z] = y;
                        BitmapEcran.DrawPixel(x, z, Red);
                    }

                }
            }

            xpos = 350;
            for (float u = 0; u <= 2 * Math.PI; u += pas)
            {
                for (float v = -1 * ((float)Math.PI) / 2.0f; v <= Math.PI / 2; v += pas)
                {
                    int x = (int)(rayon * Math.Cos(u) * Math.Cos(v)) + xpos;
                    float y = (rayon * ((float)Math.Cos(v)) * (float)Math.Sin(u)) + ypos;
                    int z = (int)(rayon * Math.Sin(v)) + zpos;
                    if (y < ZBuffer[x, z])
                        BitmapEcran.DrawPixel(x, z, Green);

                }
            }
        }

        public static void LightTesting(Couleur LampColor)
        {
            int rayon = (int)BitmapEcran.GetWidth() / 18;

            Couleur blanc = new Couleur(1.0f, 1.0f, 1.0f);
            Couleur rouge = new Couleur(1.0f, 0.0f, 0.0f);
            Couleur vert = new Couleur(0.0f, 1.0f, 0.0f);
            Couleur bleu = new Couleur(0.0f, 0.0f, 1.0f);
            Couleur vouge = new Couleur(1.0f, 1.0f, 0.0f);
            Couleur blouge = new Couleur(1.0f, 0.0f, 1.0f);
            Couleur blert = new Couleur(0.0f, 1.0f, 1.0f);
            Couleur noir = new Couleur(0.0f, 0.0f, 0.0f);

            Sphere(rayon,0, rayon, rayon, LampColor * blanc);
            Sphere(3*rayon,0, rayon, rayon, LampColor * rouge);
            Sphere(5* rayon, 0, rayon, rayon, LampColor * vouge);
            Sphere(7* rayon, 0, rayon, rayon, LampColor * vert);
            Sphere(9* rayon, 0, rayon, rayon, LampColor * blert);
            Sphere(11* rayon, 0, rayon, rayon, LampColor * bleu);
            Sphere(13*rayon, 0, rayon, rayon, LampColor * blouge);
            Sphere(15*rayon, 0, rayon, rayon, LampColor * noir);


        }

    }
}
