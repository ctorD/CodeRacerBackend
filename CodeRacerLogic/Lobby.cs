using CodeRacerBackend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CodeRacerBackend.CodeRacerLogic
{
    public class Lobby
    {
        public string LobbyId { get; set; }

        public string LobbyName { get; set; }

        public string Host { get; set; }
        public int MaxPlayers { get; set; }
        public string Snippet { get; set; }
        public Dictionary<String, String> Players { get; set; }

        public String GenerateLobbyId(string user)
        { 

            return DateTime.Now.ToString().GetHashCode().ToString(user);
            
        }

        public Lobby(string lang, string user, string lobbyName)
        {
            LobbyName = lobbyName;
            Host = user;
            this.MaxPlayers = 4;
            this.LobbyId = GenerateLobbyId(user);
            SnippetFinder sf = new SnippetFinder();
            this.Snippet = sf.getSnippet(lang);
        }

        public void Join(string user, string connectionId)
        {
            Players.Add(connectionId, user);
        }

        public void Leave(string connectionId)
        {
            Players.Remove(connectionId);
        }

        public void Start()
        {

        }

     



    }
}
