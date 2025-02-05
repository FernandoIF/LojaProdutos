using LojaProdutos.Data;
using LojaProdutos.Dto.Produto;
using LojaProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutos.Services.Produto
{
    public class ProdutoService : IProdutoInterface
    {
        private readonly DataContext _dataContext;
        private readonly string _sistema;
        public ProdutoService(DataContext context, IWebHostEnvironment sistema)
        {
            _dataContext = context;
            _sistema = sistema.WebRootPath;
        }

        public async Task<List<ProdutoModel>> BuscarProdutoFiltro(string? pesquisar)
        {
            try
            {
                var produtos = await _dataContext.Produto
                    .Include(c => c.Categoria)
                    .Where(p => p.Nome.Contains(pesquisar) || p.Marca.Contains(pesquisar))
                    .ToListAsync();

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoModel> BuscarProdutoPorId(int id)
        {
            try
            {
                var produto = await _dataContext.Produto
                    .Include(c => c.Categoria)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProdutoModel>> BuscarProdutos()
        {
            try
            {
                return await _dataContext.Produto.Include(c => c.Categoria).ToListAsync();    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoModel> Cadastrar(CriarProdutoDTO criarProdutoDTO, IFormFile foto)
        {
            try
            {
                var nomeCaminhoImagem = GeraCaminhoArquivo(foto);

                var produto = new ProdutoModel
                {
                    Nome = criarProdutoDTO.Nome,
                    Marca = criarProdutoDTO.Marca,
                    Valor = criarProdutoDTO.Valor,
                    CategoriaModelId = criarProdutoDTO.CategoriaModelId,
                    Foto = nomeCaminhoImagem,
                    QuantidadeEstoque = criarProdutoDTO.QuantidadeEstoque
                };

                _dataContext.Produto.Add(produto);
                await _dataContext.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoModel> Editar(EditarProdutoDTO editarProdutoDTO, IFormFile? foto)
        {
            try
            {
                var produto = await BuscarProdutoPorId(editarProdutoDTO.Id);

                var nomeCaminhoImagem = "";

                if (foto != null)
                {
                    string caminhoCapaExistente = _sistema + "\\imagem\\" + produto.Foto;

                    if (File.Exists(caminhoCapaExistente))
                    {
                        File.Delete(caminhoCapaExistente);
                    }

                    nomeCaminhoImagem = GeraCaminhoArquivo(foto);
                }

                produto.Nome = editarProdutoDTO.Nome;
                produto.Marca = editarProdutoDTO.Marca;
                produto.Valor = editarProdutoDTO.Valor;
                produto.QuantidadeEstoque = editarProdutoDTO.QuantidadeEstoque;
                produto.CategoriaModelId = editarProdutoDTO.CategoriaModelId;

                if (nomeCaminhoImagem != "")
                {
                    produto.Foto = nomeCaminhoImagem;    
                }

                _dataContext.Update(produto);
                await _dataContext.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<ProdutoModel> Remover(int id)
        {
            try
            {
                var produto = await BuscarProdutoPorId(id);

                _dataContext.Remove(produto);
                await _dataContext.SaveChangesAsync();  

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GeraCaminhoArquivo(IFormFile foto)
        {
            var codigoUnico = Guid.NewGuid().ToString();
            var nomeCaminhoImagem = foto.FileName.Replace(" ", "").ToLower() + codigoUnico + ".png";

            var caminhoParaSalvarImagens = _sistema + "\\imagem\\";

            if(!Directory.Exists(caminhoParaSalvarImagens))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagens);
            }

            using (var stream = File.Create(caminhoParaSalvarImagens + nomeCaminhoImagem))
            {
                foto.CopyToAsync(stream).Wait();
            }

            return nomeCaminhoImagem;
        }
    }
}
