using CodeRacerBackend.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CodeRacerBackend.CodeRacerLogic
{
  public class Lobby
  {
    public string LobbyId { get; set; }

    public string LobbyName { get; set; }

    public string Host { get; set; }
    public int MaxPlayers { get; set; }
    public string Snippet { get; set; }
    public List<String> Players { get; set; }

    public List<Tuple<String, TimeSpan>> Scores = new List<Tuple<string, TimeSpan>>();
    private Stopwatch stopWatch = new Stopwatch();

    public string GenerateLobbyId(string user)
    {
      return string.Format("{0}_{1:N}", user, Guid.NewGuid());
    }

    public Lobby(string lang, string user, string lobbyName, bool online)
    {
      LobbyName = lobbyName;
      Host = user;
      this.MaxPlayers = online ? 4 : 1;
      this.LobbyId = GenerateLobbyId(user);
      SnippetFinder sf = new SnippetFinder();
      this.Snippet = sf.getSnippet(lang);
      this.Players = new List<string>();
    }

    public Boolean Join(string connectionId)
    {
      if (!Players.Contains(connectionId) && Players.Count != MaxPlayers)
      {
        Players.Add(connectionId);
        return true;
      }
      else
      {
        return false;
      }
    }

    public void Leave(string connectionId)
    {
      Players.Remove(connectionId);
    }

    public void Start()
    {
      stopWatch.Start();
      if (stopWatch.IsRunning)
      {
        Console.WriteLine(stopWatch.Elapsed.TotalMilliseconds);
      }
      Console.WriteLine("Timer Starting for" + LobbyId);
    }

    public void GameFinished(string connectionId, string time)
    {
    }

    public void PlayerComplete(string user, string time)
    {
      // server Time, use to compare.
      // Scores.Add(new Tuple<String, TimeSpan>(user + "stopwatch", stopWatch.Elapsed));
      if (Scores.Count != MaxPlayers)
      {
        Scores.Add(new Tuple<String, TimeSpan>(user + "timespan", TimeSpan.Parse(time)));
      }
    }
  }
}