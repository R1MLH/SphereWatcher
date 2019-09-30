using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Projet_IMA
{
    public struct LocalisateurSpatial
    {
        public float X, Y, Z;	// composantes X,Y,Z permettant de définir la localisation dans l'espace

        // constructeurs

        public LocalisateurSpatial(float pX, float pY, float pZ)
        {
            X = pX;
            Y = pY;
            Z = pZ;
        }

        public LocalisateurSpatial(LocalisateurSpatial autre)
        {
            X = autre.X;
            Y = autre.Y;
            Z = autre.Z;
        }

        // méthodes

        
        // opérateurs surchargés

        
    }
}

    

    				
