using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {
    static class SystemSaveFolder
    {

        static readonly string systemSaveFolder = System.Environment.SpecialFolder.MyDocuments.ToString();

        public static void WriteObjectToFolder(string preferedFileName, object serialisable) {

            int fileNumber = 0;
            string fileName;
            do {
                fileName = preferedFileName + " (" + fileNumber + ")";
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

        public static object LoadFileFromFolder(string fileName) {
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

        public static int AmountOfFilesWithNameInFolder(string fileName) {
            return 3;
        }

    }
}
