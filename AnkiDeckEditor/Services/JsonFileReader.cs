using System.IO;
using System.Text.Json;

namespace AnkiDeckEditor.Services;

public static class JsonFileReader
{
    public static T? Read<T>(string filePath)
    {
        // todo: доступность файла.
        var text = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(text);
    }
}