using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Cipher.CipherUtility;

namespace CipherTrainingData
{
    /// <summary>
    /// This application is not included into the final UWP application. This class/console application
    /// is used for creating training data and adding additional data to it.
    /// </summary>
    class CipherTrainingData
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
        }

        /// <summary>
        /// Allows me to add data to the dataset of bigrams by just changing which file is specified.
        /// Loads the data from the file (Note: file does not get substantially larger as data is added. There is a maximum of
        /// 676 possible bi-grams, so adding data only approaches that number of lines while increasing the count
        /// next to each existing bi-gram.
        /// </summary>
        /// 
        /// <author>
        /// Ian Cordova - 4:00pm 5/4/2018
        /// </author>
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
