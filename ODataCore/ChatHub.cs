using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Nicepet_API
{
    public class ChatHub : Hub
    {
        //-------------------------Connection----------------------
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        //----------------------------Group---------------------------
        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        } 
        public Task OutOfGroup(string group)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
        public Task SendMessageToGroup(string group, string user, string message)
        {
            return Clients.Group(group).SendAsync("ReceiveMessage", user , message);
        }
        //--------------------------All--------------------------------
        public async Task SendMessage(string userId, string userName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage"+userId.ToString(), userName, message);
        }
        public async Task SendMessageToCaller(string msg)
        {
            //eturn Clients.Caller.ReceiveMessage(msg);
            await Clients.Caller.SendAsync("ReceiveMessage", msg);
        }

        public async Task SendMessageToPartner(string user, string msg)
        {
            //return Clients.Client(user).ReceiveMessageToPartner(msg);
            await Clients.Client(user).SendAsync("ReceiveMessageToPartner", msg);
        }
        //---------------------auther---------------
        [HubMethodName("SendMessageToUser")]
        public Task DirectMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }
        [HubMethodName("SendMessageToUserById")]
        public Task DirectMessageToUser(string user, string userId, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage" + userId, user, message);
        }
        //public Task SendMessageToUser(string connectionId, string message)
        //{
        //    //return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        //    return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        //}

        //-----------Notification-----------------------
        //private static ConcurrentBag<TaskItem> _tasks = new ConcurrentBag<TaskItem>();

        ////Going to be called from the client side (Javascript Vue)
        //public async Task AddTask(object taskItem)
        //{
        //    TaskItem item = JsonConvert.DeserializeObject<TaskItem>(((JsonElement)taskItem).ToString());

        //    _tasks.Add(item);

        //    #pragma warning disable CS4014
        //    Task.Factory.StartNew(DoTasks);
        //    //Kinda like a callback.
        //    await Clients.All.SendAsync("AddedTask",taskItem);
        //}

        //public async Task TaskDone(object taskItem) 
        //{
        //    await Clients.All.SendAsync("TaskIsDone", taskItem);
        //}

        //private void DoTasks() 
        //{
        //    _tasks.ToList().ForEach(x =>
        //    {
        //        Thread.Sleep(1000 * RandomNumber(1, 10));
        //        Nicepet_API.Notifier.NotifyDone(x);
        //    });
        //}

        //public int RandomNumber(int min, int max) 
        //{
        //    Random random = new Random();

        //    return random.Next(min, max);
        //}
    }

}
