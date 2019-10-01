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
            Scene scene1 = new Scene(blanc, 0.1f);
            Sphere sphere1 = new Sphere(blanc, new V3(500.0f, 0, 300.0f), 200.0f);
            //Sphere sphere2 = new Sphere(bizarre2, new V3(200.0f, 0, 300.0f), 200.0f);
            Lumiere lampe = new Lumiere(blanc, 0.8f, new V3(1.0f, -1.0f, 1.0f));
            //Lumiere lampe2 = new Lumiere(bizarre3, 0.8f, new V3(-1.0f,1.0f,-1.0f));
            scene1.AddObjet(sphere1);
            //scene1.AddObjet(sphere2);
            scene1.AddLampe(lampe);
            //scene1.AddLampe(lampe2);
            scene1.Dessine();
        }

    }
}
