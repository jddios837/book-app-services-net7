using Microsoft.CodeAnalysis;

namespace Packt.Shared;

[Generator]
public class MessageSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // not needed initialization
    }

    public void Execute(GeneratorExecutionContext execContext)
    {
        IMethodSymbol mainMethod = execContext.Compilation.GetEntryPoint(execContext.CancellationToken);

        string sourceCode = $@"// source-generated code
            static partial class {mainMethod.ContainingType.Name}
            {{
                static partial void Message(string message)
                {{
                    System.Console.WriteLine($""Generator says: '{{message}}'"");
                }}
            }}
        ";
        string typeName = mainMethod.ContainingType.Name;
        execContext.AddSource($"{typeName}.Methods.g.cs", sourceCode);

    }
    
}

