using System.ComponentModel.DataAnnotations;

namespace LojaProdutos.Dto.Produto
{
    public class EditarProdutoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite um nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a marca do Produto")]
        public string Marca { get; set; }
        public string? Foto { get; set; }

        [Required(ErrorMessage = "Digite o valor do produto")]
        public double Valor { get; set; }

        [Required(ErrorMessage = "Informe a quantidade")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "Selecione a Categoria")]
        public int CategoriaModelId { get; set; }
    }
}
