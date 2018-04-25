/// CipherSolver.cs
/// Ian Cordova
/// Senior Project

using System;
using System.Collections.Generic;

namespace CipherSolver
{
    class CipherSolver
    {
        // remove this after testing
        static void Main(string[] args)
        {
            string word = "collection";
            FindAllnGrams(word);

            Console.ReadLine();
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

            //foreach (list<string> i in allngrams)
            //{
            //    console.write("n = " + i[0].length + '\n');
            //    foreach (string j in i)
            //    {
            //        console.write(j + '\n');
            //    }
            //    console.write('\n');
            //}

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
    }
}
