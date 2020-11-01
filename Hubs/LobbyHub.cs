using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeRacerBackend.Hubs
{
    using CodeRacerBackend.CodeRacerLogic;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System.Threading.Tasks;

    namespace SignalRChat.Hubs
    {
        public class LobbyHub : Hub
        {
            public static List<Lobby> lobbies = new List<Lobby>();



            List<string> users = new List<string>();


            public override Task OnConnectedAsync()
            {
                var username = Context.GetHttpContext().Request.Query["username"];

                return base.OnConnectedAsync();
            }

            public async Task CreateLobby(string user, string lobbyName, string lang, bool online)
            {
                var lobby = new Lobby(lang, user, lobbyName, online);

                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);

                Log.Information("Created Lobby | User {userId} | Lobby Name: {lobbyName} | Language: {lang} | Online : {online}", user, lobbyName, lang, online);

                lobbies.Add(lobby);


            }
            public async void JoinLobby(string lobbyId)
            {
                var lobby = lobbies.Find(e => e.LobbyId == lobbyId);

                lobby.Join(Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);

                Log.Information("User: {userId} joined {lobbyId}", Context.ConnectionId, lobbyId);

                Console.WriteLine(Groups.ToString());

            }

            public async void LeaveLobby(string lobbyName, string userName)
            {
                await Task.Run(() =>
                {
                    lobbies.Find(e => e.LobbyName == lobbyName).Leave(Context.ConnectionId);
                });
            }


            public override async Task OnDisconnectedAsync(Exception exception)
            {
                Console.WriteLine("Disconnection");

                lobbies.RemoveAll(lobby => lobby.Host == Context.ConnectionId);

                lobbies.ForEach(lobby =>
                {
                    lobby.Players.Remove(Context.ConnectionId);
                });

                await base.OnDisconnectedAsync(exception);

            }



            public async Task StartGame(string lobbyId)
            {
                var lobby = lobbies.Find(e => e.LobbyId == lobbyId);
                await Task.Run(() => lobby.Start());

            }



            public async Task RemoveFromGroup(string lobbyName)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyName);

                await Clients.Group(lobbyName).SendAsync("Send", $"{Context.ConnectionId} has left the group {lobbyName}.");
            }


        }
    }
}
