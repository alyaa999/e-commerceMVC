using AutoMapper;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.ViewModels;
namespace e_commerce.Web.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressVM>();
            CreateMap<AddressVM, Address>();
        }
    }
}
