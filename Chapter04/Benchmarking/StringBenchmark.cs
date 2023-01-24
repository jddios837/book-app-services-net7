using BenchmarkDotNet.Attributes;

namespace Benchmarking;

public class StringBenchmark
{
    private int[] _numbers;

    public StringBenchmark()
    {
        _numbers = Enumerable.Range(start: 1, count: 20).ToArray();
    }

    [Benchmark(Baseline = true)]
    public string StringConcatenationTest()
    {
        string s = string.Empty;

        for (int i = 0; i < _numbers.Length; i++)
        {
            s += _numbers[i] + ", ";
        }

        return s;
    }

    [Benchmark]
    public string StringBuilderTest()
    {
        System.Text.StringBuilder builder = new();

        for (int i = 0; i < _numbers.Length; i++)
        {
            builder.Append(_numbers[i]);
            builder.Append(", ");
        }

        return builder.ToString();
    }
}