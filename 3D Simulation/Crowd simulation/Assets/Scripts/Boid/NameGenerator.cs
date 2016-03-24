using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Boid {
    static class NameGenerator
    {

        private static Random random = new Random();
        private static bool listsLoaded = false;

        private static List<string> maleFirstNames = new List<string>();
        private static List<string> femaleFirstNames = new List<string>();
        private static List<string> secondNames = new List<string>();
        private static List<string> generatedNames = new List<string>();

        public static string GenerateFairlyUniqueName(Gender gender) {
            string potentialName = GenerateName(gender);
            for (int i = 0; i < 3 && !generatedNames.Contains(potentialName); i++) {
                potentialName = GenerateName(gender);
                if (!generatedNames.Contains(potentialName)) potentialName = GenerateName(gender);
            }
            return potentialName;
        }

        public static string GenerateName(Gender gender) {
            return GenerateFirstName(gender) + " " + GenerateSecondName();
        }

        public static string GenerateFirstName(Gender gender) {
            if(!listsLoaded) loadLists();
            if (gender == Gender.MALE) {
                return maleFirstNames[random.Next(0, maleFirstNames.Count)];
            } else {
                return femaleFirstNames[random.Next(0, femaleFirstNames.Count)];
            }

        }

        public static string GenerateSecondName() {
            if (!listsLoaded) loadLists();
            return secondNames[random.Next(0, secondNames.Count)];
        }

        public static void loadLists() {
            maleFirstNames = loadList("MaleFirstNames");
            femaleFirstNames = loadList("FemaleFirstNames");
            secondNames = loadList("SecondNames");
            listsLoaded = true;
        }

        private static List<string> loadList(string fileName) {
            List<string> l = new List<string>();
            try {
                TextAsset file = Resources.Load("Name Files/" + fileName) as TextAsset;
                var raw = Regex.Split(file.text, @"\r\n");
                foreach (string s in raw) {
                    l.Add(s.Trim());
                }
            } catch (IOException e) {
                l.Add(fileName + ": not found");
            }
            if(l.Count == 0) l.Add(fileName + ": was empty");
            return l;
        }

    }
}
