﻿using Microsoft.Maui.Devices;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Maui.Storage;

namespace KeyChordFinder.Data
{
    public class DbHelper
    {
        public static void CopyIfDoesntExist(string dbName, Assembly assembly)
        {
            string dbPath = GetDbPath(dbName);
            if (!File.Exists(dbPath))
            {
                using (var stream = GetEmbeddedResourceStream(dbName, assembly))
                {
                    using (var fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }

        public static string GetDbPath(string dbName)
        {
            DevicePlatform platform = DeviceInfo.Platform;

            if (platform == DevicePlatform.iOS)
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", dbName);
            }
            if (platform == DevicePlatform.Android)
            {
                return Path.Combine(FileSystem.AppDataDirectory, dbName);
            }

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
        }

        private static Stream GetEmbeddedResourceStream(string resourceName, Assembly assembly)
        {
            var resourcePath = $"KeyChordMudBlazor.Resources.Raw.{resourceName}";
            return assembly.GetManifestResourceStream(resourcePath)!;
        }
    }
}
