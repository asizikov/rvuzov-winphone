﻿using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;

namespace TimeTable.Services
{
    public static class CrashLogger
    {
        private const string DirectoryName = "CrashReports";

        public static void SaveCrashInfo(ApplicationUnhandledExceptionEventArgs e)
        {
            if (e == null) return;

            var innerMessage = "";
            var innerStackTrace = "";
            var date = DateTime.UtcNow.Date.ToString("yy-MMM-dd");
            var time = DateTime.UtcNow.Ticks;
            var message = e.ExceptionObject.Message;
            var stackTrace = e.ExceptionObject.StackTrace;

            var fileSuffix = RemoveInvalidCharacters(message + date + time.ToString(CultureInfo.InvariantCulture));

            if (e.ExceptionObject.InnerException != null)
            {
                innerMessage = e.ExceptionObject.InnerException.Message;
                innerStackTrace = e.ExceptionObject.InnerException.StackTrace;
            }

            var fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!fileStorage.GetDirectoryNames().Contains(DirectoryName))
            {
                fileStorage.CreateDirectory(DirectoryName);
            }
            var fileName = DirectoryName + "\\" + fileSuffix + ".txt";

            using (var fileWriter = new StreamWriter(fileStorage.OpenFile(fileName, FileMode.Append)))
            {
                fileWriter.WriteLine(message + stackTrace + innerMessage + innerStackTrace);
            }
        }

        private static string RemoveInvalidCharacters(string fileName)
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var name = fileName;
            var invalids = invalidFileNameChars.Where(c => name.IndexOf(c) >= 0);
            fileName = invalids.Aggregate(fileName,
                (current, invalidFileNameChar) => current.Replace(invalidFileNameChar, '-'));
            return fileName;
        }
    }
}