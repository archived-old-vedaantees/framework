#region  usings 

using System;
using System.IO;
using System.Threading.Tasks;
using Vedaantees.Framework.Types.Results;

#endregion

namespace Vedaantees.Framework.Providers.FileSystem
{
    /// <summary>
    ///     Basic functionality required to safely create or delete the files.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        ///     Gets the assembly directory.
        /// </summary>
        /// <value>
        ///     The assembly directory.
        /// </value>
        public static string AssemblyDirectory => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     Deletes the File if it exist.
        /// </summary>
        public static async Task<MethodResult> CheckAndDeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                try
                {
                    await Task.Run(() => { File.Delete(filePath); });
                    return new MethodResult(MethodResultStates.Successful, "");
                }
                catch (Exception exception)
                {
                    return new MethodResult(exception, $"Error occured deleting the file {filePath}");
                }

            return new MethodResult(MethodResultStates.UnSuccessful, $"File not found {filePath}");
        }

        /// <summary>
        ///     Check if file exists.
        /// </summary>
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        ///     Reads the file to string.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static async Task<MethodResult<string>> ReadFileToString(string filePath)
        {
            if (File.Exists(filePath))
                try
                {
                    var content = await Task.Run(() => File.ReadAllText(filePath));
                    return new MethodResult<string>(content);
                }
                catch (Exception exception)
                {
                    return new MethodResult<string>(exception, $"Error occured while reading file: {filePath}");
                }

            return new MethodResult<string>(MethodResultStates.UnSuccessful, $"File not found {filePath}");
        }

        /// <summary>
        ///     Writes the string to file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static async Task<MethodResult> WriteStringToFile(string content, string filePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    var writer = File.CreateText(filePath);
                    writer.Write(content);
                    writer.Close();
                });

                return new MethodResult(MethodResultStates.Successful, "");
            }
            catch (Exception exception)
            {
                return new MethodResult(exception, $"Error occured while writing file:{filePath}");
            }
        }
    }
}