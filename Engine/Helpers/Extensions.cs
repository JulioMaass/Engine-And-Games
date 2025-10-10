using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Helpers;

public static class Extensions // TODO - ARCHITECTURE: Move each extension to a separate file
{
    // INTEGER EXTENSIONS
    public static int RoundDownDivision(this int value, int divisor) // Makes division of negative coordinates work as expected
    {
        return value / divisor + ((value % divisor) >> 31);
    }

    // INT VECTOR 2 EXTENSIONS
    public static IntVector2 Sign(this IntVector2 vector)
    {
        return IntVector2.New(Math.Sign(vector.X), Math.Sign(vector.Y));
    }

    public static IntVector2 Abs(this IntVector2 vector)
    {
        vector.X = Math.Abs(vector.X);
        vector.Y = Math.Abs(vector.Y);
        return vector;
    }

    public static int GetBiggestAxis(this IntVector2 vector)
    {
        return Math.Max(vector.X, vector.Y);
    }

    // FLOAT EXTENSIONS
    public static float RemoveDecimalsIfNegligible(this float value, float threshold = 0.001f)
    {
        var nearestInteger = (float)Math.Round(value);
        return Math.Abs(value - nearestInteger) < threshold ? nearestInteger : value;
    }

    public static int GetSign(this float value)
    {
        value = value.RemoveDecimalsIfNegligible();
        return value < 0 ? -1 : 1;
    }

    // TWO DIMENSIONAL ARRAY EXTENSIONS

    // Size properties
    public static int GetWidth<T>(this T[,] array) => array.GetLength(0);
    public static int GetHeight<T>(this T[,] array) => array.GetLength(1);
    public static IntVector2 GetSize<T>(this T[,] array) =>
        IntVector2.New(GetWidth(array), GetHeight(array));

    public static bool IsInBounds<T>(this T[,] array, IntVector2 position)
    {
        return position.X >= 0 && position.X < array.GetLength(0) && position.Y >= 0 && position.Y < array.GetLength(1);
    }

    public static T GetValueAt<T>(this T[,] array, IntVector2 position)
    {
        return array.IsInBounds(position) ? array[position.X, position.Y] : default;
    }

    public static T GetValueAtOrDefault<T>(this T[,] array, IntVector2 position, T defaultValue)
    {
        return array.IsInBounds(position) ? array[position.X, position.Y] : defaultValue;
    }

    public static void SetValueAt<T>(this T[,] array, IntVector2 position, T value)
    {
        if (array.IsInBounds(position))
            array[position.X, position.Y] = value;
    }

    public static T[,] SetValueExpandIfOutOfBounds<T>(this T[,] array, IntVector2 position, T value)
    {
        if (!array.IsInBounds(position))
        {
            var newSize = IntVector2.New(
                Math.Max(array.GetLength(0), position.X + 1),
                Math.Max(array.GetLength(1), position.Y + 1)
            );
            array = array.ResizeArray(newSize);
        }
        array[position.X, position.Y] = value;
        return array;
    }

    public static T[,] NewArray<T>(IntVector2 size, T defaultValue)
    {
        var array = new T[size.Width, size.Height];
        for (var i = 0; i < size.Width; i++)
            for (var j = 0; j < size.Height; j++)
                array[i, j] = defaultValue;
        return array;
    }

    public static IntVector2 GetSizeMin<T>(this T[,] array, IntVector2 size)
    {
        return IntVector2.New(
            Math.Min(array.GetLength(0), size.Width),
            Math.Min(array.GetLength(1), size.Height)
        );
    }

    public static IntVector2 GetSizeMax<T>(this T[,] array, IntVector2 size)
    {
        return IntVector2.New(
            Math.Max(array.GetLength(0), size.Width),
            Math.Max(array.GetLength(1), size.Height)
        );
    }

