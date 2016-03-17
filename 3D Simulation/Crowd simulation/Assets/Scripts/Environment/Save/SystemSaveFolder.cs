﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {
    static class SystemSaveFolder
    {

        static readonly string systemSaveFolder = System.Environment.SpecialFolder.MyDocuments.ToString();
        public static readonly string Extension = ".RAFT";
        public static readonly string WorldSaveName = "World";

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

        public static int AmountOfFilesWithNameInFolder(string fileName) {
            int number = 0;
            foreach (var file in Directory.GetFiles(System.Environment.CurrentDirectory)) {
                if (file.Contains(fileName)) number++;
            }
            return number;
        }
    }
}