using Engine.Libraries;
using System;
using System.Collections.Generic;

namespace Engine.Managers.Audio;

public static class AudioManager
{
    // Soloud
    private static Soloud Soloud { get; } = new();
    private static BiquadResonantFilter LowPassFilter { get; } = new();

    // Music
    private static string CurrentMusicName { get; set; }
    private static uint MusicHandle { get; set; }
    public static Dictionary<string, float> MusicLoopLengthDictionary { get; set; } = new();
    public static Dictionary<string, float> MusicLoopPointDictionary { get; set; } = new();

    // Sounds
    private static List<uint> SoundHandleList { get; } = new();

    // Constants
    private const int LOW_PASS_FREQUENCY = 500;
    private const int LOW_PASS_BYPASS_FREQUENCY = 20000;

    public static void Initialize()
    {
        Soloud.init();
        LowPassFilter.setParams(BiquadResonantFilter.LOWPASS, LOW_PASS_BYPASS_FREQUENCY, 1);
        ToggleMute();
    }

    public static void Update()
    {
        CheckToLoopMusic();
    }

    public static void PlaySound(string soundName) // TODO: Break down in smaller functions
    {
        var audio = AudioLibrary.Dictionary.GetValueOrDefault(soundName);
        var sound = audio?.Sound;
        if (sound is null) return;

        // Stop the last sound of the same type
        var lastSoundHandle = audio.Handle;
        if (lastSoundHandle != 0)
        {
            Soloud.stop(lastSoundHandle);
            SoundHandleList.Remove(lastSoundHandle);
            audio.Handle = default;
        }

        // Stop the oldest sound if all voices are being used
        var maxVoices = Soloud.getMaxActiveVoiceCount() - 1; // -1 because the music is also a voice
        while (SoundHandleList.Count >= maxVoices)
        {
            var handle = SoundHandleList[0];
            Soloud.stop(handle);
            SoundHandleList.Remove(handle);
        }

        // Add filter
        sound.setFilter(1, LowPassFilter);

        // Play the sound and add the handle to the dictionary and queue
        var soundHandle = Soloud.play(sound);
        AudioLibrary.Dictionary.GetValueOrDefault(soundName).Handle = soundHandle;
        SoundHandleList.Add(soundHandle);
    }

    public static void PlayMusic(string currentMusicName)
    {
        // Stop the last music
        Soloud.stop(MusicHandle);

        // Get the music
        var audio = AudioLibrary.Dictionary.GetValueOrDefault(currentMusicName);
        var currentMusic = audio?.Music;
        if (currentMusic is null)
            return;
        CurrentMusicName = currentMusicName;

        // Set volume and filter
        currentMusic.setVolume(0.5f);
        currentMusic.setFilter(1, LowPassFilter);

        // Play the music
        MusicHandle = Soloud.play(currentMusic);
    }

    private static void CheckToLoopMusic()
    {
        if (CurrentMusicName is null) return;
        var audio = AudioLibrary.Dictionary.GetValueOrDefault(CurrentMusicName);
        if (audio is null) return;

        var time = Soloud.getStreamPosition(MusicHandle);
        if (time > audio.LoopPoint)
        {
            var newTime = time - audio.LoopLength;
            Soloud.seek(MusicHandle, newTime);
        }

        // Restart the music if it's not playing
        if (Soloud.isValidVoiceHandle(MusicHandle) == 0)
            PlayMusic(CurrentMusicName);
    }

    public static void PauseMusic()
    {
        Soloud.setPause(MusicHandle, 1);
    }

    public static void ResumeMusic()
    {
        Soloud.setPause(MusicHandle, 0);
    }

    public static void ToggleMute()
    {
        var volume = Soloud.getGlobalVolume();
        if (volume > 0.0f)
            Soloud.setGlobalVolume(0.0f);
        else
            Soloud.setGlobalVolume(1.0f);
    }

    public static void IncreaseVolume()
    {
        var volume = Soloud.getVolume(MusicHandle);
        var changedVolume = Math.Min(volume + 0.1f, 1.0f);
        Soloud.setVolume(MusicHandle, changedVolume);
    }

    public static void DecreaseVolume()
    {
        var volume = Soloud.getVolume(MusicHandle);
        var changedVolume = Math.Max(volume - 0.1f, 0.0f);
        Soloud.setVolume(MusicHandle, changedVolume);
    }

    public static void ActivateLowPassFilter()
    {
        // Apply on current music
        Soloud.fadeFilterParameter(MusicHandle, 1, BiquadResonantFilter.FREQUENCY, LOW_PASS_FREQUENCY, 0.1);
        // Apply future sounds
        LowPassFilter.setParams(BiquadResonantFilter.LOWPASS, LOW_PASS_FREQUENCY, 1);
    }

    public static void DeactivateLowPassFilter()
    {
        // Apply on current music
        Soloud.fadeFilterParameter(MusicHandle, 1, BiquadResonantFilter.FREQUENCY, LOW_PASS_BYPASS_FREQUENCY, 0.1);
        // Apply future sounds
        LowPassFilter.setParams(BiquadResonantFilter.LOWPASS, LOW_PASS_BYPASS_FREQUENCY, 1);
    }
}
