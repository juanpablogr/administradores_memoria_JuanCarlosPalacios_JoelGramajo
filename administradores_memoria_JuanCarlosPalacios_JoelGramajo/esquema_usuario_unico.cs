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
    public partial class esquema_usuario_unico : Form
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

        int peso_so;
        int job_index = 1;
        bool busy = false;

        public esquema_usuario_unico()
        {
            InitializeComponent();
        }

        private void esquema_usuario_unico_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = peso_procesos[i] + " KB | " + (char)(65 + i) + " | " + listBox1.Items[i];
            }
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label8.Text = trackBar1.Value + "%";
            // ejecutandose, detenida, en espera - DimGray - ForestGreen
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Maximum = Convert.ToInt32(textBox3.Text);
                groupBox3.Enabled = false;
                label11.BackColor = Color.ForestGreen;
                label11.Text = "ejecutandose";
                AddRegister("Cargando S.O.");

                label19.Text = textBox3.Text + " KB";

                peso_so = (trackBar1.Value * progressBar1.Maximum) / 100;

                for (int i = 1; i <= peso_so; i++)
                {
                    UpdateProgressBar(i);
                }

                AddRegister("S.O. cargado con éxito!");
                button3.Enabled = true;

                groupBox1.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Parámetros de la máquina incorrectos", "ERROR 001", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        void LoadJob0()
        {
            if (listBox2.Items.Count == 0)
            {
                busy = false;
                label18.Text = "0";
            }
            else
            {
                int initial_value = progressBar1.Value;
                int job_idx = Convert.ToInt32(Convert.ToChar(listBox2.Items[0].ToString().Split(' ')[4])) - 65;

                AddRegister("Cargando proceso de " + peso_procesos[job_idx] + " KB");
                AddRegister("Ejecutando ID " + listBox2.Items[0].ToString().Split(' ')[8]);
                listBox2.Items.RemoveAt(0);

                for (int i = initial_value + 1; i <= initial_value + peso_procesos[job_idx]; i++)
                {
                    try
                    {
                        UpdateProgressBar(i);
                        label18.Text = (i + 1) + "";
                    }
                    catch
                    {
                        textBox5.Text = "ERROR: No hay memoria disponible para alojar el proceso";
                        label11.Text = "detenida";
                        label11.BackColor = Color.Red;
                        AddRegister("Memoria insuficiente");
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.Items.Add("Proceso asociado al script " + (char)(65+listBox1.SelectedIndex) + " con el ID: " + job_index);
                AddRegister("Proceso con ID " + job_index + " agregado");
                job_index++;

                if (!busy)
                {
                    busy = true;
                    LoadJob0();
                }
            }
            else
            {
                MessageBox.Show("No ha seleccionado ningún proceso", "ERROR 002", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        void UpdateProgressBar(int v)
        {
            progressBar1.Value = v;
            label15.Text = v + "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (busy)
            {
                AddRegister("Finalizando proceso actual");

                for (int i = progressBar1.Value; i >= peso_so; i--)
                {
                    UpdateProgressBar(i);
                }

                label11.Text = "ejecutandose";
                label11.BackColor = Color.ForestGreen;
                textBox5.Text = "";

                LoadJob0();
            }
            else
            {
                MessageBox.Show("No hay otro proceso ademas del S.O.", "ERROR 003", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label18_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
