#region  usings 

using System;
using System.IO;
using Vedaantees.Framework.Types.Results;

#endregion

namespace Vedaantees.Framework.Providers.FileSystem
{
    /// <summary>
    ///     Basic functionality required to safely create or delete the folders.
    /// </summary>
    public static class FolderManager
    {
        /// <summary>
        ///     Creates a new directory, if not present. Returns false on any error.
        /// </summary>
        public static MethodResult<DirectoryInfo> CheckAndCreateDirectory(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                    return new MethodResult<DirectoryInfo>(new DirectoryInfo(folderPath));

                Directory.CreateDirectory(folderPath);
                return new MethodResult<DirectoryInfo>(new DirectoryInfo(folderPath));
            }
            catch (Exception exception)
            {
                return new MethodResult<DirectoryInfo>(exception,
                    $"Error occured while creating directory: {folderPath}");
            }
        }

        /// <summary>
        ///     Deletes directory and its content, if not present. Returns false on any error.
        /// </summary>
        public static MethodResult CheckAndDeleteDirectory(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    RecursiveDelete(new DirectoryInfo(folderPath));
                    return new MethodResult(MethodResultStates.Successful, "");
                }

                return new MethodResult(MethodResultStates.UnSuccessful,$"Directory not found {folderPath}");
            }
            catch (Exception exception)
            {
                return new MethodResult(exception, $"Error occured while deleting directory: {folderPath}");
            }
        }

        /// <summary>
        ///     Checks if folders exists.
        /// </summary>
        public static bool FolderExists(string strFolderPath)
        {
            return Directory.Exists(strFolderPath);
        }

        /// <summary>
        ///     Recursivesly deletes all folder and its nested folder/contents.
        /// </summary>
        /// <param name="baseDir">The base dir.</param>
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories()) RecursiveDelete(dir);
            baseDir.Delete(true);
        }
    }
}