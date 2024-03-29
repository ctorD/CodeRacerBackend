﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeRacerBackend.CodeRacerLogic;
using CodeRacerBackend.Utils;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace CodeRacerBackend.Hubs
{
    public class LobbyHub : Hub
    {
        private static readonly Dictionary<string, string> NameByConnectionId = new();
        private readonly ISnippetFinder _snippetFinder;
        private readonly ILobbyManager _lobbyManager;

        public LobbyHub(ISnippetFinder snippetFinder, ILobbyManager lobbyManager)
        {
            _snippetFinder = snippetFinder;
            _lobbyManager = lobbyManager;
        }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        // private void RemoveExpired()
        // {
        //     var checkDatesTask = new Task(
        //         () =>
        //         {
        //             while (!_cancellationTokenSource.IsCancellationRequested)
        //             {
        //                 //TODO: check and delete elements here
        //                 Lobbies.RemoveAll(lobby => lobby.IsComplete());

        //                 _cancellationTokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromMinutes(60));
        //             }
        //         },
        //         _cancellationTokenSource.Token,
        //         TaskCreationOptions.LongRunning);
        //     checkDatesTask.Start();
        // }

        public override Task OnConnectedAsync()
        {
            //TODO: Add username to cookie instead of setting each time
            var username = Context.GetHttpContext().Request.Query["access_token"]
                .FirstOrDefault(Context.ConnectionId);
            if (username != null) SetDisplayName(username);

            return base.OnConnectedAsync();
        }

        public async Task CreateLobby(string lobbyName, string lang, bool online)
        {
            var hasUser = NameByConnectionId.TryGetValue(Context.ConnectionId, out var username);
            var lobby = new Lobby(_snippetFinder);
            lobby.Initalise(lang, hasUser ? username : Context.ConnectionId, lobbyName, online);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);

            Log.Information(
                "Created Lobby | User {Username} | Lobby Name: {LobbyName} | Language: {Lang} | Online : {Online}",
                hasUser ? username : Context.ConnectionId, lobbyName, lang, online);

            _lobbyManager.AddLobby(lobby);
        }

        public async void JoinLobby(string lobbyId)
        {
            var lobby = _lobbyManager.FindLobby(lobbyId);
            NameByConnectionId.TryGetValue(Context.ConnectionId, out var name);
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
            await Task.Run(() => { _lobbyManager.FindLobby(lobbyName).Leave(Context.ConnectionId); });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Disconnection");

            NameByConnectionId.Remove(Context.ConnectionId);
            // Lobbies.RemoveAll(lobby => lobby.Host == Context.ConnectionId);
            _lobbyManager.GetAllLobbies().ForEach(lobby =>
            {
                var player = lobby.Players.Find(p => p.ConnectionId == Context.ConnectionId);
                if (player != null) lobby.Players.Remove(player);
            });

            await base.OnDisconnectedAsync(exception);
        }

        public async Task ReadyUp(string lobbyId)
        {
            var lobby = _lobbyManager.FindLobby(lobbyId);
            await Task.Run(() => lobby.VoteStart(Context.ConnectionId));
            await Clients.Group(lobbyId).SendAsync("UpdateUserList", lobby.Players);
            if (lobby.Players.FindAll(p => p.Ready).Count == lobby.Players.Count)
                await Clients.Group(lobbyId).SendAsync("StartGame");
        }

        public void StartServerTimer(string lobbyId)
        {
            var lobby = _lobbyManager.FindLobby(lobbyId);
            Log.Information("Starting server time for {Lobby}", lobbyId);
            lobby.StartTimer();
        }


        public async Task UserComplete(string lobbyId, string time)
        {
            var lobby = _lobbyManager.FindLobby(lobbyId);

            await Task.Run(() => lobby.PlayerComplete(Context.ConnectionId, time));
            await UpdateLobbyList(lobby);
            await Clients.Group(lobbyId).SendAsync("UpdateScoreboard", lobby.Scores);
        }

        private Task UpdateLobbyList(Lobby lobby)
        {
            if (lobby.IsComplete()) _lobbyManager.RemoveLobby(lobby);

            return Task.CompletedTask;
        }

        public Task SetDisplayName(string name)
        {
            Log.Information("Setting name for {Conn} to {Name}", Context.ConnectionId, name);
            NameByConnectionId[Context.ConnectionId] = name;
            _lobbyManager.GetAllLobbies().ForEach(lobby =>
            {
                var player = lobby.Players.Find(p => p.ConnectionId == Context.ConnectionId);
                player?.UpdateDisplayName(name);
            });
            return Task.CompletedTask;
        }


        public async Task RemoveFromGroup(string lobbyName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyName);

            await Clients.Group(lobbyName)
                .SendAsync("Send", $"{Context.ConnectionId} has left the group {lobbyName}.");
        }
    }
}