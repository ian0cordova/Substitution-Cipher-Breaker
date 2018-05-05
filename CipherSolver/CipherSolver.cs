/// CipherSolver.cs
/// Ian Cordova
/// Senior Project

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Cipher.CipherMaker;

namespace Cipher.CipherSolver
{
    /// <summary>
    /// Handles the deciphering of encoded text
    /// </summary>
    class CipherSolver
    {
        // remove this after testing
        static void Main(string[] args)
        {
            string cipherText = File.ReadAllText("C:\\Users\\icordova\\Documents\\cipherText.txt");
            Dictionary<char, char> solution = new Dictionary<char, char>()
            {
                {'A', 'Y' },
                {'B', 'S' },
                {'C', 'E' },
                {'D', 'H' },
                {'E', 'C' },
                {'F', 'A' },
                {'G', 'I' },
                {'H', 'N' },
                {'I', 'T' },
                {'J', 'P' },
                {'K', 'L' },
                {'L', 'V' },
                {'M', 'Z' },
                {'N', 'M' },
                {'O', 'W' },
                {'P', 'D' },
                {'Q', 'O' },
                {'R', 'R' },
                {'S', 'G' },
                {'T', 'F' },
                {'U', 'U' },
                {'V', 'Q' },
                {'W', 'K' },
                {'X', 'B' },
                {'Y', 'J' },
                {'Z', 'X' }
            };
            // possibly read training data set into memory here or as a private member
            Dictionary<string, int> dataSetBiGrams = GetTrainingBiGrams();

            // generate initial population of chromosomes/guesses at a solution
            List<Dictionary<char, char>> population = new List<Dictionary<char, char>>();
            for(int i = 0; i < POPULATION_SIZE; ++i)
            {
                population.Add(GenerateChromosome(cipherText, dataSetBiGrams));
            }
                
            int lastScore = 0;
            int lastScoreIncrease = 0;
            int iterations = -STABILITY_INTERVALS;

            // begin genetic algorithm - iterate through generations of populations of chromosomes
            while(lastScoreIncrease < STABILITY_INTERVALS)
            {
                // breed/mutate next generation
                population = NextGeneration(population);

                var topChromosomes = SelectTopChromosomes(population, cipherText, dataSetBiGrams);
                var bestScore = topChromosomes.Item2;
                Console.WriteLine(bestScore);
                population = topChromosomes.Item1;

                if(bestScore > lastScore)
                {
                    lastScoreIncrease = 0;
                    lastScore = bestScore;
                }
                else
                {
                    lastScoreIncrease += 1;
                }
                iterations += 1;
            }

            Console.WriteLine("Best solution found after {0} iterations", iterations);
            Console.WriteLine(DecodeText(cipherText, population[0]));
            foreach(var entry in population[0])
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }

            double accuracy = 0;
            foreach(var entry in solution)
            {
                if (population[0].ContainsKey(entry.Key) && population[0][entry.Key] == entry.Value) 
                {
                    accuracy++;
                }
            }
            Console.WriteLine("Letters correct: " + accuracy);




            /*string text_txt = File.ReadAllText("C:\\Users\\icordova\\Documents\\test.txt");

            Dictionary<string, int> ngrams = new Dictionary<string, int>(FindnGrams(2, text_txt));
            ngrams = (from entry in ngrams orderby entry.Value descending select entry)
  .ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var entry in ngrams)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }*/

            Console.ReadLine();
        }

