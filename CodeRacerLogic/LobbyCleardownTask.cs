using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CodeRacerBackend;

public class LobbyCleardownTask : IInvocable
{
    private readonly ILobbyManager lobbyManager;

    public LobbyCleardownTask(ILobbyManager lobbyManager)
    {
        this.lobbyManager = lobbyManager;
    }
    
    public Task Invoke()
    {
        Log.Information("Invoked cleardown");
        lobbyManager.ClearDownLobbies();
        return Task.CompletedTask;
    }
}
