using Microsoft.AspNetCore.SignalR.Client;
namespace Nicepet_API
{
    public static class Notifier
    {
        public async static void NotifyDone(TaskItem task)
        {
            HubConnection connection;
            connection = new HubConnectionBuilder().WithUrl("https://localhost:44377/signalr").Build();

            await connection.StartAsync();

            task.Done = true;
            await connection.InvokeAsync("TaskDone", task);
        }
    }
}
