using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Projet_IMA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = BitmapEcran.Init(pictureBox1.Width, pictureBox1.Height);
        }

        public bool Checked()               { return checkBox1.Checked;   }
        public void PictureBoxInvalidate()  { pictureBox1.Invalidate(); }
        public void PictureBoxRefresh()     { pictureBox1.Refresh();    }

        private void button1_Click(object sender, EventArgs e)
        {
            BitmapEcran.RefreshScreen(new Couleur(0,0,0));
            ProjetEleve.Go();
            BitmapEcran.Show();          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BitmapEcran.RefreshScreen(new Couleur(0, 0, 0));
            ProjetEleve.Sphere(200,200,200,200,new Couleur(1.0f,0,0));
            BitmapEcran.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BitmapEcran.RefreshScreen(new Couleur(0, 0, 0));
            ProjetEleve.SphereZBuffer();
            BitmapEcran.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(1.0f,1.0f,1.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(1.0f, 0.0f, 0.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(1.0f, 1.0f, 0.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(0.0f, 1.0f, 0.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(0.0f, 1.0f, 1.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(0.0f, 0.0f, 1.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(1.0f, 0.0f, 1.0f));
            BitmapEcran.Show();
            BitmapEcran.RefreshScreen(new Couleur(0.5f, 0.5f, 0.5f));
            ProjetEleve.LightTesting(new Couleur(0.0f, 0.0f, 0.0f));
            BitmapEcran.Show();
        }
    }
}
