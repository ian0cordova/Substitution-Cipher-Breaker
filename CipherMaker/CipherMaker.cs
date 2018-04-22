/// CipherMaker.cs
/// Ian Cordova
/// Senior Project - CipherBreaker

using System;
using System.Collections.Generic;

namespace CipherMaker
{
    /// <summary>
    /// Handles the creation and translation of substitution ciphers.
    /// </summary>
    public class CipherCreator
    {
        /// <summary>
        /// String which contains the alphabet.
        /// </summary>
        private static readonly string m_Alphabet;

        /// <summary>
        /// Static constructor to initialize alphabet. It is invoked before the first instance constructor is run.
        /// </summary>
        static CipherCreator()
        {
            m_Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CipherCreator() { }

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
        public Dictionary<char, char> CreateSubCipher()
        {
            Dictionary<char, char> subCipher = new Dictionary<char, char>();
            Random rand = new Random();
            int randLetter;

            // iterate through alphabet
            for(int i = 0; i < 26; ++i)
            {
                // generate a random letter and check if it has already been used.
                do randLetter = rand.Next(0, 25); while ((CipherContains(subCipher, m_Alphabet[randLetter])));
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
        public bool CipherContains(Dictionary<char, char> a_cipher, char a_value)
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
        public string EncodeText(string a_plainText, Dictionary<char, char> a_cipher)
        {
            string encodedText = "";

            // iterate through plaintext
            foreach(char i in a_plainText)
            {
                // assign value to its ciphered, capital value
                encodedText += a_cipher[Char.ToUpper(i)];
            }
            return encodedText;
        }

    }
}
