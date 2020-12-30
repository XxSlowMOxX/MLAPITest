using UnityEngine;

public class Player
{
    public Teams team;
    public ulong ClientID;
    public int Resources;
    public Player(Teams myTeams, ulong ID)
    {
        team = myTeams;
        ClientID = ID;
        Resources = 10;
        Debug.Log("Player Created with Team: " + team.ToString());
    }
}

public enum Teams
{
    red, blue
}