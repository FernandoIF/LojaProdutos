using LojaProdutos.Data;
using LojaProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutos.Services.Categoria
{
    public class CategoriaService : ICategoriaInterface
    {
        private readonly DataContext _dbContext;

        public CategoriaService(DataContext context)
        {
            _dbContext = context;
        }
        public async Task<List<CategoriaModel>> BuscarCategorias()
        {
            try
            {
                var categorias = await _dbContext.Categoria.ToListAsync();
                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
