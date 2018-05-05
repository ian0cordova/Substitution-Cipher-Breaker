using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Cipher.CipherUtility;

namespace CipherTrainingData
{
    class Program
    {
        static void Main(string[] args)
        {
            AddDataToDataSet();
        }
        /// <summary>
        /// This is a method that is only called when creating our reference/training set of data.
        /// ie it should be only called once, and not during normal project run time. This method
        /// should probably be a separate entity in the form of a separate program or a script, but 
        /// since it uses the same functionality that the main project does I am including it here 
        /// to see everything that was done to achieve the finished result
        /// </summary>
        /// 
        /// <returns>
        /// void - but reads/writes to files
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 6:00pm 4/25/2018
        /// </author>
        private static void GetnGramsFromDataSet()
        {
            // dictionaries to store all n-grams and their number of occurances [n-gram, count]
            Dictionary<string, int> bigrams = new Dictionary<string, int>();
            Dictionary<string, int> trigrams = new Dictionary<string, int>();

            // temporary storage for grams after they are found
            List<string> grams = new List<string>();
            string fullText = "";

            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherTrainingData\\DataSet.csv";

            using (FileStream fs = File.Open(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    fullText += line; // this is so ineffective and slow, but im lazy and only have to run this once
                }

                bigrams = CipherUtility.FindnGrams(2, fullText);
                //trigrams = FindnGrams(3, fullText);
            }

            // write dictionaries to files
            using (StreamWriter biGramFile = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherTrainingData\\DataSetBigrams.txt"))
            {
                bigrams = (from entry in bigrams orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var entry in bigrams)
                {
                    biGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }
            /*using (StreamWriter triGramFile = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetTrigrams.txt"))
            {
                trigrams = (from entry in trigrams orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (var entry in trigrams)
                {
                    triGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }*/

        }

        private static void AddDataToDataSet()
        {
            var allBiGrams = CipherUtility.GetTrainingBiGrams();
            var newBiGrams = new Dictionary<string, int>();
            string fullText = "";
            string destPath = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherTrainingData\\DataSetBigrams.txt";

            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherTrainingData\\twocities.txt";

            using (FileStream fs = File.Open(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    fullText += line; // this is so ineffective and slow, but im lazy and only have to run this once
                }
            }

            newBiGrams = CipherUtility.FindnGrams(2, fullText);

            foreach(var entry in newBiGrams)
            {
                if(allBiGrams.ContainsKey(entry.Key))
                {
                    allBiGrams[entry.Key] += entry.Value;
                }
                else
                {
                    allBiGrams.Add(entry.Key, 1);
                }
            }

            File.WriteAllText(destPath, "");
            using (StreamWriter biGramFile = new StreamWriter(destPath))
            {
                allBiGrams = (from entry in allBiGrams orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var entry in allBiGrams)
                {
                    biGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }
        }
    }
}
