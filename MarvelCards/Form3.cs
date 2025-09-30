using MarvelCards.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarvelCards
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            button2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {                        
            this.Close();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = true;
            pictureBox1.BackgroundImage = Resources.coisa;
            pictureBox2.BackgroundImage = Resources.homelas;
            pictureBox3.BackgroundImage = Resources.tochahum;
            pictureBox4.BackgroundImage = Resources.magneto;
            pictureBox5.BackgroundImage = Resources.doctordoom;
            pictureBox6.BackgroundImage = Resources.thanos;
            pictureBox7.BackgroundImage = Resources.electro;
            pictureBox8.BackgroundImage = Resources.homareia;
            pictureBox9.BackgroundImage = Resources.deadpool;
            pictureBox10.BackgroundImage = Resources.reidocrime;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible= false;
            button1.Visible= true;
            pictureBox1.BackgroundImage = Resources.miranha;
            pictureBox2.BackgroundImage = Resources.kraven;
            pictureBox3.BackgroundImage = Resources.ironm;
            pictureBox4.BackgroundImage = Resources.capi;
            pictureBox5.BackgroundImage = Resources.carni;
            pictureBox6.BackgroundImage = Resources.lagarto;
            pictureBox7.BackgroundImage = Resources.venom;
            pictureBox8.BackgroundImage = Resources.octo;
            pictureBox9.BackgroundImage = Resources.mist;
            pictureBox10.BackgroundImage = Resources.capimarvel;
        }
    }
}
