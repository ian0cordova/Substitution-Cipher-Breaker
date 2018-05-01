/// CipherUtility.cs
/// Ian Cordova
/// Senior Project

using System;
using System.IO;
using System.Linq;

namespace Cipher.CipherUtility
{
    /// <summary>
    /// This class provides some random functionality needed by the UWP application such as file access.
    /// </summary>
    public class CipherUtility
    {
        /// <summary>
        /// Copies text from txt file to a string and sends it back to caller
        /// </summary>
        /// 
        /// <param name="a_filePath"> name of file to be read </param>
        /// 
        /// <returns>
        /// string plainText which contains text from txt file
        /// </returns>
        public static string LoadPlainText(string a_filePath)
        {
            string plainText = "";

            using (FileStream fs = new FileStream("C:\\Users\\icordova\\Documents\\test.txt", FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    plainText += line;
                }
            }
            return plainText;
        }

        /// <summary>
        /// Saves generated cipher text to a file.
        /// </summary>
        /// 
        /// <param name="a_cipherText"> text to be saved </param>
        /// <param name="a_filePath"> specified file path to save text </param>
        /// 
        /// <author>
        /// Ian Cordova - 8:00pm, 4/30/2018
        /// </author>
        public static void SaveCipherText(string a_cipherText, string a_filePath = "")
        {
            if(a_filePath == "")
            {
                a_filePath = RandomString();
            }
            using (StreamWriter cipherText = new StreamWriter(a_filePath))
            {
                cipherText.WriteLine(a_cipherText);
            }
        }

        /// <summary>
        /// Creates a randomized string, i use this if a person doesn't specify a file name
        /// used code from https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        /// </summary>
        /// 
        /// <returns>
        /// random 8 character long string
        /// </returns>
        /// 
        /// <author>
        /// User Iaika on StackOverFlow from above link
        /// </author>
        private static string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
