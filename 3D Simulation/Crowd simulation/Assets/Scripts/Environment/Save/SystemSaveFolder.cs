using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {

    /// <summary>
    /// Handles File Operations
    /// </summary>
    internal static class SystemSaveFolder {

        public const string Extension = ".RAFT"; //Raft's A File Type
        public const string WorldSaveName = "World";

        /// <summary>
        /// Writes a serialisable object to a file with name: <paramref name="preferedFileName"/>, a (num) will be appended if the file exists already
        /// </summary>
        /// <param name="preferedFileName">The filename you want</param>
        /// <param name="serialisable">A Serializable Object</param>
        public static void WriteObjectToFolder(string preferedFileName, object serialisable) {
            int fileNumber = 0;
            string fileName;
            do {
                fileName = preferedFileName + " (" + fileNumber + ")" + Extension;
                fileNumber++;
            } while (File.Exists(fileName));

            try {
                using (Stream stream = File.Open(fileName, FileMode.Create)) {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, serialisable);
                }
            } catch (IOException e) {
                Debug.Log("Save Failed: " + e.Message);
            }
        }

        /// <summary>
        /// Loads a file into an object, deserialising
        /// </summary>
        /// <param name="fileName">The serialised and saved object you wish to deserialise</param>
        /// <returns>the deserialised object, ready for casting</returns>
        public static object LoadFileFromFolder(string fileName) {
            fileName = fileName + Extension;
            try {
                if(!File.Exists(fileName)) throw new FileNotFoundException();
                using (Stream stream = File.Open(fileName, FileMode.Open)) {
                    return new BinaryFormatter().Deserialize(stream);
                }
            } catch (IOException e) {
                Debug.Log("Save Failed: " + e.Message);
            }
            return null;
        }

        /// <summary>
        /// Returns the amount of files in the save folder which contain the name: <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">The name to count</param>
        /// <returns>the number of files with that name</returns>
        public static int AmountOfFilesWithNameInFolder(string fileName) {
            return Directory.GetFiles(System.Environment.CurrentDirectory).Count(file => file.Contains(fileName));
        }
    }
}
