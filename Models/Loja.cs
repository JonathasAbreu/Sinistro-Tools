using System;

namespace LojaAPI.Models
{
    public class Loja
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Responsável { get; set; }
        public int MaiorMargem { get; set; }
        public int MenorMargem { get; set; }
        public Boolean CobraMaoDeObra { get; set; }
        public required string Observacao { get; set; }
    }
}