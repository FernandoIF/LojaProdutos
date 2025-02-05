using LojaProdutos.Data;
using LojaProdutos.Models;
using LojaProdutos.Services.Produto;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutos.Services.Estoque
{
    public class EstoqueService : IEstoqueInterface
    {
        private readonly DataContext _dataContext;
        private readonly IProdutoInterface _produtoInterface;

        public EstoqueService(DataContext dataContext, IProdutoInterface produtoInterface)
        {
            _dataContext = dataContext;
            _produtoInterface = produtoInterface;
        }
        public async Task<ProdutosBaixadosModel> CriarRegistro(int idProduto)
        {
            try
            {
                var produto = await _produtoInterface.BuscarProdutoPorId(idProduto);

                var produtoBaixado = new ProdutosBaixadosModel()
                {
                    Produto = produto,
                    ProdutoModelId = idProduto,
                };

                _dataContext.Add(produtoBaixado);
                await _dataContext.SaveChangesAsync();

                BaixarEstoque(produto);

                _dataContext.Update(produto);
                await _dataContext.SaveChangesAsync();

                return produtoBaixado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void BaixarEstoque(ProdutoModel produto)
        {
            produto.QuantidadeEstoque--;
        }

        public List<RegistrosProdutosModel> ListagemRegistros()
        {
            try
            {
                var resultado = from c in _dataContext.ProdutosBaixados.Include(x => x.Produto)
                                                                       .Include(x => x.Produto.Categoria)
                                                                       .ToList()
                                group c by new { c.Produto.CategoriaModelId, c.DataDaBaixa } into total
                                select new
                                {
                                    ProdutoId = total.First().Produto.Categoria.Id,
                                    CategoriaNome = total.First().Produto.Categoria.Nome,
                                    DataCompra = total.First().DataDaBaixa,
                                    Total = total.Sum(c => c.Produto.Valor)
                                };

                var totalGeral = _dataContext.ProdutosBaixados.Include(x => x.Produto)
                                                              .Include(x => x.Produto.Categoria)
                                                              .Sum(c => c.Produto.Valor);

                List<RegistrosProdutosModel> lista = new List<RegistrosProdutosModel>();

                foreach(var result in resultado)
                {
                    var registro = new RegistrosProdutosModel()
                    {
                        ProdutoId = result.ProdutoId,
                        CategoriaNome = result.CategoriaNome,
                        DataCompra = result.DataCompra,
                        Total = result.Total,
                        TotalGeral = totalGeral
                    };

                    lista.Add(registro);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
