/// CipherSolver.cs
/// Ian Cordova
/// Senior Project

using System;
using System.Collections.Generic;
using System.IO;

namespace CipherSolver
{
    class CipherSolver
    {
        // remove this after testing
        static void Main(string[] args)
        {
            //Console.ReadLine();
        }

        /// <summary>
        /// Finds all possible n-grams in the word
        /// </summary>
        /// 
        /// <param name="a_word"> word which will be parsed into n-grams </param>
        /// 
        /// <returns>
        /// List of Lists of strings - each list contains a list of strings which are the n-grams for that length
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 9:40pm, 4/24/2018
        /// </author>
        private static List<List<string>> FindAllnGrams(string a_word)
        {
            List<List<string>> allnGrams = new List<List<string>>();
            for(int i = 2; i < a_word.Length; ++i)
            {
                allnGrams.Add(FindnGrams(i, a_word));
            }

            return allnGrams;
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
        private static List<string> FindnGrams(int a_nlength, string a_word)
        {
            // each n-gram will be stored in this list
            List<string> nGramList = new List<string>();
            string ngram;

            // these pivots will determine each n-gram and we will shift them down the word
            int pivot1 = 0;
            int pivot2 = a_nlength;

            // verify string is only one word
            foreach(char i in a_word)
            {
                if (IsEnglishLetter(i)) continue;
                throw new Exception("String a_word consists of invalid characters.");
            }
            
            // iterate over the word, ends when pivot2 hits the end of the word
            while (pivot2 <= a_word.Length)
            {
                ngram = "";
                // add all letters between pivot1 and pivot2 to string
                for(int i = 0; i < a_nlength; ++i)
                {
                    ngram += a_word[pivot1 + i];
                }
                nGramList.Add(ngram);

                //Console.Write(ngram + '\n');

                // shift down one letter
                pivot1++;
                pivot2++;
            }

            return nGramList;
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
        private static bool IsEnglishLetter(char a_character)
        {
            return ((a_character >= 'A' && a_character <= 'Z') || (a_character >= 'a' && a_character <= 'z'));
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

            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSet.csv";

            // opening a filestream and reading it as a buffered stream is more efficient for large files
            using (FileStream fs = File.Open(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                string word;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    word = "";

                    // iterate over the line char by char
                    for(int i = 0; i < line.Length; ++i)
                    {
                        // begin saving letters to create a word
                        if (IsEnglishLetter(line[i]))
                        {
                            word += line[i];
                        }

                        // we reached the end of a word or the end of a word at the end of a line
                        if ((!IsEnglishLetter(line[i]) || i == line.Length - 1) && (word != "")) 
                        {
                            // save all bigrams to dictionary, increment duplicates
                            grams = FindnGrams(2, word);
                            foreach(string ngram in grams)
                            {
                                // increment bigram
                                if(bigrams.ContainsKey(ngram))
                                {
                                    bigrams[ngram]++;
                                }
                                // first bigram
                                else
                                {
                                    bigrams[ngram] = 1;
                                }
                            }
                            grams.Clear();

                            // save all trigrams to dictionary, increment duplicates
                            grams = FindnGrams(3, word);
                            foreach(string ngram in grams)
                            {
                                if(trigrams.ContainsKey(ngram))
                                {
                                    trigrams[ngram]++;
                                }
                                else
                                {
                                    trigrams[ngram] = 1;
                                }
                                
                            }
                            grams.Clear();
                            word = "";
                        }
                    } 
                } // end read line
            } // end using

            // write dictionaries to files
            using (StreamWriter biGramFile = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetBigrams.txt"))
            {
                foreach(var entry in bigrams)
                {
                    biGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }
            using (StreamWriter triGramFile = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetTrigrams.txt"))
            {
                foreach (var entry in trigrams)
                {
                    triGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }
        }
    }
}
