namespace Engine.Managers.GameModes;

public abstract class GameLoop
{
    public virtual void GameSpecificSetup() { }
    public abstract void Update();
}
