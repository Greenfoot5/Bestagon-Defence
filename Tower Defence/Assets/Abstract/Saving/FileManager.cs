using System;
using System.IO;
using UnityEngine;

namespace Abstract.Saving
{
    /// <summary>
    /// Handles any loading/saving to files
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Writes a string to a certain file
        /// </summary>
        /// <param name="fileName">The full path of the file to write to</param>
        /// <param name="fileContents">The contents to put into the file</param>
        /// <returns>true if the write was successful</returns>
        public static bool WriteToFile(string fileName, string fileContents)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(fullPath, fileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }
        
        /// <summary>
        /// Loads the contents of a file
        /// </summary>
        /// <param name="fileName">The full path of the file to load</param>
        /// <param name="result">The loaded contents of the file</param>
        /// <returns>true if the load was successful</returns>
        public static bool LoadFromFile(string fileName, out string result)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
    }
}