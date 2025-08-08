using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.GlobalManagement;

public abstract class Values
{
    public int Timer { get; private set; }

    public List<CharData> CharsData { get; } = [new()]; // Adds 1st char by default
    public CharData MainCharData => CharsData.FirstOrDefault() ?? new(); // Returns the first char data or a new one if none exist

    protected Values()
    {
        Initialize();
    }

    private void Initialize() // Load from save file
    {
        CustomInitialize();
    }

    public void Update()
    {
        Timer++;
        CustomUpdate();
    }

    protected abstract void CustomInitialize();
    protected abstract void CustomUpdate();
}
