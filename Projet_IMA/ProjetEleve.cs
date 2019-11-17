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
        }


        public static Scene Scene1()
        {
            Couleur blanc = new Couleur(1.0f, 1.0f, 1.0f);
            Scene scene = new Scene(blanc, 0.05f, 50);
          
            Couleur orangesque = new Couleur(1.0f, 0.78f, 0.59f);
            Couleur bleuesque = new Couleur(0.78f, 0.59f, 1.0f);
            Couleur rouge = new Couleur(1.0f, 0.0f, 0.0f);

            
            Sphere sphere1 = new Sphere("carreau.jpg", "bump38.jpg", new V3(200.0f, 0.0f, 200.0f), 100.0f);
            scene.AddObjet(sphere1);
            Sphere sphere2 = new Sphere("lead.jpg", "bump38.jpg", new V3(350.0f, - 150.0f, 350.0f), 50.0f);
            scene.AddObjet(sphere2);
            Sphere sphere3 = new Sphere("brick01.jpg", "bump38.jpg", new V3(350.0f, 700.0f, 120.0f), 50.0f);
            scene.AddObjet(sphere3);


            Quadrilatere quad1 = new Quadrilatere("brick01.jpg", "bump38.jpg", new V3(650.0f, 1000.0f, 100.0f), new V3(750.0f, 10, 100.0f), new V3(650.0f, 1000.0f, 300.0f));
            scene.AddObjet(quad1);


            Quadrilatere sol = new Quadrilatere("wood.jpg", "bump1.jpg", new V3(500.0f, 0.0f, 0.0f), new V3(1000.0f, 500.0f, 0.0f), new V3(0.0f, 500.0f, 0.0f));
            scene.AddObjet(sol);

            Quadrilatere mur = new Quadrilatere(blanc, "bump.jpg", new V3(0.0f, 500.0f, 0.0f), new V3(500.0f, 1000.0f, 0.0f), new V3(0.0f, 500.0f, 1000.0f));
            scene.AddObjet(mur);

            Quadrilatere mur2 = new Quadrilatere(blanc, "bump.jpg", new V3(500.0f, 1000.0f, 0.0f), new V3(1000.0f, 500.0f, 0.0f), new V3(500.0f, 1000.0f, 1000.0f));
            scene.AddObjet(mur2);

            //Quadrilatere quad3 = new Quadrilatere(rouge, "n", new V3(0, 0, 0), new V3(10, 0, 0), new V3(0, 0, 10));
            //scene1.AddObjet(quad3);

            Lumiere key = new LampeDirectionelle(orangesque, 0.48f, new V3(1.0f, -1.0f, 1.0f));
            scene.AddLampe(key);
            Lumiere fill = new LampeDirectionelle(bleuesque, 0.27f, new V3(-1.0f, -1.0f, 1.0f));
            scene.AddLampe(fill);
            Lumiere back = new LampeDirectionelle(blanc, 0.2f, new V3(-1.0f, 1.0f, -1.0f));
            scene.AddLampe(back);

            Lumiere centrale = new LampePonctuelle(blanc, 0.5f, new V3(650.0f, 900.0f, 500.0f), 0.001f);
            scene.AddLampe(centrale);
             centrale = new LampePonctuelle(blanc, 0.7f, new V3(250.0f, 900.0f, 500.0f), 0.001f);
            scene.AddLampe(centrale);




            return scene;
        }

        public static Scene Scene2()
        {
            Couleur blanc = new Couleur(1.0f, 1.0f, 1.0f);
            Scene scene = new Scene(blanc, 0.05f, 50);
            Couleur orangesque = new Couleur(1.0f, 0.78f, 0.59f);
            Couleur bleuesque = new Couleur(0.78f, 0.59f, 1.0f);
            Couleur rouge = new Couleur(1.0f, 0.0f, 0.0f);


            Quadrilatere sol = new Quadrilatere("wood.jpg", "bump1.jpg", new V3(500.0f, 0.0f, 0.0f), new V3(1000.0f, 500.0f, 0.0f), new V3(0.0f, 500.0f, 0.0f));
            scene.AddObjet(sol);

            Quadrilatere mur = new Quadrilatere(blanc, "bump.jpg", new V3(0.0f, 500.0f, 0.0f), new V3(500.0f, 1000.0f, 0.0f), new V3(0.0f, 500.0f, 1000.0f));
            scene.AddObjet(mur);

            Quadrilatere mur2 = new Quadrilatere(blanc, "bump.jpg", new V3(500.0f, 1000.0f, 0.0f), new V3(1000.0f, 500.0f, 0.0f), new V3(500.0f, 1000.0f, 1000.0f));
            scene.AddObjet(mur2);

            Lumiere key = new LampeDirectionelle(orangesque, 0.48f, new V3(1.0f, -1.0f, 1.0f));
            scene.AddLampe(key);
            Lumiere fill = new LampeDirectionelle(bleuesque, 0.27f, new V3(-1.0f, -1.0f, 1.0f));
            scene.AddLampe(fill);
            Lumiere back = new LampeDirectionelle(blanc, 0.2f, new V3(-1.0f, 1.0f, -1.0f));
            scene.AddLampe(back);

            Mesh mesh = new Mesh("teapot.obj", "gold.jpg");
            mesh.Rescale(100.0f);
            mesh.Translate(new V3(500.0f, 300.0f, 70.0f));



            foreach (Triangle t in mesh.GetPolygons())
            {
                scene.AddObjet(t);

            }

            return scene;
        }
    }
}
