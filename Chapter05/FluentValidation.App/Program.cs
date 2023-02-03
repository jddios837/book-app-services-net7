// See https://aka.ms/new-console-template for more information
using FluentValidation.Models;
using FluentValidation.Results;
using FluentValidation.Validators;

Order order = new() { };

OrderValidator validator = new();

ValidationResult result = validator.Validate(order);

Console.WriteLine("Hello, World!");