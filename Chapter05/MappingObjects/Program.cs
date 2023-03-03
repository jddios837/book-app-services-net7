// See https://aka.ms/new-console-template for more information
using AutoMapper;
using MappingObjects.Mappers;
using Packt.Entities;
using Packt.ViewModels;

Cart cart = new(
    Customer: new(FirstName: "John", LastName: "Smith"),
    Items: new()
    {
        new(ProductName: "Apples", UnitPrice: 0.49M, Quantity: 10),
        new(ProductName: "Bananas", UnitPrice: 0.49M, Quantity: 10)
    }
);

WriteLine($"{cart.Customer}");
foreach (LineItem e in cart.Items)
{
    WriteLine($"   {e}");
}

MapperConfiguration config = CartToSummaryMapper.GetMapperConfiguration();

IMapper mapper = config.CreateMapper();

Summary summary = mapper.Map<Cart, Summary>(cart);

WriteLine($"Summary: {summary.FullName} spent {summary.Total}.");