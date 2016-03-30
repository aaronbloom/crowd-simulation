using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Boid {
    internal static class NameGenerator {

        private static readonly Random random = new Random();
        private static List<string> maleFirstNames = new List<string>();
        private static List<string> femaleFirstNames = new List<string>();
        private static List<string> secondNames = new List<string>();
        private static readonly List<string> generatedNames = new List<string>();
        private static bool listsLoaded;

        /// <summary>
        /// Generates a name that's not guarenteed to be unique, but most likely will be. (3 attempts are made at unique gen)
        /// </summary>
        /// <param name="gender">The required gender of the name</param>
        /// <returns>name</returns>
        public static string GenerateFairlyUniqueName(Gender gender) {
            string potentialName = GenerateName(gender);
            for (int i = 0; (i < 3) && !generatedNames.Contains(potentialName); i++) {
                potentialName = GenerateName(gender);
                if (!generatedNames.Contains(potentialName)) potentialName = GenerateName(gender);
            }
            return potentialName;
        }

        /// <summary>
        /// Generates a name that's not guarenteed to be unique
        /// </summary>
        /// <param name="gender">The required gender of the name</param>
        /// <returns>name</returns>
        public static string GenerateName(Gender gender) {
            return GenerateFirstName(gender) + " " + GenerateSecondName();
        }

        /// <summary>
        /// Generates a first name that's not guarenteed to be unique
        /// </summary>
        /// <param name="gender">The required gender of the name</param>
        /// <returns>name</returns>
        public static string GenerateFirstName(Gender gender) {
            if(!listsLoaded) LoadLists();
            return gender == Gender.Male ?
                maleFirstNames[random.Next(0, maleFirstNames.Count)] :
                femaleFirstNames[random.Next(0, femaleFirstNames.Count)];
        }

        /// <summary>
        /// Generates a second name that's not guarenteed to be unique
        /// </summary>
        /// <param name="gender">The required gender of the name</param>
        /// <returns>name</returns>
        public static string GenerateSecondName() {
            if (!listsLoaded) LoadLists();
            return secondNames[random.Next(0, secondNames.Count)];
        }

        /// <summary>
        /// Loads the name files into string lists
        /// </summary>
        public static void LoadLists() {
            maleFirstNames = loadList("MaleFirstNames");
            femaleFirstNames = loadList("FemaleFirstNames");
            secondNames = loadList("SecondNames");
            listsLoaded = true;
        }

        /// <summary>
        /// Opens a file with name: <paramref name="fileName"/> and turns each line into a string
        /// </summary>
        /// <param name="fileName">the name of the file to be loaded</param>
        /// <returns>List of strings representing lines in the file</returns>
        private static List<string> loadList(string fileName) {
            List<string> l = new List<string>();
            try {
                TextAsset file = Resources.Load("Name Files/" + fileName) as TextAsset;
                var raw = Regex.Split(file.text, @"\r\n");
                l.AddRange(raw.Select(s => s.Trim()));
            } catch (IOException) {
                l.Add(fileName + ": not found");
            }
            if(l.Count == 0) l.Add(fileName + ": was empty");
            return l;
        }

    }
}
