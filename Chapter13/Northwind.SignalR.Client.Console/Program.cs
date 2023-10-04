using Microsoft.AspNetCore.SignalR.Client;
using Northwind.Common;

Write("Enter a username (required): ");
string? username = ReadLine();

if (string.IsNullOrWhiteSpace(username))
{
    WriteLine("Username is required.");
    return;
}

Write("Enter your group name (optional): ");
string? groupName = ReadLine();

HubConnection connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5132/chat")
    .Build();
    
connection.On<MessageModel>("ReceiveMessage", message =>
{
    WriteLine($"To {message.To}, From {message.From}: {message.Body}");
});

await connection.StartAsync();

WriteLine("Successfully started.");

UserModel user = new UserModel
{
    Name = username,
    Groups = groupName
};

await connection.InvokeAsync("Register", user);

WriteLine("Successfully registered.");
WriteLine("Listening for messages. Press any key to exit.");
ReadLine();