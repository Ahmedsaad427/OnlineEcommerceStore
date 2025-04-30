using AutoMapper;
using Domain.Models.OrderModels;
using Shared.OrderModelsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
    {
    public class OrderProfile : Profile
        {
        public OrderProfile()
            {
            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.paymentStatus, opt => opt.MapFrom(src => src.paymentStatus.ToString()))
            .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SubTotal + src.DeliveryMethod.Cost))
            ;

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl))
                ;

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
            }
        }
    }
