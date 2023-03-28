using AutoMapper;
using FluentValidationApp.Web.Models;
using FluentValidations.Web.DTO;
using FluentValidations.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidations.Web.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreditCard, CustomerDto>();
            //Aynı property isimleri kullanırsak şu şekilde yaparız : 
            //CreateMap<Customer, CustomerDto>().ReverseMap();
            //CreateMap<CustomerDto, Customer>(); bu şekilde yazmak yerine ReverseMap kullandık.

            //Farklı kullanmak için : 
            CreateMap<Customer, CustomerDto>().IncludeMembers(x=>x.CreditCard)
                .ForMember(dest=>dest.Isim,opt=>opt.MapFrom(x=>x.Name))
                .ForMember(dest => dest.Eposta, opt => opt.MapFrom(x => x.Email))
                .ForMember(dest => dest.Yas, opt => opt.MapFrom(x => x.Age));
            //eğer isim GetFullName değil FullName2 olsaydı belirtmemiz gerekirdi.
            //.ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.FullName2));

            //IncludeMembers isimler aynı olursa -propertyler- kullanılır.
        }
    }
}
