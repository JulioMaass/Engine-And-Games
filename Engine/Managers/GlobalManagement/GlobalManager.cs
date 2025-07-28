using Engine.GameSpecific;
using Engine.Main;
using System;

namespace Engine.Managers.GlobalManagement;

public static class GlobalManager
{
    public static Values Values { get; private set; }

    public static void Initialize()
    {
        Values = (Values)Activator.CreateInstance(GameManager.GameSpecificSettings.GlobalValues);
    }

    public static void Update()
    {
        Values.Update();
    }
}
