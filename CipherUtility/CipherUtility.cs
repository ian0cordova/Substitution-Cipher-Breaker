/// CipherUtility.cs
/// Ian Cordova
/// Senior Project

using System;
using System.IO;
using System.Collections.Generic;

namespace Cipher.CipherUtility
{
    /// <summary>
    /// This class provides some random functionality needed by
    /// multiple different libraries to prevent the rewriting of code.
    /// </summary>
    public class CipherUtility
    {
        /// <summary>
        /// Reads in all bi-grams and their frequency into a dictionary for use
        /// </summary>
        /// 
        /// <returns>
        /// Dictionary containing bi-grams and their frequency from the training data
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 7:00pm 5/2/2018
        /// </author>
        public static Dictionary<string, int> GetTrainingBiGrams()
        {
            Dictionary<string, int> trainingBiGrams = new Dictionary<string, int>();
            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherBreakerUWP\\Assets\\DataSetBiGrams.txt";

            using (FileStream fs = File.Open(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;

                // read file and populate dict
                while ((line = sr.ReadLine()) != null)
                {
                    Tuple<string, int> nGramData = ParseDataSetLine(line);
                    trainingBiGrams.Add(nGramData.Item1, nGramData.Item2);
                }
            }
            return trainingBiGrams;
        }

        /// <summary>
        /// Helper method to parse the n-gram and count from the data set.
        /// Format looks like [THE 12345] where THE is the n-gram and 12345 is the count
        /// </summary>
        /// 
        /// <param name="a_line"> line to be parsed </param>
        /// 
        /// <returns>
        /// Returns a tuple which is to be an entry in a dictionary
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 1:00am 5/2/2018
        /// </author>
        public static Tuple<string, int> ParseDataSetLine(string a_line)
        {
            string nGram = "";
            string nGramCount = "";
            int nGramCountNum;

            foreach (char ch in a_line)
            {
                if (ch == '[' || ch == ']' || ch == ' ') continue;

                // read n-gram
                else if (IsEnglishLetter(ch))
                {
                    nGram += ch;
                }
                // read n-gram count
                else if (Char.IsNumber(ch))
                {
                    nGramCount += ch;
                }
                // wrong format
                else
                {
                    throw new Exception();
                }
            }
            nGramCountNum = Int32.Parse(nGramCount);

            Tuple<string, int> nGramData = new Tuple<string, int>(nGram, nGramCountNum);
            return nGramData;
        }

        /// <summary>
        /// Finds all of the n-grams of a letter given a specific n value
        /// </summary>
        /// 
        /// <param name="a_nlength"> specified length of n-grams </param>
        /// <param name="a_word"> word to parse n-grams from </param>
        /// 
        /// <returns>
        /// Returns list of n-grams in a list
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 9:30pm, 4/24/2018
        /// </author>
        public static Dictionary<string, int> FindnGrams(int a_nlength, string a_text)
        {
            // each n-gram will be stored in this dictionary
            Dictionary<string, int> nGramDict = new Dictionary<string, int>();
            string ngram;
            string word = "";

            // these pivots will determine each n-gram and we will shift them down the word
            int pivot1 = 0;
            int pivot2 = a_nlength;

            for (int i = 0; i < a_text.Length; ++i)
            {
                // continue building word
                if (IsEnglishLetter(a_text[i]))
                {
                    word += a_text[i];
                    continue;
                }
                // reached end of word, now find n-grams
                if ((!IsEnglishLetter(a_text[i]) || i == a_text.Length - 1) && (word != ""))
                {
                    pivot1 = 0;
                    pivot2 = a_nlength;
                    // iterate over the word, ends when pivot2 hits the end of the word
                    while (pivot2 <= word.Length)
                    {
                        ngram = "";
                        // add all letters between pivot1 and pivot2 to string
                        for (int j = 0; j < a_nlength; ++j)
                        {
                            ngram += word[pivot1 + j];
                        }
                        // n-gram already exists in dictionary, increment count
                        if (nGramDict.ContainsKey(ngram))
                        {
                            nGramDict[ngram]++;
                        }
                        // n-gram does not exist in dictionary, initialize count
                        else
                        {
                            nGramDict[ngram] = 1;
                        }
                        // shift down one letter
                        pivot1++;
                        pivot2++;
                    }
                    word = "";
                }
            }

            return nGramDict;
        }

        /// <summary>
        /// Determines if the specified character is actually a letter
        /// </summary>
        /// 
        /// <param name="a_character"> Single character to test </param>
        /// 
        /// <returns>
        /// If char is an english letter return true, else false.
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 9:00pm, 4/24/2018
        /// </author>
        public static bool IsEnglishLetter(char a_character)
        {
            return ((a_character >= 'A' && a_character <= 'Z') || (a_character >= 'a' && a_character <= 'z'));
        }
    }
}
