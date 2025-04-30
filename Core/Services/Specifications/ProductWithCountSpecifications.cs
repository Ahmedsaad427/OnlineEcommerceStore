using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
    {
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
        {
        public ProductWithCountSpecifications(ProductSpecificationsParameters productSpecsParams)
            : base(
                    p =>
                    (string.IsNullOrEmpty(productSpecsParams.Search) || p.Name.ToLower().Contains(productSpecsParams.Search.ToLower()))
                    &&
                    (!productSpecsParams.BrandId.HasValue || p.BrandId == productSpecsParams.BrandId)
                    &&
                    (!productSpecsParams.TypeId.HasValue || p.TypeId == productSpecsParams.TypeId)
                  )
            {

            }
        }
    }
