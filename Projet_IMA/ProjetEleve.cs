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
            Couleur blanc = new Couleur(1.0f, 1.0f, 1.0f);
            Couleur vert = new Couleur(0.0f, 1.0f, 0.0f);
            Couleur rouge = new Couleur(1.0f, 0.0f, 0.0f);
            Scene scene1 = new Scene(blanc, 0.1f);
            Sphere sphere1 = new Sphere(rouge, new V3(200.0f, 0, 200.0f), 50.0f);
            scene1.AddObjet(sphere1);

            scene1.Dessine();
        }

    }
}
