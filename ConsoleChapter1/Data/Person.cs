namespace ConsoleChapter1.Data;

/// <summary>
/// Person model
/// </summary>
public class Person
{
    /// <summary>
    /// Value for Name
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Example to use raw string literals
    /// </summary>
    public string Xml = """
            <person age="50">
                <first_name>Mark</first_name>
            </person>
            """;

    
    
    
    /// <summary>
    /// Return a complete name
    /// </summary>
    /// <param name="manager">Manager type person</param>
    public string CompleteName(Person? manager)
    {
        ArgumentNullException.ThrowIfNull(manager);
        string text = "";

        text = manager.Name switch
        {
            "Juan" => text += "Hola Juan",
            null => text += "Is null",
            _ => ""
        };

        // return $"Complete Name {this.Name}";
        // Number $ represent number of {} to use
        string Json = $$$"""
                    {
                        "firstName": "{{{text}}}",
                        
                    }
                    """;
        
        return text;
    }
}

