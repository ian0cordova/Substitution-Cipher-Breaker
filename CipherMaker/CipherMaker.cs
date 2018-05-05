/// CipherMaker.cs
/// Ian Cordova
/// Senior Project - CipherBreaker

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cipher.CipherMaker
{
    /// <summary>
    /// Handles the creation and translation of substitution ciphers.
    /// </summary>
    public class CipherMaker
    {
        /// <summary>
        /// String which contains the alphabet.
        /// </summary>
        private static readonly string m_Alphabet;

        /// <summary>
        /// Static constructor to initialize alphabet. It is invoked before the first instance constructor is run.
        /// </summary>
        static CipherMaker()
        {
            m_Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        /// <summary>
        /// Creates a random substitution Cipher
        /// </summary>
        /// 
        /// <returns>
        /// subCipher - Dictionary containing alphabet as keys mapped to their random, ciphered values.
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 11:30pm, 4/21/2018
        /// </author>
        public static Dictionary<char, char> CreateSubCipher()
        {
            Dictionary<char, char> subCipher = new Dictionary<char, char>();
            Random rand = new Random();
            string alphabet = m_Alphabet;
            int randLetter;

            // iterate through alphabet
            for(int i = 0; i < 26; ++i)
            {
                // generate a random letter and check if it has already been used.
                randLetter = rand.Next(0, alphabet.Length);
                subCipher.Add(alphabet[i], alphabet[randLetter]);
                Debug.WriteLine(subCipher[alphabet[i]]);
            }
            return subCipher;
        } 

        /// <summary>
        /// Checks if the dictionary has not assigned duplicate letters
        /// </summary>
        /// 
        /// <param name="a_cipher"> Dictionary containing existing cipher so-far (haystack) </param>
        /// <param name="a_value"> Value to check if exists in cipher (needle) </param>
        /// 
        /// <returns>
        /// True, if it exists in dictionary, otherwise false.
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova 11:30pm, 4/21/2018
        /// </author>
        public static bool CipherContains(Dictionary<char, char> a_cipher, char a_value)
        {
            // iterate through dictionary
            foreach (KeyValuePair<char, char> entry in a_cipher)
            {
                // if value already exists in dictionary, return true
                if(entry.Value == a_value)
                {
                    return true;
                }
            }
            // value does not exist in dictionary
            return false;
        }

        /// <summary>
        /// Converts plain text to ciphered text based on provided cipher.
        /// </summary>
        /// 
        /// <param name="a_plainText"> English text to be encoded </param>
        /// <param name="a_cipher"> Cipher which determines how text will be encoded </param>
        /// 
        /// <returns>
        /// encodedText - text which has been converted from english to ciphertext
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova 11:50pm, 4/21/2018
        /// </author>
        public static string EncodeText(string a_plainText, Dictionary<char, char> a_cipher)
        {
            string encodedText = "";
            string cleanText = CleanString(a_plainText);

            // iterate through plaintext
            foreach(char i in cleanText)
            {
                if (i == ' ' || i == '\n')
                {
                    encodedText += i;
                    continue;
                }
                // assign value to its ciphered, capital value
                encodedText += a_cipher[Char.ToUpper(i)];
            }
            return encodedText;
        }
        
        /// <summary>
        /// Removes all punctuation and non-needed characters from the string.
        /// </summary>
        /// 
        /// <param name="a_text"> string to be cleaned </param>
        /// 
        /// <returns>
        /// Original string removed of unnecessary characters.
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 9:00pm, 4/23/2018
        /// </author>
        private static string CleanString(string a_text)
        {
            string cleanString = "";
            foreach(char i in a_text)
            {
                if (IsEnglishLetter(i) || i == ' ' || i == '\n')
                {
                    cleanString += i;
                }
            }
            return cleanString;
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
