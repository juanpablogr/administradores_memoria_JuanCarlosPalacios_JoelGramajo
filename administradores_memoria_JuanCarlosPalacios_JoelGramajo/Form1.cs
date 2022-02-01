using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace administradores_memoria_JuanCarlosPalacios_JoelGramajo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            esquema_usuario_unico feuu = new esquema_usuario_unico();
            feuu.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            particiones_fijas fpf = new particiones_fijas();
            fpf.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pronto veras algo legendario aquí. Nuestro equipo trabaja en ello...", "Gracias por tu visita", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pronto veras algo legendario aquí. Nuestro equipo trabaja en ello...", "Gracias por tu visita", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
