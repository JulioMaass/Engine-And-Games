using System;
using System.Collections.Generic;

namespace Engine.GameSpecific;

public class GameSpecificSettings
{
    // Game
    public GameId CurrentGame { get; protected set; }
    public string GameFolder { get; protected set; }

    // Types // TODO: These can all be automatically generated from the assembly probably
    public Type MainLoop { get; protected set; }
    public Type GlobalValues { get; protected set; }
    public Type InitialMenu { get; protected set; }
    public Type PauseMenu { get; protected set; }
    public List<Type> PlayerTypes { get; protected set; }
    public List<Type> EditorEntityTypes { get; protected set; } = new();
    public List<Type> TilesetTypes { get; protected set; } = new();
    public Type DefaultTilesetType { get; protected set; }

    // Stage files
    public List<string> StageFiles { get; protected set; } = new();

    public virtual void Initialize()
    {
    }
}

public enum GameId
{
    None,
    Mmdb,
    Mole,
    Candle,
    Shooter,
    SpaceMiner,
    CrtTest
}