using Engine.GameSpecific;
using Engine.Main;
using Engine.Managers.StageHandling;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Engine.Managers.SaveSystem;

public static class JsonHandler // TODO: Load all save files when opening the game, so the game doesn't stutter during gameplay
{
    public static void SaveObjectToFile(object @object, string fileName = null)
    {
        // Serialize
        var json = JsonConvert.SerializeObject(@object, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            //Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });

        // Save to local folder
        fileName = GetFileName(fileName);
        var filePath = GetFilePath(fileName);
        File.WriteAllText(filePath, json);

        // Save to root folder
        var rootFilePath = GetRootFilePath(fileName);
        if (Directory.Exists(Path.GetDirectoryName(rootFilePath)))
            File.WriteAllText(rootFilePath, json);
    }

    private static object LoadObjectFromFile(string fileName, Type type)
    {
        fileName = GetFileName(fileName);
        var filePath = GetFilePath(fileName);

        if (!File.Exists(filePath))
            return null;

        // Load and deserialize
        var json = File.ReadAllText(filePath);
        var @object = JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });

        return @object;
    }

    private static string GetFileName(string fileName)
    {
        if (fileName == null)
            fileName = "default.json";
        else
            fileName += ".json";
        return fileName;
    }

    private static string GetFilePath(string fileName) // save to local (bin) folder 
    {
        var stagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/Stages");
        return Path.Combine(stagePath, fileName);
    }

    private static string GetRootFilePath(string fileName) // save to root (project) folder 
    {
        var stagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\", "Content/Stages", GameManager.GameSpecificSettings.GameFolder, "Stages");
        stagePath = Path.GetFullPath(stagePath);
        return Path.Combine(stagePath, fileName);
    }

    public static Stage LoadStageFromFile(string fileName = null)
    {
        // Create stage from StageData
        var stageData = (StageData)LoadObjectFromFile(fileName, typeof(StageData));
        var stageFromData = stageData?.LoadStageData();
        var stage = stageFromData
                     ?? StageManager.GenerateEmptyStage();

        return stage;
    }

}
