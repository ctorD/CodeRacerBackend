using System;
using System.Collections.Generic;
using CodeRacerBackend.CodeRacerLogic;

namespace CodeRacerBackend;

public class LobbyManager : ILobbyManager
{
    private List<Lobby> lobbies = new List<Lobby>();
    
    public void AddLobby(Lobby lobby){
        lobbies.Add(lobby);
    }

    public void RemoveLobby(Lobby lobby){
        lobbies.Remove(lobby);
    }
    
    public void ClearDownLobbies(){
        lobbies.RemoveAll(l => l.IsComplete() || l.IsExpired());
    }

    public List<Lobby> GetAllLobbies()
    {
        return lobbies;
    }

    public Lobby FindLobby(string id)
    {
        return lobbies.Find(l => l.LobbyId == id);
    }
}
