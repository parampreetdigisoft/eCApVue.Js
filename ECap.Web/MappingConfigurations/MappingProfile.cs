using AutoMapper;
using ECap.Core.Entities;
using ECap.DTO;
using ECap.Web.Models;

namespace ECap.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ManageUserDTO, ManageUserViewModel>();
            CreateMap<ProductDTO, Product>();
            CreateMap<AddonDTO, AddOn>();
            CreateMap<UserInfoDTO, UserInfoViewModel>();
        }
    }
}
