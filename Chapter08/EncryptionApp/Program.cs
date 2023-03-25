// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Packt.Shared;

WriteLine("Write a message that you want encrypt: ");
string? message = ReadLine();

WriteLine("Write a password: ");
string? password = ReadLine();

if((password is null) || (message is null))
{
    WriteLine("Message or password cannot be null.");
    return;
}

string cypherText = Protector.Encrypt(message, password);

WriteLine($"Encrypted text: {cypherText}");

Write("Enter the password to decrypt: ");
string? password2Decrypt = ReadLine();

if(password2Decrypt is null)
{
    WriteLine("Password to decrypt cannot be null.");
    return;
}

try
{
    string plainText = Protector.Decrypt(cypherText, password2Decrypt);
    WriteLine($"Decrypted text: {plainText}");
}
catch (CryptographicException)
{
    WriteLine("The password is incorrect.");
}
catch (Exception ex)
{
    WriteLine("Non-cryptographic exception: {0}, {1}", ex.GetType().Name, ex.Message);
}
