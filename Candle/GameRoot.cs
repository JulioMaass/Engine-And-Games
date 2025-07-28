using Engine.Main;
using Microsoft.Xna.Framework;

namespace Candle;

public class GameRoot : Engine.Main.GameRoot
{
    protected override void Initialize()
    {
        GameManager.GameSpecificSettings = new GameSpecific.GameSpecificSettings();
        GameManager.GameSpecificSettings.Initialize();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}