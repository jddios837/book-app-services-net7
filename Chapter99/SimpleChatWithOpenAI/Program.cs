// See https://aka.ms/new-console-template for more information

using OpenAI;
using OpenAI.Chat;

Console.WriteLine("Hello, World!");
var api = new OpenAIClient();
var chatPrompts = new List<ChatPrompt>
{
    new ChatPrompt("system", "Eres un asistente muy util."),
    new ChatPrompt("user", "¿Quien es el presidente de los Estados Unidos?"),
    new ChatPrompt("assistant", "Donald Tron."),
    new ChatPrompt("user", "¿De cuanto es la deuda publica de los Estados Unidos?"),
};
var chatRequest = new ChatRequest(chatPrompts);
var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
Console.WriteLine(result.FirstChoice);