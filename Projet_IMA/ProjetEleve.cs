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

            Scene scene1 = new Scene(blanc, 0.1f,100);

            Sphere sphere1 = new Sphere("brick01.jpg", new V3(400.0f, 0, 300.0f), 200.0f);
            scene1.AddObjet(sphere1);
            Sphere sphere2 = new Sphere("carreau.jpg", new V3(100.0f, 0, 100.0f), 100.0f);
            scene1.AddObjet(sphere2);
            Sphere sphere3 = new Sphere("gold.jpg", new V3(800.0f, 0, 400.0f), 50.0f);
            scene1.AddObjet(sphere3);

            Lumiere lampe = new Lumiere(blanc, 0.8f, new V3(1.0f, -1.0f, 1.0f));
            scene1.AddLampe(lampe);

            scene1.Dessine();
        }

    }
}
