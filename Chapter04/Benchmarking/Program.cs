﻿// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Benchmarking;

//Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<StringNullValidationBenchmark>();
