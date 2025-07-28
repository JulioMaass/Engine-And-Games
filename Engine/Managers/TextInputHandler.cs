using Microsoft.Xna.Framework;

namespace Engine.Managers;

public static class TextInputHandler
{
    private static string TextInput { get; set; } = string.Empty;

    public static void ResetTextInput()
    {
        TextInput = string.Empty;
    }

    public static void Update(object sender, TextInputEventArgs args)
    {
        var pressedKey = args.Key;
        var character = args.Character;
        TextInput += character;
    }
}