        private const string m_Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int POPULATION_SIZE = 500;
        private const int TOP_POPULATION = 75;
        private const int STABILITY_INTERVALS = 10;
        private const int CROSSOVER_COUNT = 1;
        private const int MUTATIONS_COUNT = 1;

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
        /*static void GenerateInitialChromosome()
        {
            string cipherText = "NQMWTRN YP WTOKMYO E FEOKVQ OMML E OMJJ MR IVR ERB CYQOTX FEZTJA JTFVXOAJV ERB FYOYQV IVTRN TR ER VRGTQMRZVRO OKEO WEX MRJA PEQOTEJJA XYPPMQOTGV KEJOVB OKVTQ CQTOTCEJ BVGVJMPZVRO OKVTQ FEOKVQ WMYJBRO XYPPMQO OKV CKTJBQVR EFOVQ KTX XVPEQEOTMR WTOK XMRAE IVRRTV CMYJBRO YRBVQXOERB WKA XMZVMRV OKEO JMGVX KTZ WMYJBRO PQMGTBV ZMRVA FMQ FMMB ER VTNKO AVEQ MJB CKTJB XKMYJB RMO IV CMRCVQRVB WTOK MQ KEGV OM WMQQA EIMYO FTRERCTEJ TRDYTQTVX RMO IVTRN XYPPJTVB WTOK VRMYNK VEQRTRN OM QVXTBV TR OKVTQ MQTNTREJ KMYXTRN OKV CEQXMRX WVQV QVDYTQVB OM ZMGV FQMZ BVOQMTO OM IMXOMR OKVA ZMGVB TR WTOK HVER ERB WTJJTEZ EGVQA XMRAEX MJBVQ XTXOVQ ERB IQMOKVQ TR JEW TO WEXRO E RMQZEJ MQ TBVEJ JTGTRN XTOYEOTMR IYO TO WEX OKV XVRXV MF FEZTJA YRTOA";

            Dictionary<string, int> chromosome = new Dictionary<string, int>();
            Dictionary<string, int> cipherTriGrams = new Dictionary<string, int>();
            Dictionary<string, int> randTriGrams = new Dictionary<string, int>();
 
            // find all tri-grams for the cipher text
            cipherTriGrams = FindnGrams(3, cipherText);

            // get top tri-grams from dataset
            chromosome = GetTopnGrams(cipherTriGrams.Count);

            // get random tri-grams from dataset
            randTriGrams = GetRandomnGrams(cipherTriGrams.Count);

            // merge random trigrams into chromosome
            foreach (var entry in randTriGrams)
            {
                chromosome[entry.Key] = entry.Value;
            }

            // sort trigrams from cipher by frequency, highest to lowest
            cipherTriGrams = (from entry in cipherTriGrams orderby entry.Value descending select entry)
              .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var entry in cipherTriGrams)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }
            Console.WriteLine('\n');
            int i = 0;
            Console.WriteLine(cipherTriGrams.Count);
            foreach (var entry in chromosome)
            {
                i++;
                Console.WriteLine(i + " " + entry.Key + ':' + entry.Value);
            }         
        } */

        /// <summary>
        /// Generates an initial chromosome to compare against our frequency data.
        /// A chromosome is a potential solution to our problem that we will refine
        /// the initial chromosome is just a random mapping of letters
        /// </summary>
        /// 
        /// <returns>
        /// Dictionary containing the alphabet as keys and random, unique characters as values
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 7:00pm, 5/2/2018 
        /// </author>
        private static Dictionary<char, char> GenerateChromosome(string a_cipherText, Dictionary<string, int> a_dataSet)
        {
            Dictionary<char, char> initialChromosome = new Dictionary<char, char>();
            Dictionary<string, int> cipherTextBiGrams = new Dictionary<string, int>(FindnGrams(2, a_cipherText));
            List<char> alphabet = new List<char>(m_Alphabet);

            HashSet<char> topCipherTextChars = new HashSet<char>(GetCommonLetters(cipherTextBiGrams));
            HashSet<char> topDataSetChars = new HashSet<char>(GetCommonLetters(a_dataSet));
            List<char> topChars = new List<char>(topDataSetChars.ToList());
            Random rand = new Random();
            int randLetter;

            // fill in chromosome with most likely possibilites
            foreach(var letter in topCipherTextChars)
            {
                randLetter = rand.Next(0, topChars.Count() - 1);
                initialChromosome.Add(letter, topChars[randLetter]);
                alphabet.Remove(topChars[randLetter]);
                topChars.RemoveAt(randLetter);
            }

            // fill in rest of chromosome
            for(int i = 0; i < 26;)
            {
                randLetter = rand.Next(0, alphabet.Count);

                // already have this key
                if(initialChromosome.ContainsKey(m_Alphabet[i]))
                {
                    i++;
                    continue;
                }

                // already have this value
                if (initialChromosome.ContainsValue(alphabet[randLetter]))
                {
                    continue;
                }
                // save key and value to new chromosome
                else
                {
                    initialChromosome.Add(m_Alphabet[i], alphabet[randLetter]);
                    i++;
                }
            }
            
            return initialChromosome;
        }

