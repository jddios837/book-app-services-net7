using AutoMapper;
using MappingObjects.Mappers;

namespace MappingObjects.Tests;

public class TestAutoMapperConfig
{
    [Fact]
    public void TestSummaryMapping()
    {
        // Arrange
        MapperConfiguration config = CartToSummaryMapper.GetMapperConfiguration();
        
        // Act

        // Assert
        config.AssertConfigurationIsValid();
    }
}