using System;
using System.Collections.Generic;
using CodeRacerBackend.CodeRacerLogic;

namespace CodeRacerBackend;

public interface ILobbyManager
{
    public List<Lobby> GetAllLobbies();
    public void AddLobby(Lobby lobby);
    public Lobby FindLobby(string id);
    public void RemoveLobby(Lobby lobby);
    public void ClearDownLobbies();

}
