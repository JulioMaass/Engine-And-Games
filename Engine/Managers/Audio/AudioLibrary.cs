using Engine.GameSpecific;
using Engine.Libraries;
using Engine.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Engine.Managers.Audio;

public static class AudioLibrary
{
    public static Dictionary<string, Audio> Dictionary { get; } = new();

    public class Audio
    {
        public WavStream Music;
        public Wav Sound;
        public float LoopPoint;
        public float LoopLength;
        public uint Handle;
    }

    public static void LoadAudioLibrary()
    {
        if (GameManager.GameSpecificSettings.CurrentGame == GameId.Mmdb)
        {
            LoadSound("sndShot");
            LoadMusic("musIntroStage", 68.0f, 64.0f);
            LoadMusic("musMM6BossFull", 33.67f, 19.985f);
        }
    }

    private static void LoadSound(string name, float loopPoint = default, float loopLength = default)
    {
        LoadAudio(name, AudioType.Sound, loopPoint, loopLength);
    }

    private static void LoadMusic(string name, float loopPoint, float loopLength)
    {
        LoadAudio(name, AudioType.Music, loopPoint, loopLength);
    }

    private static void LoadAudio(string fileName, AudioType audioType, float loopPoint, float loopLength)
    {
        var folder = audioType == AudioType.Sound ? "Sounds" : "Music";
        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", folder);
        var filePath = Path.Combine(folderPath, fileName + ".ogg");

        if (!File.Exists(filePath))
            Debugger.Break();

        var audio = new Audio { LoopPoint = loopPoint, LoopLength = loopLength };
        if (audioType == AudioType.Sound)
        {
            var sound = new Wav();
            sound.load(filePath);
            audio.Sound = sound;
        }
        else if (audioType == AudioType.Music)
        {
            var music = new WavStream();
            music.load(filePath);
            audio.Music = music;
        }
        Dictionary.Add(fileName, audio);
    }
}

public enum AudioType
{
    Sound,
    Music
}