    private static T[,] ResizeArray<T>(this T[,] originalArray, IntVector2 newSize)
    {
        var newArray = new T[newSize.Width, newSize.Height];
        var minSize = originalArray.GetSizeMin(newSize);
        for (var i = 0; i < minSize.X; i++)
            for (var j = 0; j < minSize.Y; j++)
                newArray[i, j] = originalArray[i, j];
        return newArray;
    }

    public static T[,] ResizeArrayWithOffsetData<T>(this T[,] originalArray, IntVector2 newSize, IntVector2 offset, T defaultValue)
    {
        var newArray = NewArray(newSize, defaultValue);
        var minSize = originalArray.GetSizeMax(newSize);
        for (var i = 0; i < minSize.Width; i++)
        {
            for (var j = 0; j < minSize.Height; j++)
            {
                var originalArrayPosition = IntVector2.New(i, j);
                var value = originalArray.GetValueAtOrDefault(originalArrayPosition, defaultValue);
                var newArrayPosition = IntVector2.New(i + offset.X, j + offset.Y);
                newArray.SetValueAt(newArrayPosition, value);
            }
        }
        return newArray;
    }

    public static void PasteArrayAt<T>(this T[,] originalArray, T[,] arrayToPaste, IntVector2 position)
    {
        for (var i = 0; i < arrayToPaste.GetLength(0); i++)
            for (var j = 0; j < arrayToPaste.GetLength(1); j++)
                originalArray.SetValueAt(IntVector2.New(i + position.X, j + position.Y), arrayToPaste[i, j]);
    }

    public static T[,] NewTransposedArray<T>(T[,] array) // Used to create a 2D array with code in a way that the code position matches the array x,y position
    {
        var width = array.GetLength(1);
        var height = array.GetLength(0);
        var transposedArray = new T[width, height];

        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                transposedArray[x, y] = array[y, x];

        return transposedArray;
    }

    // LIST EXTENSIONS
    public static T GetValueAtOrDefault<T>(this List<T> list, int index, T defaultValue = default)
    {
        return index >= 0 && index < list.Count
            ? list[index]
            : defaultValue;
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list.Count == 0 ?
            default :
            list[Helpers.GetRandom.UnseededInt(list.Count)];
    }

    public static T GetRandomAndRemove<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        var index = Helpers.GetRandom.UnseededInt(list.Count);
        var value = list[index];
        list.RemoveAt(index);
        return value;
    }

    public static void Shuffle<T>(this IList<T> list) // Shuffle list using Fisher-Yates algorithm
    {
        var rng = new Random();
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static List<T> Without<T>(this List<T> list, T item)
    {
        var newList = new List<T>(list);
        newList.Remove(item);
        return newList;
    }

    public static void AddAtIndex<T>(this List<T> list, T item, int index)
    {
        while (list.Count <= index)
            list.Add(default);
        list[index] = item;
    }

    // TEXTURE EXTENSIONS
    public static IntVector2 GetSize(this Texture2D texture)
    {
        return IntVector2.New(texture.Width, texture.Height);
    }

    // STRING EXTENSIONS
    public static string AddLineBreaks(this string text, int maxLineLength, int lineBreakSize)
    {
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = "";
        const char lineBreakChar = '\n';
        var lineBreak = new string(lineBreakChar, lineBreakSize);

        foreach (var word in words)
        {
            // If new word won't fit, move current line to list
            if ((currentLine + word).Length > maxLineLength)
            {
                lines.Add(currentLine.TrimEnd());
                currentLine = "";
            }

            // If current word is a line break, move current line to list
            if (word.Contains(lineBreakChar))
            {
                lines.Add(currentLine.TrimEnd());
                currentLine = "";
                continue;
            }

            // Add word to current line
            currentLine += word + " ";
        }

        // Add last line
        lines.Add(currentLine.TrimEnd());

        //return string.Join("\n", _lines);
        return string.Join(lineBreak, lines);
    }

    // COLOR EXTENSIONS
    public static Color DefaultTo(this Color color, Color defaultOverwrite)
    {
        if (color == default)
            return defaultOverwrite;
        return color;
    }

}
