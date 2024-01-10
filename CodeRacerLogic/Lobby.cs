using CodeRacerBackend.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeRacerBackend.CodeRacerLogic
{
  public class Lobby
  {
    public string LobbyId { get; set; }
    public string LobbyName { get; set; }
    public string Host { get; set; }
    public int MaxPlayers { get; set; }
    public string Snippet { get; set; }
    public List<Player> Players { get; set; }

    public readonly List<Tuple<string, TimeSpan>> Scores = new List<Tuple<string, TimeSpan>>();
    private readonly Stopwatch _stopWatch = new Stopwatch();

    private string GenerateLobbyId(string user)
    {
      return $"{user}_{Guid.NewGuid():N}";
    }

    public Lobby(string lang, string user, string lobbyName, bool online)
    {
      LobbyName = lobbyName;
      Host = user;
      this.MaxPlayers = online ? 4 : 1;
      this.LobbyId = GenerateLobbyId(user);
      SnippetFinder sf = new SnippetFinder();
      this.Snippet = sf.GetSnippet(lang);
      //this.Snippet = "test";
      this.Players = new List<Player>();
    }

    public void Join(Player player)
    {
      Players.Add(player);
    }

    public bool HasPlayer(Player player)
    {
      return Players.FindIndex(p => p.ConnectionId == player.ConnectionId) != -1;
    }

    public Boolean Join(string connectionId)
    {
      
      if (Players.FindIndex(p => p.ConnectionId == connectionId) == -1 && Players.Count != MaxPlayers)
      {
        Players.Add(new Player(connectionId));
        return true;
      }
      else
      {
        return false;
      }
    }

    public void Leave(string connectionId)
    {
      var player = Players.Find(player => player.ConnectionId == connectionId);
      Players.Remove(player);
    }

    public void StartTimer()
    {
      _stopWatch.Restart();
      if (_stopWatch.IsRunning)
      {
        Console.WriteLine(_stopWatch.Elapsed.TotalMilliseconds);
      }
      Console.WriteLine("Timer Starting for" + LobbyId);
    }

    public void VoteStart(string connectionId)
    {
      var player = Players.Find(p => p.ConnectionId == connectionId);
      if (player == null) return;
      player.Ready = true;
    }

    private static TimeSpan GetValidScore(TimeSpan clientTime, TimeSpan serverTime)
    {
      var lowerLimit = serverTime.Seconds - 2;
      return clientTime.TotalSeconds < lowerLimit ? serverTime : clientTime;
    }
    public void PlayerComplete(string user, string time)
    {
      // server Time, use to compare.
      var clientTime = TimeSpan.Parse(time);
      var serverTime = _stopWatch.Elapsed;
      var player = Players.Find(pl => pl.ConnectionId == user);
      if (Scores.Count != MaxPlayers)
      {
        Scores.Add(new Tuple<string, TimeSpan>(player.Name, GetValidScore(clientTime, serverTime)));
      }
    }
  }
}