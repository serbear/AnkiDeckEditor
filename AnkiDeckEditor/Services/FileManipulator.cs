using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnkiDeckEditor.Services;

public static class FileManipulator
{
    public static List<string> GetFileNamesProjectRelative(
        string subDirectoryPath, string extension, string[] filterFileNames)
    {
        List<string> output;
        var projectDirectoryPath = GetProjectDirectoryPath();
        var fullDirectoryPath = Path.Combine(projectDirectoryPath!, subDirectoryPath);

        if (Directory.Exists(fullDirectoryPath))
            output = new List<string>(Directory.GetFiles(fullDirectoryPath, $"*.{extension}"));
        else
            throw new Exception("Directory does not exist.");

        // Remove files that are in the filterFileNames argument.
        output = FilterFiles(ref output, ref filterFileNames);

        return output;
    }

    private static List<string> FilterFiles(ref List<string> fileNames, ref string[] filter)
    {
        if (fileNames.Count.Equals(0)) return [];

        var output = new List<string>();

        foreach (var path in fileNames)
        {
            var fName = Path.GetFileNameWithoutExtension(path);
            if (!filter.Contains(fName)) output.Add(path);
        }

        return output;
    }

    private static string? GetProjectDirectoryPath()
    {
        var parent = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;

        if (parent != null)
        {
            var directoryInfo = parent.Parent;
            if (directoryInfo != null) return directoryInfo.FullName;
        }

        return null;
    }
}