/// CipherSolver.cs
/// Ian Cordova
/// Senior Project

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CipherSolver
{
    class CipherSolver
    {
        // remove this after testing
        static void Main(string[] args)
        {
            /*string plainText = "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG";
           
            Console.WriteLine('\n');
            Dictionary<string, int> plainTriGrams = new Dictionary<string, int>();
            plainTriGrams = FindnGrams(3, plainText);
            foreach (var entry in plainTriGrams)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }

            Console.WriteLine(plainTriGrams.Values.Max()); */

            //GenerateInitialChromosome();
            GetnGramsFromDataSet();
            //Console.ReadLine();
        }

        /// <summary>
        /// Generates the starting chromosome for the genetic algorithm.
        /// Uses the top tri grams from the cipher text and gets the top few trigrams and some random 
        /// trigrams from the data set to use as the initial chromosome.
        /// </summary>
        /// 
        /// <returns>
        /// void
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 11:00pm, 4/29/2018 
        /// </author>
        static void GenerateInitialChromosome()
        {
            string cipherText = "NQMWTRN YP WTOKMYO E FEOKVQ OMML E OMJJ MR IVR ERB CYQOTX FEZTJA JTFVXOAJV ERB FYOYQV IVTRN TR ER VRGTQMRZVRO OKEO WEX MRJA PEQOTEJJA XYPPMQOTGV KEJOVB OKVTQ CQTOTCEJ BVGVJMPZVRO OKVTQ FEOKVQ WMYJBRO XYPPMQO OKV CKTJBQVR EFOVQ KTX XVPEQEOTMR WTOK XMRAE IVRRTV CMYJBRO YRBVQXOERB WKA XMZVMRV OKEO JMGVX KTZ WMYJBRO PQMGTBV ZMRVA FMQ FMMB ER VTNKO AVEQ MJB CKTJB XKMYJB RMO IV CMRCVQRVB WTOK MQ KEGV OM WMQQA EIMYO FTRERCTEJ TRDYTQTVX RMO IVTRN XYPPJTVB WTOK VRMYNK VEQRTRN OM QVXTBV TR OKVTQ MQTNTREJ KMYXTRN OKV CEQXMRX WVQV QVDYTQVB OM ZMGV FQMZ BVOQMTO OM IMXOMR OKVA ZMGVB TR WTOK HVER ERB WTJJTEZ EGVQA XMRAEX MJBVQ XTXOVQ ERB IQMOKVQ TR JEW TO WEXRO E RMQZEJ MQ TBVEJ JTGTRN XTOYEOTMR IYO TO WEX OKV XVRXV MF FEZTJA YRTOA";

            Dictionary<string, int> chromosome = new Dictionary<string, int>();
            Dictionary<string, int> cipherTriGrams = new Dictionary<string, int>();
            cipherTriGrams = FindnGrams(3, cipherText);

            cipherTriGrams = (from entry in cipherTriGrams orderby entry.Value descending select entry)
              .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var entry in cipherTriGrams)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static Dictionary<string, int> GetTopnGrams()
        {
            Dictionary<string, int> topnGrams = new Dictionary<string, int>();



            return topnGrams;
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
        /*private static List<List<string>> FindAllnGrams(string a_word)
        {
            List<List<string>> allnGrams = new List<List<string>>();
            for(int i = 2; i < a_word.Length; ++i)
            {
                allnGrams.Add(FindnGrams(i, a_word));
            }

            return allnGrams;
        }*/
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
        private static Dictionary<string, int> FindnGrams(int a_nlength, string a_text)
        {
            // each n-gram will be stored in this dictionary
            Dictionary<string, int> nGramDict = new Dictionary<string, int>();
            string ngram;
            string word = "";

            // these pivots will determine each n-gram and we will shift them down the word
            int pivot1 = 0;
            int pivot2 = a_nlength;

            for(int i = 0; i < a_text.Length; ++i)
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
            string fullText = "";

            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSet.csv";

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

                bigrams = FindnGrams(2, fullText);
                trigrams = FindnGrams(3, fullText);
            }

            // write dictionaries to files
            using (StreamWriter biGramFile = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetBigrams.txt"))
            {
                bigrams = (from entry in bigrams orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var entry in bigrams)
                {
                    biGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }
            using (StreamWriter triGramFile = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetTrigrams.txt"))
            {
                trigrams = (from entry in trigrams orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (var entry in trigrams)
                {
                    triGramFile.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }
            }
        }

        /*
        static void RemoveLineEndings()
        {
            using (StreamWriter newdata = new StreamWriter("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSet2.csv"))
            using (FileStream fs = new FileStream("C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSet.csv", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    {
                        newdata.WriteLine(Regex.Replace(line, "FALSE", ""));
                    }
                }
            }
        }*/
    }
}
