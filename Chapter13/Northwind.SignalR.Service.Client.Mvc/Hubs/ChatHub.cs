using Microsoft.AspNetCore.SignalR;
using Northwind.Common;

namespace Northwind.SignalR.Service.Client.Mvc.Hubs;

public class ChatHub : Hub
{
    private static Dictionary<string, UserModel> Users = new();

    public async Task Register(UserModel newUser)
    {
        UserModel user;
        string action = "registered as a new user";

        if (Users.ContainsKey(newUser.Name))
        {
            user = Users[newUser.Name];

            if (user.Groups is not null)
            {
                foreach (string group in user.Groups.Split(','))
                {
                    await Groups.RemoveFromGroupAsync(user.ConnectionId, group);
                }
            }

            user.Groups = newUser.Groups;
            user.ConnectionId = Context.ConnectionId;

            action = "updated your registered user";
        }
        else
        {
            if (string.IsNullOrEmpty(newUser.Name))
            {
                newUser.Name = Guid.NewGuid().ToString();
            }

            newUser.ConnectionId = Context.ConnectionId;
            Users.Add(key: newUser.Name, value: newUser);
            user = newUser;
        }

        if (user.Groups is not null)
        {
            foreach (string group in user.Groups.Split(','))
            {
                await Groups.AddToGroupAsync(user.ConnectionId, group);
            }
        }

        MessageModel message = new()
        {
            From = "SignalR Hub", To = user.Name,
            Body = string.Format(
                "You have successfully {0} with connection ID {1}.",
                arg0: action, arg1: user.ConnectionId)
        };

        IClientProxy proxy = Clients.Client(user.ConnectionId);
        await proxy.SendAsync("ReceiveMessage", message);
    }

    public async Task SendMessage(MessageModel message)
    {
        IClientProxy proxy;

        if (string.IsNullOrEmpty(message.To))
        {
            message.To = "Everyone";
            proxy = Clients.All;
            await proxy.SendAsync("ReceiveMessage", message);
        }
        
        // if To has a value, then split it into a list of user and group names
        string[] userAndGroupList = message.To.Split(',');

        foreach (string userOrGroup in userAndGroupList)
        {
            if (Users.ContainsKey(userOrGroup))
            {
                message.To = $"User: {Users[userOrGroup].Name}";
                proxy = Clients.Client(Users[userOrGroup].ConnectionId);
            }
            else
            {
                message.To = $"Group: {userOrGroup}";
                proxy = Clients.Group(userOrGroup);
            }

            await proxy.SendAsync("ReceiveMessage", message);
        }
    }
}