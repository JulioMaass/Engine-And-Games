using Engine.Managers.Graphics;
using Engine.Managers.StageEditing;
using Engine.Managers.StageEditing.Tools;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Data;
using System.Text;

namespace Engine.Managers;

public static class StringInput
{
    public static StringBuilder StringBuilder { get; } = new();
    private static string OriginalString { get; set; } // Just to show the original string when canceling with esc
    public static bool IsOn { get; set; }
    private static KeyboardState PreviousKeyboardState { get; set; }
    public static bool Finalized { get; private set; } // Finalizing means that the receiver of the string can update the value
    public static bool Overwrite { get; set; } = true; // If true, the string will be cleared when the input is activated

    private static void TurnOn()
    {
        StringBuilder.Clear();
        IsOn = true;
        Finalized = false;
        Overwrite = true;
    }

    private static void TurnOff()
    {
        IsOn = false;
        Finalized = false;
        Overwrite = false;
        RevertToOriginalString();
    }

    public static void CheckToTurnOff() // Automatically turn off in situations that it's not being used
    {
        // Using entity editor
        if (StageEditor.IsOn && StageEditor.CurrentMode == StageEditor.EntityMode && StageEditor.CurrentMode.CurrentTool.GetType() == typeof(StageEditorEditEntityTool))
            return;

        TurnOff();
    }

    private static void RevertToOriginalString()
    {
        StringBuilder.Clear();
        StringBuilder.Append(OriginalString);
    }

    public static void EditString(string @string)
    {
        TurnOn();
        OriginalString = @string;
        StringBuilder.Append(@string);
    }

    private static void FinalizeInput()
    {
        Finalized = true;
        IsOn = false;
        Overwrite = false;
    }

    public static int OutputAsInt()
    {
        Finalized = false;
        var currentString = StringBuilder.ToString();

        // Check if current string is a valid integer
        if (int.TryParse(currentString, out var newValidInt))
            return newValidInt;

        // Evaluate current string as a mathematical expression (can use + - * / %)
        try // If it gets any error from Compute (like trying to process a random word, this block is skipped)
        {
            var dataTable = new DataTable();
            var result = dataTable.Compute(currentString, "");
            if (result is double validDouble)
                return (int)Math.Round(validDouble, MidpointRounding.AwayFromZero);
            if (result is int validInt)
                return validInt;
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch { } // no need to run special code if it fails

        // If is not a valid number, revert to previous value
        RevertToOriginalString();
        if (int.TryParse(OriginalString, out var oldValidInt))
            return oldValidInt;
        return 0;
    }

    public static void Update()
    {
        if (!IsOn)
            return;

        var currentKeyboardState = Keyboard.GetState();
        foreach (var key in currentKeyboardState.GetPressedKeys())
        {
            if (!PreviousKeyboardState.IsKeyDown(key))
                HandleKeyPress(key);
        }
        PreviousKeyboardState = currentKeyboardState;
    }

    public static void HandleKeyPress(Keys key)
    {
        if (key == Keys.Enter)
            FinalizeInput();
        else if (key == Keys.Escape)
            TurnOff();
        else if (key == Keys.Back && StringBuilder.Length > 0)
        {
            Overwrite = false;
            StringBuilder.Length--;
        }
        else
        {
            CheckToTriggerOverwrite();
            var character = GetKeyChar(key);
            if (character != '\0')
                StringBuilder.Append(character);
        }
    }

    public static void CheckToTriggerOverwrite()
    {
        if (!Overwrite)
            return;

        StringBuilder.Clear();
        Overwrite = false;
    }

    public static void Draw(IntVector2 position, string preText)
    {
        var inputString = preText + ": " + StringBuilder;
        var color = IsOn ? Color.Yellow : Color.White;
        if (Overwrite)
            color = Color.Red;
        StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeFont, inputString, position, color);
    }

    private static char GetKeyChar(Keys key)
    {
        return key switch
        {
            Keys.A => 'a',
            Keys.B => 'b',
            Keys.C => 'c',
            Keys.D => 'd',
            Keys.E => 'e',
            Keys.F => 'f',
            Keys.G => 'g',
            Keys.H => 'h',
            Keys.I => 'i',
            Keys.J => 'j',
            Keys.K => 'k',
            Keys.L => 'l',
            Keys.M => 'm',
            Keys.N => 'n',
            Keys.O => 'o',
            Keys.P => 'p',
            Keys.Q => 'q',
            Keys.R => 'r',
            Keys.S => 's',
            Keys.T => 't',
            Keys.U => 'u',
            Keys.V => 'v',
            Keys.W => 'w',
            Keys.X => 'x',
            Keys.Y => 'y',
            Keys.Z => 'z',
            Keys.D0 => '0',
            Keys.D1 => '1',
            Keys.D2 => '2',
            Keys.D3 => '3',
            Keys.D4 => '4',
            Keys.D5 => '5',
            Keys.D6 => '6',
            Keys.D7 => '7',
            Keys.D8 => '8',
            Keys.D9 => '9',
            Keys.NumPad0 => '0',
            Keys.NumPad1 => '1',
            Keys.NumPad2 => '2',
            Keys.NumPad3 => '3',
            Keys.NumPad4 => '4',
            Keys.NumPad5 => '5',
            Keys.NumPad6 => '6',
            Keys.NumPad7 => '7',
            Keys.NumPad8 => '8',
            Keys.NumPad9 => '9',
            Keys.Space => ' ',
            Keys.Add => '+',
            Keys.Subtract => '-',
            Keys.Multiply => '*',
            Keys.Divide => '/',
            Keys.OemQuestion => '%',
            _ => '\0' // null character
        };
    }
}
