namespace Candle.GameSpecific.Entities.Enemies;

public class RatGray : Rat
{
    public RatGray()
    {
        AddSprite("RatGray", 22, 15, 13, 9);
        AddDamageTaker(3);
        AddMoveSpeed(1.5f);
    }
}