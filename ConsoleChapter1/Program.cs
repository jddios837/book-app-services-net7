// See https://aka.ms/new-console-template for more information

using ConsoleChapter1.Data;

// Is possible use only "new()" instead of "new Class()"
Person student = new();
Person? manager = new() { Name = "Juan"};
WriteLine($"Hello, World! {student.CompleteName(manager)}");