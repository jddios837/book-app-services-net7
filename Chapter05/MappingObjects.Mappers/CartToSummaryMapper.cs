using AutoMapper; // MapperConfiguration
using AutoMapper.Internal; // Internal() extension method
using Packt.Entities; // Cart
using Packt.ViewModels; // Summary


namespace MappingObjects.Mappers;

public static class CartToSummaryMapper
{
  public static MapperConfiguration GetMapperConfiguration()
  {
      MapperConfiguration config = new(cfg =>
      {
          cfg.Internal().MethodMappingEnabled = false;

          // configure mapping using projection
          cfg.CreateMap<Cart, Summary>()
              // FullName
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                  string.Format("{0} {1}", src.Customer.FirstName, src.Customer.LastName)))
              // Total
              .ForMember(dest => dest. Total, opt => opt.MapFrom(src => 
                  src.Items.Sum(item => item.UnitPrice * item.Quantity)));
      });
      
      return config;
  }
}