        private static HashSet<char> GetCommonLetters(Dictionary<string, int> a_ngramFrequencies)
        {
            HashSet<char> commonChars = new HashSet<char>();

            // add values to hashset which forces uniqueness
            foreach(var entry in a_ngramFrequencies)
            {
                // save up to 10 of the top letters to use
                if (commonChars.Count >= 10) break;

                // making assumption we are using bigrams
                commonChars.Add(entry.Key[0]);
                commonChars.Add(entry.Key[1]);
            }

            return commonChars;
        }

        /// <summary>
        /// Updates chromosome by switching values between parents or introducing
        /// a random value (or mutation) to the mix in order to change the chromosome
        /// </summary>
        /// 
        /// <param name="a_chromosome"> character mapping/potential solution </param>
        /// <param name="a_charX"> value from parentX to switch </param>
        /// <param name="a_charY"> value from parentY to switch (or a random value if mutating) </param>
        /// 
        /// <returns>
        /// Newly formed child chromosome
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 3:30am, 5/3/2018
        /// </author>
        /*private static Dictionary<char, char> UpdateChromosome(Dictionary<char, char> a_chromosome, char a_charX, char a_charY) 
        {
            Dictionary<char, char> updatedChromosome = a_chromosome;

            char current_charX = a_chromosome[a_charY];
            char current_charY = a_chromosome[a_charX];

            if (current_charX == a_charY)
            {
                current_charX = current_charY;
            }
            else if (current_charY == a_charX)
            {
                current_charY = current_charX;
            }

            updatedChromosome[current_charX] = current_charY;
            updatedChromosome[current_charY] = current_charX;

            updatedChromosome[a_charX] = a_charY;
            updatedChromosome[a_charY] = a_charX;
            
            return updatedChromosome;
        }*/

        /// <summary>
        /// Takes two random items from parentX and fills in the rest of the places from parentY
        /// to create a child chromosome.
        /// </summary>
        /// 
        /// <param name="parentX"> chromosome from population to breed child </param>
        /// <param name="parentY"> chromosome from population to breed child </param>
        /// 
        /// <returns>
        /// Child chromosome made from the entries from the two parents
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 6:00pm, 5/4/2018
        /// </author>
        private static Dictionary<char, char> Crossover(Dictionary<char, char> parentX, Dictionary<char, char> parentY)
        {
            Random rand = new Random();
            string alphabet = m_Alphabet;
            int randLetter;
            Dictionary<char, char> child = new Dictionary<char, char>();
            KeyValuePair<char, char> gene;

            randLetter = rand.Next(0, 13);
            int i = 0;
            // get at least random keys from parentX and add entries to child
            foreach (var entry in parentX)
            {
                if(i == randLetter)
                {
                    gene = entry;
                    child.Add(gene.Key, gene.Value);
                    randLetter = rand.Next(13, 26);
                }
                ++i;
            }


            // fill child with rest of entries from parentY
            foreach(var entry in parentY)
            {
                if (child.ContainsKey(entry.Key)) continue;
                child.Add(entry.Key, entry.Value);
            }
            child = (from entry in child orderby entry.Key ascending select entry)
    .ToDictionary(pair => pair.Key, pair => pair.Value);
            return child;
        }

        /// <summary>
        /// Mutates any duplicated values to make up for any values that were missing from the chromosome
        /// </summary>
        /// 
        /// <param name="a_child"> child chromosome to be mutated </param>
        /// 
        /// <returns>
        /// child chromosome with no duplicated values
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 7:00pm, 5/4/2018
        /// </author>
        private static Dictionary<char, char> Mutation(Dictionary<char, char> a_child)
        {
            var mutatedChild = new Dictionary<char, char>(a_child);

            // find unrepresented characters
            string unrepresentedChars = "";
            foreach(char c in m_Alphabet)
            {
                if (mutatedChild.ContainsValue(c)) continue;
                unrepresentedChars += c;
            }

            // unrepresented characters means there are duplicates
            if(unrepresentedChars != "")
            {
                // find duplicate values
                var duplicates = mutatedChild.GroupBy(x => x.Value).Where(x => x.Count() > 1);

                int i = 0;

                // replace duplicate values with unrepresented chars
                foreach (var entry in duplicates)
                {
                    var toMutate = mutatedChild.FirstOrDefault(x => x.Value == entry.Key).Key;
                    mutatedChild[toMutate] = unrepresentedChars[i];
                    unrepresentedChars.Remove(i);
                    i++;
                }
            }
            // there were no duplicates, mutate a random value
            else
            {
                Random rand = new Random();
                int randLetter = rand.Next(0, 26);
                int randLetter2 = rand.Next(0, 26);
                mutatedChild[m_Alphabet[randLetter]] = m_Alphabet[randLetter2];
                mutatedChild[m_Alphabet[randLetter2]] = m_Alphabet[randLetter];
            }

            return mutatedChild;
        }

