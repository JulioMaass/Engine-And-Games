using Engine.Managers;
using Engine.Managers.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Main;

public class GameRoot : Game
{
    public GameRoot()
    {
        GameManager.Game = this;
        Video.CreateGraphicsDevice();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        Settings.Initialize();
        base.Initialize(); // This calls LoadContent()
        Window.TextInput += TextInputHandler.Update;
        GameManager.Initialize();
    }

    protected override void LoadContent()
    {
        GameManager.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        if (IsActive)
        {
            GameManager.CheckToUnpauseEngine();
            GameManager.Update();
            base.Update(gameTime);
        }
        else
            GameManager.CheckToPauseEngine();
        Camera.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        Video.Draw();
        base.Draw(gameTime);
    }
}