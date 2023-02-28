namespace MappingObjects.Models;

public record class Cart(
    Customer Customer,
    List<LineItem> Items);