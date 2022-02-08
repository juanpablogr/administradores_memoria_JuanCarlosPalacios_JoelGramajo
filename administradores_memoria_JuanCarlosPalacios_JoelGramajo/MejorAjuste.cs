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
    public partial class MejorAjuste : Form
    {
        List<Bloque> MemoryBlocks = new List<Bloque>();
        List<Proceso> ListaProcesos = new List<Proceso>();

        void RefreshGridViews()
        {
            dataGridView1.DataSource = new BindingList<Proceso>(ListaProcesos);
            dataGridView2.DataSource = new BindingList<Bloque>(MemoryBlocks);
        }

        void MergeAdjacentBlocks()
        {
            for (int idx = 0; idx < MemoryBlocks.Count - 1; idx++)
            {
                if (MemoryBlocks[idx].Libre && MemoryBlocks[idx + 1].Libre)
                {
                    int new_size = MemoryBlocks[idx].Size + MemoryBlocks[idx + 1].Size;

                    MemoryBlocks.RemoveAt(idx);
                    MemoryBlocks.RemoveAt(idx);
                    MemoryBlocks.Insert(idx, new Bloque(new_size));

                    idx--;
                }
            }
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

        int MaximumBlockSize()
        {
            int msize = 0;

            foreach (Bloque blk in MemoryBlocks)
            {
                if (blk.Size > msize) msize = blk.Size;
            }

            return msize;
        }

        void TryAllocateProcesses()
        {
            for (int pidx = 0; pidx < ListaProcesos.Count; pidx++)
            {
                if (ListaProcesos[pidx].Size <= Convert.ToInt32(textBox3.Text))
                {
                    if (ListaProcesos[pidx].Size <= MaximumBlockSize())
                    {
                        int idx_minloss = -1;
                        int minloss = Convert.ToInt32(textBox3.Text);

                        for (int bidx = 0; bidx < MemoryBlocks.Count; bidx++)
                        {
                            if (MemoryBlocks[bidx].Libre && ListaProcesos[pidx].Size <= MemoryBlocks[bidx].Size)
                            {
                                int bdif = MemoryBlocks[bidx].Size - ListaProcesos[pidx].Size;

                                if (bdif < minloss)
                                {
                                    minloss = bdif;
                                    idx_minloss = bidx;
                                }
                            }
                        }

                        if (idx_minloss < 0)
                        {
                            AddRegister("Proceso " + ListaProcesos[pidx].Id + " en espera");
                            ListaProcesos[pidx].PonerEnEspera();
                        }
                        else
                        {
                            MemoryBlocks.RemoveAt(idx_minloss);
                            MemoryBlocks.Insert(idx_minloss, new Bloque(ListaProcesos[pidx]));
                            if (minloss != 0) MemoryBlocks.Insert(idx_minloss + 1, new Bloque(minloss));
                            ListaProcesos.RemoveAt(pidx);
                            pidx--;
                        }
                    }
                    else // se queda
                    {
                        AddRegister("Proceso " + ListaProcesos[pidx].Id + " mayor que cualquier bloque, en espera");
                        ListaProcesos[pidx].PonerEnEspera();
                    }
                }
                else // no cambia
                {
                    AddRegister("Proceso " + ListaProcesos[pidx].Id + " mayor que el tamaño de la RAM");
                    ListaProcesos[pidx].PonerAlertaOverflow();
                }
            }
        }

        void AddProcess2ListAndTryAlloc(Proceso pr)
        {
            AddRegister("Proceso " + pr.Id + " agregado a la lista");
            ListaProcesos.Add(pr);
            MergeAdjacentBlocks();
            TryAllocateProcesses();
            RefreshGridViews();
        }

        void ShowError(string desc, string head)
        {
            MessageBox.Show(desc, head, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public MejorAjuste()
        {
            InitializeComponent();
        }

        private void MejorAjuste_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryBlocks.Add(new Bloque(Convert.ToInt32(textBox3.Text)));
                groupBox3.Enabled = false;
                groupBox1.Enabled = true;

                AddProcess2ListAndTryAlloc(new Proceso("S.O.", Convert.ToInt32(textBox3.Text) * trackBar2.Value / 100));

                labelMachineState.BackColor = Color.ForestGreen;
                labelMachineState.Text = "Ejecutandose";
            }
            catch
            {
                ShowError("Parámetros incorrectos", "001");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AddProcess2ListAndTryAlloc(new Proceso(textBox1.Text, Convert.ToInt32(textBox2.Text)));
                textBox1.Text = "";
                textBox2.Text = "";
            }
            catch
            {
                ShowError("Parámetros del proceso incorrectos", "002");
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label30.Text = trackBar2.Value + "%";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Index > 0)
            {
                MemoryBlocks[dataGridView2.CurrentRow.Index].ReleaseBlock();
                AddRegister("Bloque " + dataGridView2.CurrentRow.Index + " liberado");
                MergeAdjacentBlocks();
                TryAllocateProcesses();
                RefreshGridViews();
            }
        }
    }
}
