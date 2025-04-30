using AutoMapper;
using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
    {
    public class ProductProfile : Profile
        {
        public ProductProfile()
            {
            CreateMap<Product, ProductResultDto>()
                .ForMember(bn => bn.BrandName, o => o.MapFrom(p => p.ProductBrand.Name))
                                .ForMember(tn => tn.TypeName, o => o.MapFrom(p => p.ProductType.Name))
                                .ForMember(pu => pu.PictureUrl, o => o.MapFrom<PictureUrlResolver>());


            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();

            }
        }
    }
