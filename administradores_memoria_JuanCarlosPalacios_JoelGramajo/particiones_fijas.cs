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
    public partial class particiones_fijas : Form
    {
        string[] file_names =
        new string[16]{
            "media",
            "areacuadrado",
            "arearectangulo",
            "areatriangulorectangulo",
            "multiplodetres",
            "parimpar",
            "pesomasa",
            "masapeso",
            "radio",
            "bin2dec",
            "kelvin2fahrenheit",
            "millas2km",
            "longitudtypes",
            "saludo",
            "primos",
            "fibonacci"
        };

        int[] peso_procesos =
        new int[16] {
            4461,
            2229,
            3798,
            3681,
            1864,
            5002,
            4400,
            3957,
            4217,
            3891,
            3170,
            4422,
            5234,
            1238,
            5977,
            6457
        };

        string main_dir = "C:\\Users\\juanp\\Documents\\codigos cpp";
        int part_len;

        public particiones_fijas()
        {
            InitializeComponent();
        }

        private void particiones_fijas_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = peso_procesos[i] + " KB | " + (char)(65 + i) + " | " + listBox1.Items[i];
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label8.Text = trackBar1.Value + "";
        }

        void AddRegister(string msg)
        {
            listBox3.Items.Add(">>> " + msg);

            while (listBox3.Items.Count > 2048)
            {
                listBox3.Items.RemoveAt(0);
            }

            listBox3.SelectedIndex = listBox3.Items.Count - 1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox1.SelectedIndex;
            string assembler_path = main_dir + "\\assembler_files\\" + file_names[idx] + ".s";
            string cpp_path = main_dir + "\\cpp_files\\" + file_names[idx] + ".cpp";

            string assembler_code = System.IO.File.ReadAllText(assembler_path);
            string cpp_code = System.IO.File.ReadAllText(cpp_path);

            textBox1.Text = cpp_code.Replace(Convert.ToString((char)13) + Convert.ToString((char)10), Convert.ToString((char)10)).Replace(Convert.ToString((char)10), Convert.ToString((char)13) + Convert.ToString((char)10)); // 13, 10 => 10 and 10 => 13, 10
            textBox2.Text = assembler_code.Replace(Convert.ToString((char)13) + Convert.ToString((char)10), Convert.ToString((char)10)).Replace(Convert.ToString((char)10), Convert.ToString((char)13) + Convert.ToString((char)10));

        }

        void UpdatePartition(int number, bool state, int value) // mostrar uso en el label, el maximum progressbar, label del maximum
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                part_len = Convert.ToInt32(textBox3.Text);
                groupBox3.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Parámetros de la máquina incorrectos", "ERROR 001", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
