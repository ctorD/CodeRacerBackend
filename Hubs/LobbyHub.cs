using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeRacerBackend.Hubs
{
    using CodeRacerBackend.CodeRacerLogic;
    using Microsoft.AspNetCore.SignalR;
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

                lobbies.Add(lobby);


            }
            public void JoinLobby(string lobbyId)
            {
                var lobby = lobbies.Find(e => e.LobbyId == lobbyId);
                
                lobby.Join(Context.ConnectionId);

                Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);

            }

            public void LeaveLobby(string lobbyName, string userName)
            {
                lobbies.Find(e => e.LobbyName == lobbyName).Leave(Context.ConnectionId);
            }

            public void StartGame(string lobbyId)
            {
                lobbies.Find(e => e.LobbyId == lobbyId).Leave(Context.ConnectionId);
            }

            





            public async Task RemoveFromGroup(string lobbyName)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyName);

                await Clients.Group(lobbyName).SendAsync("Send", $"{Context.ConnectionId} has left the group {lobbyName}.");
            }


        }
    }
}
