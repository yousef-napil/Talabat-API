using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.Entities.ProductModule;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;

namespace Talabat.API.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> productRepository;
        private readonly IGenericRepository<ProductBrand> brandRepository;
        private readonly IGenericRepository<ProductType> typeRepository;
        private readonly IMapper mapper;

        public ProductsController(  IGenericRepository<Product> productRepository ,
                                    IGenericRepository<ProductBrand> brandRepository,
                                    IGenericRepository<ProductType> typeRepository,
                                    IMapper mapper )
        {
            this.productRepository = productRepository;
            this.brandRepository = brandRepository;
            this.typeRepository = typeRepository;
            this.mapper = mapper;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> getProducts( [FromQuery] ProductSpecParams Params)
        {
            var spec = new ProductWithTypeAndBrandSpecification(Params);
            var countSpec = new ProductForCountSpecification(Params);
            var Count = await productRepository.CountAsync(countSpec);
            var Products = await productRepository.GetAllAsyncWithSpec(spec);
            var MappedProducts = mapper.Map<IReadOnlyList<ProductToReturnDto>>(Products);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageSize , Params.PageIndex , Count , MappedProducts));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> getProduct(int id)
        {
            var spec = new ProductWithTypeAndBrandSpecification(id);
            var Product = await productRepository.GetByIdAsyncWithSpec(spec);
            if (Product == null) return NotFound(new ApiResponse(404));
            var MappedProduct = mapper.Map<ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getBrands()
        {
            var Brands = await brandRepository.GetAllAsync();
            return Ok(Brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> getTypes()
        {
            var Types = await typeRepository.GetAllAsync();
            return Ok(Types);
        }
    }
}
