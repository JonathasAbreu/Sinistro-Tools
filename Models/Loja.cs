using System;

namespace LojaAPI.Models
{
    public class Loja
    {
        public int Codigo { get; set; }
        public string? Nome { get; set; }
        public string? Responsavel { get; set; }
        public int MaiorMargem { get; set; }
        public int MenorMargem { get; set; }
        public bool CobraMaoDeObra { get; set; }
        public string? Observacao { get; set; }
    }
}