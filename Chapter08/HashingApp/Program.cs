// See https://aka.ms/new-console-template for more information
using Packt.Shared;

WriteLine("Registering Alice with Pa$$w0rd");
User alice = Protector.Register("Alice", "Pa$$w0rd");

WriteLine($"   Name: {alice.Name}");
WriteLine($"   Salt: {alice.Salt}");
WriteLine($"   Password (salted and hashed): {alice.SaltedHashedPassword}");
WriteLine();

WriteLine($"Enter a new user to register: ");
string? username = ReadLine();
if (string.IsNullOrEmpty(username))
{
    username = "Bob";
}

WriteLine($"Enter a password for {username}: ");
string? password = ReadLine();
if (string.IsNullOrEmpty(password))
{
    password = "Pa$$w0rd";
}

WriteLine("Registering a new user:");
User newUser = Protector.Register(username, password);

WriteLine($"   Name: {newUser.Name}");
WriteLine($"   Salt: {newUser.Salt}");
WriteLine($"   Password (salted and hashed): {newUser.SaltedHashedPassword}");
WriteLine();

bool correctPassword = false;

while (!correctPassword)
{
    WriteLine("Enter a username to log in: ");
    string? loginUserName = ReadLine();
    if (string.IsNullOrEmpty(loginUserName))
    {
        WriteLine("Login username cannot be empty.");
        Write("Press Ctrl + C to end or press ENTER to retry.");
        ReadLine();
        continue;
    }
    
    WriteLine("Enter a password to log in: ");
    string? loginPassword = ReadLine();
    if (string.IsNullOrEmpty(loginPassword))
    {
        WriteLine("Login password cannot be empty.");
        Write("Press Ctrl + C to end or press ENTER to retry.");
        ReadLine();
        continue;
    }

    correctPassword = Protector.CheckPassword(loginUserName, loginPassword);

    if (correctPassword)
    {
        WriteLine($"Correct! {loginUserName} has been logged in.");
    }
    else
    {
        WriteLine("Invalid username or password. Try again.");
    }
}

