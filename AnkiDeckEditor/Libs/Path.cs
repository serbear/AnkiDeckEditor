using System;
using System.Runtime.InteropServices;

namespace AnkiDeckEditor.Libs;

public static class Path
{
    public static string ConfigPath(string applicationName, string fileName)
    {
        var configDirectory = string.Empty;
        const string CONFIG_DIRECTORY = ".config";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            configDirectory = System.IO.Path.Combine(appData, applicationName);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            configDirectory = System.IO.Path.Combine(homeDirectory, CONFIG_DIRECTORY, applicationName);
        }

        return System.IO.Path.Combine(configDirectory, fileName);
    }
}