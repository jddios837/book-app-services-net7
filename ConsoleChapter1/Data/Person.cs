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
        return text;
    }
}

