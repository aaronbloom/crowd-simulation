using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Boid {
    static class NameGenerator
    {

        private static Random random = new Random();
        private static bool listsLoaded = false;

        private static List<string> maleFirstNames = new List<string>();
        private static List<string> femaleFirstNames = new List<string>();
        private static List<string> secondNames = new List<string>();
        private static List<string> generatedNames = new List<string>();
        
        public static string GenerateFairlyUniqueName(bool male) {
            string potentialName = GenerateName(male);
            for (int i = 0; i < 3 && !generatedNames.Contains(potentialName); i++) {
                potentialName = GenerateName(male);
                if (!generatedNames.Contains(potentialName)) potentialName = GenerateName(male);
            }
            return potentialName;
        }

        public static string GenerateName(bool male) {
            return GenerateFirstName(male) + GenerateSecondName();
        }

        public static string GenerateFirstName(bool male) {
            if(!listsLoaded) loadLists();
            if (male) {
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
            maleFirstNames = loadList("MaleFirstNames.txt");
            femaleFirstNames = loadList("FemaleFirstNames.txt");
            secondNames = loadList("SecondNames.txt");
        }

        private static List<string> loadList(string fileName) {
            List<string> l = new List<string>();
            try {
                var reader = new StreamReader(File.OpenRead(fileName));
                while (!reader.EndOfStream) {
                    var readLine = reader.ReadLine();
                    if (readLine != null) l.Add(readLine.Trim());
                }
            } catch (IOException e) {
                l.Add(fileName + ": not found");
            }
            if(l.Count == 0) l.Add(fileName + ": was empty");
            return l;
        }

    }
}
