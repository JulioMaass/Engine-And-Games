namespace SpaceMiner.GameSpecific.Managers;

public class LevelData(int smallSpawns, int bigSpawns = 0, int smallSpawns2 = 0, int bigSpawns2 = 0)
{
    public int SmallSpawns { get; set; } = smallSpawns;
    public int BigSpawns { get; set; } = bigSpawns;
    public int SmallSpawns2 { get; set; } = smallSpawns2;
    public int BigSpawns2 { get; set; } = bigSpawns2;

    public int TotalSpawns => SmallSpawns + BigSpawns + SmallSpawns2 + BigSpawns2;
}
