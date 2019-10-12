using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    static class ProjetEleve
    {

        public static void Go()
        {
            Random rand = new Random();
            Couleur blanc = new Couleur(1.0f, 1.0f, 1.0f);
            Couleur bizarre1 = new Couleur((float) rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            Couleur bizarre2 = new Couleur((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            Couleur bizarre3 = new Couleur((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            Couleur rouge = new Couleur(1.0f, 0.0f, 0.0f);

            Scene scene1 = new Scene(blanc, 0.1f,50);

            Sphere sphere1 = new Sphere("carreau.jpg", "bump38.jpg", new V3(200.0f, 0.0f, 200.0f), 100.0f);
            scene1.AddObjet(sphere1);
            Sphere sphere2 = new Sphere("lead.jpg", "bump38.jpg", new V3(350.0f, - 150.0f, 350.0f), 50.0f);
            scene1.AddObjet(sphere2);

            Quadrilatere quad1 = new Quadrilatere("brick01.jpg", "bump38.jpg", new V3(650.0f, 1000.0f, 100.0f), new V3(750.0f, 10, 100.0f), new V3(650.0f, 1000.0f, 300.0f));
            scene1.AddObjet(quad1);

            //Quadrilatere quad3 = new Quadrilatere(rouge, "n", new V3(0, 0, 0), new V3(10, 0, 0), new V3(0, 0, 10));
            //scene1.AddObjet(quad3);

            Lumiere key = new Lumiere(blanc, 0.48f, new V3(1.0f, -1.0f, 1.0f));
            scene1.AddLampe(key);
            Lumiere fill = new Lumiere(blanc, 0.27f, new V3(-1.0f, -1.0f, 1.0f));
            scene1.AddLampe(fill);
            Lumiere back = new Lumiere(blanc, 0.2f, new V3(-1.0f, 1.0f, -1.0f));
            scene1.AddLampe(back);

            scene1.DessineRaycast();
        }

    }
}
