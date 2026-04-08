using System.IO;
using UnityEngine;
using MyExceptions;

public static class FilesIO
{
    public static T Read<T>(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("JSON file not found: " + path);
            throw new NoFileFound("Not find file:" + path);
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    public static void Write<T>(T data, string path)
    {
        string json = JsonUtility.ToJson(data);
        //Debug.Log(json);
        File.WriteAllText(path, json);

        Debug.Log("Saved data to: " + path);
    }
}
