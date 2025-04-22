using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using e_commerce.Domain.DTOS;
using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.ViewModels.Home;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace e_commerce.Web.Mappers
{
 
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {

            CreateMap<ShopViewModel, ShopDTO>();
            CreateMap<ProductImage, ProductImageViewModel>();
            CreateMap<Review, ReviewViewMode>();
        
            CreateMap<SubCategory, SubCategoryViewModel>();
            CreateMap<Tag,TagViewModel>();

            CreateMap<Product, ProductViewModel>()

             .ForMember(dest => dest.SubCategoryName,
                        opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.Name : ""))
             .ForMember(dest => dest.Tag,
             opt => opt.MapFrom(src => src.TagObj.Name))
             .ForMember(dest => dest.CategoryName,
                        opt => opt.MapFrom(src => src.SubCategory != null && src.SubCategory.Category != null
                            ? src.SubCategory.Category.Name : ""))
             .ForMember(dest => dest.CategoryId,
                        opt => opt.MapFrom(src => src.SubCategory != null && src.SubCategory.Category != null ? src.SubCategory.CategoryId : 0))
             .ForMember(dest => dest.Images,
                        opt => opt.MapFrom(src => src.ProductImages ?? new List<ProductImage>())) // fix here
             .ForMember(dest => dest.Reviews,
                        opt => opt.MapFrom(src => src.Reviews ?? new List<Review>()))
           .ForMember(dest => dest.PrimaryImageIndex,
                opt => opt.MapFrom(src =>
                    src.ProductImages != null && src.ProductImages.Any(x => x.IsPrimary == true)
                        ? src.ProductImages
                            .Select((image, index) => new { image, index })
                            .Where(x => x.image.IsPrimary == true)
                            .Select(x => x.index)
                            .FirstOrDefault()
                        : -1
                ));





            CreateMap<Category, CategoryViewModel>()
                .ForMember(des => des.subCategory,
                    opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<ProductViewModel, Product>()
                 .ForMember(dest => dest.TagId,
             opt => opt.MapFrom(src => src.TagId));






















        }
    }

}
