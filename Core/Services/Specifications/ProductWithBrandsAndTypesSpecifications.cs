using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
    {
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>

        {
        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationsParameters productSpecsParams)
            : base(
                  p =>
                    (string.IsNullOrEmpty(productSpecsParams.Search) || p.Name.ToLower().Contains(productSpecsParams.Search.ToLower()))
                    &&
                    (!productSpecsParams.BrandId.HasValue || p.BrandId == productSpecsParams.BrandId)
                    &&
                    (!productSpecsParams.TypeId.HasValue || p.TypeId == productSpecsParams.TypeId)
                  )
            {
            ApplyIncludes();
            ApplySorting(productSpecsParams.Sort);
            ApplyPagination(productSpecsParams.PageIndex, productSpecsParams.PageSize);
            }


        public ProductWithBrandsAndTypesSpecifications(int id) : base(p => p.Id == id)
            {
            ApplyIncludes();
            }


        private void ApplyIncludes()
            {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            }

        public void ApplySorting(string? sort)
            {
            if ( !string.IsNullOrEmpty(sort) )
                {
                switch ( sort.ToLower() )
                    {
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    case "nameasc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "namedesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                    }
                }
            else
                {
                AddOrderBy(p => p.Name);
                }
            }

        }
    }
