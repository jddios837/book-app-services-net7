using BenchmarkDotNet.Attributes;

namespace Benchmarking;

public class StringNullValidationBenchmark
{
    private string testText = "";

    public StringNullValidationBenchmark()
    {
        
    }
    
    [Benchmark(Baseline = true)]
    public bool StringIsNullOrEmptyTest()
    {
        return string.IsNullOrEmpty(testText);
    }
    
    [Benchmark]
    public bool StringIsNullOrWhiteSpace()
    {
        return string.IsNullOrWhiteSpace(testText);
    }
    
    [Benchmark]
    public bool StringTrimIsNullOrWhiteSpace()
    {
        return string.IsNullOrEmpty(testText.Trim());
    }
}