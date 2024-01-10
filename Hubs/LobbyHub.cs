using System;
using System.Collections.Generic;

namespace CodeRacerBackend.Hubs
{
    using CodeRacerBackend.CodeRacerLogic;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    namespace SignalRChat.Hubs
    {
        public class LobbyHub : Hub
        {
            public static readonly List<Lobby> Lobbies = new List<Lobby>();
            private static readonly Dictionary<string, string> NameByConnectionId = new Dictionary<string, string>();

            public override Task OnConnectedAsync()
            {
                //TODO: Add username to cookie instead of setting each time
                var username = Context.GetHttpContext().Request.Query["username"];

                return base.OnConnectedAsync();
            }

            public async Task CreateLobby(string user, string lobbyName, string lang, bool online)
            {
                var lobby = new Lobby(lang, user, lobbyName, online);

                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);

                Log.Information("Created Lobby | User {UserId} | Lobby Name: {LobbyName} | Language: {Lang} | Online : {Online}", user, lobbyName, lang, online);

                Lobbies.Add(lobby);
            }

            public async void JoinLobby(string lobbyId)
            {
                var lobby = Lobbies.Find(e => e.LobbyId == lobbyId);
                NameByConnectionId.TryGetValue(Context.ConnectionId, out string name);
                var player = name != null ? new Player(Context.ConnectionId, name) : new Player(Context.ConnectionId);
                if (!lobby.HasPlayer(player))
                {
                    lobby.Join(player);
                    await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
                    await Clients.Group(lobbyId).SendAsync("UpdateUserList", lobby.Players);

                    Log.Information("User: {UserId} joined {LobbyId}", Context.ConnectionId, lobbyId);
                }

                Console.WriteLine(Groups.ToString());
            }

            public async void LeaveLobby(string lobbyName, string userName)
            {
                await Task.Run(() =>
                {
                    Lobbies.Find(e => e.LobbyName == lobbyName).Leave(Context.ConnectionId);
                });
            }

            public override async Task OnDisconnectedAsync(Exception exception)
            {
                Console.WriteLine("Disconnection");

                NameByConnectionId.Remove(Context.ConnectionId);
                Lobbies.RemoveAll(lobby => lobby.Host == Context.ConnectionId);
                Lobbies.ForEach(lobby =>
                {
                    var player = lobby.Players.Find(p => p.ConnectionId == Context.ConnectionId);
                    if (player != null)
                    {
                        lobby.Players.Remove(player);
                    }
                });

                await base.OnDisconnectedAsync(exception);
            }

            public async Task ReadyUp(string lobbyId)
            {
                var lobby = Lobbies.Find(e => e.LobbyId == lobbyId);
                await Task.Run(() => lobby.VoteStart(Context.ConnectionId));
                await Clients.Group(lobbyId).SendAsync("UpdateUserList", lobby.Players);
                if (lobby.Players.FindAll(p => p.Ready == true).Count == lobby.Players.Count)
                {
                    await Clients.Group(lobbyId).SendAsync("StartGame");
                    // await Task.Run(() => lobby.Start());
                    // await Task.Run(() => lobby.Start());
                }
            }
            public void StartServerTimer(string lobbyId)
            {
                var lobby = Lobbies.Find(e => e.LobbyId == lobbyId);
                Log.Information("Starting server time for {Lobby}", lobbyId);
                lobby.StartTimer();
            }
            

            public async Task UserComplete(string lobbyId, string time)
            {
                var lobby = Lobbies.Find(e => e.LobbyId == lobbyId);
                await Task.Run(() => lobby.PlayerComplete(Context.ConnectionId, time));
                await Clients.Group(lobbyId).SendAsync("UpdateScoreboard", lobby.Scores);
            }

            public Task SetDisplayName(string name)
            {
                Log.Information("Setting name for {Conn} to {Name}", Context.ConnectionId, name);
                NameByConnectionId[Context.ConnectionId] = name;
                Lobbies.ForEach(lobby =>
                {
                    var player = lobby.Players.Find(p => p.ConnectionId == Context.ConnectionId);
                    player?.UpdateDisplayName(name);
                });
                return Task.CompletedTask;
            }


            public async Task RemoveFromGroup(string lobbyName)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyName);

                await Clients.Group(lobbyName).SendAsync("Send", $"{Context.ConnectionId} has left the group {lobbyName}.");
            }
        }
    }
}