namespace LojaProdutos.Models
{
    public class RegistrosProdutosModel
    {
        public int ProdutoId { get; set; }
        public string CategoriaNome { get; set; }
        public double Total { get; set; }
        public DateTime DataCompra {  get; set; }
        public double TotalGeral { get; set; }
    }
}
