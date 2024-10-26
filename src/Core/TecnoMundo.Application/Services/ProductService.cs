using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;

namespace TecnoMundo.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductVO> Create(CreateProductVO vo)
        {
            var product = _mapper.Map<Product>(vo);
            var productCreate = await _repository.Create(product);
            return _mapper.Map<ProductVO>(productCreate);
        }

        public async Task<bool> Delete(Guid id)
        {
            var deleted = await _repository.Delete(id);
            return deleted;
        }

        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            var products = await _repository.FindAll();
            return _mapper.Map<IEnumerable<ProductVO>>(products);
        }

        public async Task<IEnumerable<CategoryVO>> FindAllCategories()
        {
            var categories = await _repository.FindAllCategories();
            return _mapper.Map<IEnumerable<CategoryVO>>(categories);
        }

        public async Task<ProductVO> FindById(Guid id)
        {
            var product = await _repository.FindById(id);
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<IEnumerable<ProductVO>> FindProductsByCategoryId(Guid id)
        {
            var products = await _repository.FindProductsByCategoryId(id);
            return _mapper.Map<IEnumerable<ProductVO>>(products);
        }

        public async Task<IEnumerable<ProductVO>> ProductFilter(string? name, decimal? priceOf, decimal? priceUpTo)
        {
            var products = await _repository.ProductFilter(name, priceOf, priceUpTo);
            return _mapper.Map<IEnumerable<ProductVO>>(products);
        }

        public async Task<ProductVO> Update(ProductVO vo)
        {
            var product = _mapper.Map<Product>(vo);
            var productUpdated = await _repository.Update(product);
            return _mapper.Map<ProductVO>(productUpdated);
        }
    }
}
