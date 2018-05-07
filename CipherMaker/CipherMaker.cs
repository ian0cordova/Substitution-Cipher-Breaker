/// CipherMaker.cs
/// Ian Cordova
/// Senior Project - CipherBreaker

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cipher.CipherMaker
{
    using Cipher.CipherUtility;
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
            List<char> alphabet = new List<char>(m_Alphabet);
            int randLetter;

            // iterate through alphabet
            for(int i = 0; i < 26; ++i)
            {
                // generate a random letter and check if it has already been used.
                randLetter = rand.Next(0, alphabet.Count - 1);
                subCipher.Add(m_Alphabet[i], alphabet[randLetter]);
                alphabet.RemoveAt(randLetter);
            }
            return subCipher;
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
                if (CipherUtility.IsEnglishLetter(i) || i == ' ' || i == '\n')
                {
                    cleanString += i;
                }
            }
            return cleanString;
        }
    }
}
