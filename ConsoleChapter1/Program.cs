// See https://aka.ms/new-console-template for more information

using ConsoleChapter1.Data;

Person student = new();
Person? manager = new() { Name = "Juan"};
WriteLine($"Hello, World! {student.CompleteName(manager)}");