        /// <summary>
        /// Handles the changes of the population over to the next generation
        /// using crossover and mutation
        /// </summary>
        /// 
        /// <param name="a_population"> list containing chromosomes </param>
        /// 
        /// <returns>
        /// New population that has changed by one generation
        /// </returns>
        static List<Dictionary<char, char>> NextGeneration(List<Dictionary<char, char>> a_population)
        {

            List<Dictionary<char, char>> newPopulation = new List<Dictionary<char, char>>(a_population);

            Random rand = new Random();

             while(newPopulation.Count < POPULATION_SIZE)
            {
                // randomly select two parent solutions
                var parentX = new Dictionary<char, char>(a_population[rand.Next(a_population.Count)]);
                var parentY = new Dictionary<char, char>(a_population[rand.Next(a_population.Count)]);

                // create a child solution
                var child = new Dictionary<char, char>();

                // switch chromosomes between parents
                for(int i = 0; i < CROSSOVER_COUNT; ++i)
                {
                    child = Crossover(parentX, parentY);
                }

                // randomly mutate chromosomes from child
                for(int i = 0; i < MUTATIONS_COUNT; ++i)
                {
                    child = Mutation(child);
                }

                newPopulation.Add(child);
            }
    
            return newPopulation;
        }

        /// <summary>
        /// This is our selection method. This step involves determining the fitness
        /// of a chromosome to continue based on a score related to its frequency in our
        /// reference data set. We keep the most fit of the population.
        /// </summary>
        /// 
        /// <param name="a_population"> Collection of potential solutions </param>
        /// <param name="a_cipherText"> The text we are trying to decipher </param>
        /// <param name="a_dataSetnGrams"> Reference data from data set </param>
        /// 
        /// <returns>
        /// Dictionary of chromosomes and their respective scores.
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 4:20am, 5/3/2018 
        /// </author>
        static private Tuple<List<Dictionary<char, char>>, int> SelectTopChromosomes(List<Dictionary<char, char>> a_population, string a_cipherText, Dictionary<string, int> a_dataSetnGrams)
        {
            // this is ugly but its just a Dictionary of chromosomes and their respective scores
            var chromosomeScores = new Dictionary<Dictionary<char, char>, int>();  

            // calculate score of each chromosome
            foreach(var chromosome in a_population)
            {
                chromosomeScores.Add(chromosome, ScoreChromosome(DecodeText(a_cipherText, chromosome), a_dataSetnGrams));
            }

            // sort solutions by score
            chromosomeScores = (from entry in chromosomeScores orderby entry.Value descending select entry)
              .ToDictionary(pair => pair.Key, pair => pair.Value);

            // select the highest scored chromosomes
            var topScores = new List<Dictionary<char, char>>();

            int i = 0;
            foreach(var entry in chromosomeScores)
            {
  
                if (i > TOP_POPULATION) break;
                i++;
                topScores.Add(entry.Key);
            }

            var tuple = new Tuple<List<Dictionary<char, char>>, int>(topScores, chromosomeScores.Values.Max());
            return tuple;
        }

        /// <summary>
        /// Scores the chromosome by adding the sum of the frequencies 
        /// in our text * the frequencies in the data set.
        /// </summary>
        /// 
        /// <param name="a_decodedText"> text decoded using a chromosome </param>
        /// <param name="a_dataSetnGrams"> frequences of n-grams from our dataset </param>
        /// 
        /// <returns>
        /// int score of the chromosome
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova 4:30am 5/3/2018
        /// </author>
        static private int ScoreChromosome(string a_decodedText, Dictionary<string, int> a_dataSetnGrams)
        {
            double score = 0;
            Dictionary<string, int> decodedTextBiGrams = FindnGrams(2, a_decodedText);

            // rate each entry and add all of them together to score the chromosome as a whole
            foreach(var entry in decodedTextBiGrams)
            {
                if(a_dataSetnGrams.ContainsKey(entry.Key))
                {
                    score += entry.Value * Math.Log(a_dataSetnGrams[entry.Key], 2);
                }
            }
            return (int)score;
        }

