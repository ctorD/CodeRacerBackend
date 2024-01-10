namespace CodeRacerBackend.CodeRacerLogic;

public class Player
{
    public string ConnectionId { get; }
    private string DisplayName { get; set; }
    public bool Ready { get; set; }

    public string Name => DisplayName ?? ConnectionId;

    public Player(string connectionId)
    {
        ConnectionId = connectionId;
        Ready = false;
    }
    public Player(string connectionId, string displayName)
    {
        ConnectionId = connectionId;
        DisplayName = displayName;
        Ready = false;
    }

    public void ReadyUp()
    {
        Ready = true;
    }

    public void UpdateDisplayName(string name)
    {
        DisplayName = name;
    }
    
}