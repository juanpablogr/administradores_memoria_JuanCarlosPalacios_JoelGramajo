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
        int process_counter = 1;
        int max_part_size = 0;

        bool[] partitions_states = new bool[18] {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        };

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
            label30.Text = trackBar2.Value + "%";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
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

            textBox1.Text = cpp_code.Replace(Convert.ToString((char)13) + Convert.ToString((char)10), 
            Convert.ToString((char)10)).Replace(Convert.ToString((char)10), Convert.ToString((char)13) 
            + Convert.ToString((char)10)); // 13, 10 => 10 and 10 => 13, 10
            textBox2.Text = assembler_code.Replace(Convert.ToString((char)13) + Convert.ToString((char)10), 
            Convert.ToString((char)10)).Replace(Convert.ToString((char)10), Convert.ToString((char)13) + 
            Convert.ToString((char)10));
        }

        void LoadJobToPartition(int idx, int size, string process_letter, string identificador)
        {
            /*
             * foreach (Button item in f.Controls.OfType<Button>())
             * Now "item" is already of type Button so no need to use "as"
             */

            int maxmem = Convert.ToInt32(listBox4.Items[idx]);

            partitions_states[idx] = size > 0;

            foreach (GroupBox gpp in groupBox2.Controls.OfType<GroupBox>())
            {
                if (gpp.Name == "groupBox" + (idx + 5))
                {
                    gpp.Visible = true;
                    gpp.Text = "Partición " + idx + " - " + size + " KB / " + maxmem + " KB";

                    ProgressBar pbp = gpp.Controls.OfType<ProgressBar>().ToList()[0];
                    pbp.Maximum = maxmem;
                    pbp.Value = size;
                    pbp.Visible = true;

                    Label lmp = gpp.Controls.OfType<Label>().ToList()[0];
                    lmp.Text = process_letter + identificador;
                    lmp.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox4.Items.Count > 1)
            {
                labelMachineState.BackColor = Color.ForestGreen;
                labelMachineState.Text = "ejecutandose";

                groupBox3.Enabled = false;
                groupBox1.Enabled = true;

                AddRegister("Particionando memoria");

                for (int i = 0; i < listBox4.Items.Count; i++)
                {
                    LoadJobToPartition(i, 0, "FREE", "");
                    
                    if (i != 0)
                    {
                        comboBox1.Items.Add("Partición " + i);
                    }
                }

                AddRegister("Particionado finalizado");

                AddRegister("Cargando S.O.");
                LoadJobToPartition(0, (trackBar2.Value * Convert.ToInt32(listBox4.Items[0])) / 100, "S.", "O.");
                AddRegister("S.O. Cargado con éxito!");

                button3.Enabled = true;
                comboBox1.Enabled = true;
            }
            else
            {
                ShowError("Debe agregar por lo menos una particion para el S.O.", "ERROR 002");
            }
        }

        void ShowError(string desc, string head)
        {
            MessageBox.Show(desc, head, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void LoadStackToMemory()
        {
            for (int job_idx = 0; job_idx < listBox2.Items.Count; job_idx++)
            {
                int idx_pfree = -1;
                int indice_peso = Convert.ToInt32(listBox2.Items[job_idx].ToString()[0]) - 65;
                int peso_actual_job = peso_procesos[indice_peso];

                if (peso_actual_job <= max_part_size) // <= maximo
                {
                    for (int part_idx = 0; part_idx < listBox4.Items.Count; part_idx++)
                    {
                        if (!partitions_states[part_idx] && peso_actual_job <= Convert.ToInt32(listBox4.Items[part_idx]))
                        {
                            idx_pfree = part_idx;
                            break;
                        }
                    }

                    if (idx_pfree != -1)
                    {
                        AddRegister("Proceso " + listBox2.Items[job_idx] + " alojado en P" + idx_pfree);
                        LoadJobToPartition(idx_pfree, peso_actual_job, listBox2.Items[job_idx].ToString(), "");
                        listBox2.Items.RemoveAt(job_idx);
                        job_idx--;
                    }
                    else
                    {
                        AddRegister("Proceso " + listBox2.Items[job_idx] + " en espera");
                    }
                }
                else
                {
                    textBox5.Text = "El proceso " + listBox2.Items[job_idx] 
                    + " ha revasado el límite de capacidad de la partición más grande! " 
                    + peso_actual_job + " KB / " + max_part_size + " KB";
                    AddRegister("Imposible alojar " + listBox2.Items[job_idx] + " en memoria");
                }
            }
        }

        // FALTA EL TRY DEL OVERFLOW

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.Items.Add(Convert.ToString((char)(65 + listBox1.SelectedIndex)) + process_counter);
                AddRegister("Proceso " + listBox2.Items[listBox2.Items.Count - 1] + " agregado");
                process_counter++;
                LoadStackToMemory();
            }
            else
            {
                ShowError("No ha seleccionado ningún proceso", "ERROR 003");
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label30.Text = trackBar2.Value + "%";
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int tam = Convert.ToInt32(textBox3.Text);
                
                if (tam > 0)
                {
                    listBox4.Items.Add(tam);
                    textBox3.Text = "";

                    if (listBox4.Items.Count >= 18)
                    {
                        button4.Enabled = false;
                        textBox3.Enabled = false;
                    }

                    if (tam > max_part_size)
                    {
                        max_part_size = tam;
                    }
                }
                else
                {
                    ShowError("La partición debe ser mayor a 0 KB", "ERROR 005");
                }
            }
            catch
            {
                ShowError("Datos inválidos", "ERROR 001");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                int part_idx = comboBox1.SelectedIndex + 1;

                if (partitions_states[part_idx])
                {
                    LoadJobToPartition(part_idx, 0, "FREE", "");
                    LoadStackToMemory();
                    AddRegister("Partición " + part_idx + " liberada con éxito");
                }
                else
                {
                    MessageBox.Show("No hay un proceso asignado a la partición seleccionada!", "INFO"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                ShowError("Primero debe seleccionar una partición", "ERROR 004");
            }
        }
    }
}