        /// <summary>
        /// Basically the same method as EncodeText from CipherMaker, but instead
        /// searches the value and assigns character to the key
        /// </summary>
        /// 
        /// <param name="a_encodedText"> cipherText to be decoded using a potential solution </param>
        /// <param name="a_cipher"> chromosome/potential solution to test </param>
        /// 
        /// <returns>
        /// text after it has been decoded according to passed in cipher
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 4:00am, 5/3/2018
        /// </author>
        private static string DecodeText(string a_encodedText, Dictionary<char, char> a_cipher)
        {
            string decodedText = "";
            //Console.Clear();
            // decode text using cipher
            foreach(char i in a_encodedText)
            {
                // Get the key of the specified value
                char key = a_cipher.FirstOrDefault(x => x.Value == i).Key;
                decodedText += key;
            }
            //Console.WriteLine(decodedText);
            return decodedText;
        }

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
        static private Dictionary<string, int> GetTrainingBiGrams()
        {
            Dictionary<string, int> trainingBiGrams = new Dictionary<string, int>();
            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetBiGrams.txt";

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
        /// Reads in the most frequent n-grams from our training data to use for 
        /// </summary>
        /// 
        /// <returns>
        /// Dictionary containing the most frequent n-grams from the dataset
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 6:50pm 4/30/2018
        /// </author>
        static Dictionary<string, int> GetTopnGrams(int a_numberOfnGrams)
        {
            Dictionary<string, int> topnGrams = new Dictionary<string, int>();

            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetTriGrams.txt";

            using (FileStream fs = File.Open(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;

                // find specified number of nGrams
                for(int i = 0; i < a_numberOfnGrams/2; ++i)
                {
                    // end of file
                    if ((line = sr.ReadLine()) == null) break;

                    Tuple<string, int> nGramData = ParseDataSetLine(line);
                    topnGrams.Add(nGramData.Item1, nGramData.Item2);
                }
            }
            return topnGrams;
        }

        /// <summary>
        /// Gets random n-grams from the data set to populate the remaining parts of our chromosome
        /// </summary>
        /// 
        /// <param name="a_numberOfnGrams"> total of n-grams needed to complete chromosome </param>
        /// 
        /// <returns>
        /// Dictionary containing the random n-grams
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 1:00am, 5/2/2018
        /// </author>
        static Dictionary<string, int> GetRandomnGrams(int a_numberOfnGrams)
        {
            Dictionary<string, int> randomnGrams = new Dictionary<string, int>();

            // generate random #s which will correspond to a random ngram in our dataset
            Random rand = new Random();
            List<int> randomLines = new List<int>();
            for(int i = 0; i < a_numberOfnGrams/2; ++i)
            {
                randomLines.Add(rand.Next(a_numberOfnGrams / 2, 5000));
            }
            randomLines.Sort();

            string path = "C:\\Users\\icordova\\Source\\Repos\\CipherBreaker\\CipherSolver\\DataSetTriGrams.txt";

            using (FileStream fs = File.Open(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                int lineCount = 0;
                // populate dictionary with random nGrams
                for (int i = 0; i < a_numberOfnGrams; i++)
                {
                    string line;

                    // eof
                    if ((line = sr.ReadLine()) == null) break;
                    lineCount++;

                    // first half of a_numberofnGrams already used for topnGrams
                    if (i < a_numberOfnGrams / 2) continue;

                    // take data from random lines and store it to dictionary
                    if (randomLines.Contains(lineCount))
                    {
                        Tuple<string, int> nGramData = ParseDataSetLine(line);
                        randomnGrams.Add(nGramData.Item1, nGramData.Item2);
                    }
                    // did not add value to dictionary
                    else
                    {
                        --i;
                    }
                }
            }
            return randomnGrams;
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
        private static Tuple<string, int> ParseDataSetLine(string a_line)
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
            }
            nGramCountNum = Int32.Parse(nGramCount);

            Tuple<string, int> nGramData = new Tuple<string, int>(nGram, nGramCountNum);
            return nGramData;
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
