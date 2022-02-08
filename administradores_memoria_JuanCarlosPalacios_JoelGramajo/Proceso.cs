using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace administradores_memoria_JuanCarlosPalacios_JoelGramajo
{
    class Proceso
    {
        private string id;
        private int size;
        private string state;

        public Proceso(string pid, int psize)
        {
            Id = pid;
            Size = psize;
            State = "Próximo a ejecutar";
        }

        public void PonerEnEspera()
        {
            State = "En espera";
        }

        public void PonerAlertaOverflow()
        {
            State = "Imposible alojar";
        }

        public string Id { get => id; set => id = value; }
        public int Size { get => size; set => size = value; }
        public string State { get => state; set => state = value; }
    }
}
