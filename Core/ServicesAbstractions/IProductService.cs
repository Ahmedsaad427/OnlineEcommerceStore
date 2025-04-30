using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
    {
    public interface IProductService
        {
        //Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? brandId, int? typeId, string? sort, int pageIndex = 1, int pageSize = 10);
        Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters productSpecsParams);
        Task<ProductResultDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        }
    }
