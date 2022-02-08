using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace administradores_memoria_JuanCarlosPalacios_JoelGramajo
{
    class Bloque
    {
        private bool libre;
        private string id;
        private int size;

        public Bloque(int bsize)
        {
            Libre = true;
            Id = "Free";
            Size = bsize;
        }

        public Bloque(Proceso proceso)
        {
            Libre = false;
            Id = proceso.Id;
            Size = proceso.Size;
        }

        public Bloque(string pid, int psize, bool plibre)
        {
            Libre = plibre;
            Id = pid;
            Size = psize;
        }

        public void ReleaseBlock()
        {
            Libre = true;
            Id = "Free";
        }

        public bool Libre { get => libre; set => libre = value; }
        public string Id { get => id; set => id = value; }
        public int Size { get => size; set => size = value; }
    }
}
