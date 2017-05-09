using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace DataCompression
{
    public interface IEncoder
    {
        void Encode(string file);
    }

    public class FanoShennonEncoder : IEncoder
    {
        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var frequencyTable = SymbolFrequencyCounter.CountFrequenciesForLetters(message);
            
        }
    }

    public class RLEEncoder : IEncoder
    {
        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var messageAfterBW = ApplyBWTransformation(message);
            var messageAfterRLE = string.Empty;

            int increment = 0;
            for (int i = 0; i < message.Length; i += increment)
            {
                var currentCharacter = messageAfterBW.Key.Substring(i, 1);
                increment = 0;
                var nextCharacterPosition = i + increment + 1;
                if (nextCharacterPosition < message.Length)
                {
                    while (messageAfterBW.Key.Substring(nextCharacterPosition, 1) == currentCharacter)
                    {
                        increment++;
                        nextCharacterPosition++;
                        if (nextCharacterPosition >= message.Length)
                        {
                            break;
                        }
                    }
                }
                
                if (increment == 0)
                {
                    messageAfterRLE += currentCharacter;
                    increment++;
                }
                else
                {
                    increment++;
                    messageAfterRLE += currentCharacter + increment;
                }
            }
            MessageFileWriter.WriteEncodedMessageToFile("output1.txt", messageAfterRLE);
        }

        private KeyValuePair<string, int> ApplyBWTransformation(string text)
        {
            var table = GetBWTransformationTable(text);
            var lastCharacters = string.Empty;
            int textInTablePosition = -1;

            for (int i = 0; i < table.Length; i++)
            {
                lastCharacters += table[i].Substring(table[i].Length - 1, 1);
                if (table[i] == text)
                {
                    textInTablePosition = i;
                }
            }
            return new KeyValuePair<string, int>(lastCharacters, textInTablePosition);
        }

        private string[] GetBWTransformationTable(string text)
        {
            var BWTable = new string[text.Length];
            BWTable[0] = text;
            for (int i = 1; i < text.Length; i++)
            {
                BWTable[i] = RotateWordByOneCharacter(BWTable[i - 1]);
            }
            Array.Sort(BWTable);
            return BWTable;
        }

        private string RotateWordByOneCharacter(string word)
        {
            return word.Substring(1, word.Length - 1) + word[0];
        } 
    }

    public class LZ78Encoder : IEncoder
    {
        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var dictionary = new Dictionary<string, int>();
            var encodedMessage = String.Empty;

            var newEntrylength = 1;

            for (int i = 0; i < message.Length; i += newEntrylength)
            {
                newEntrylength = 1;
                var newEntry = message.Substring(i, newEntrylength);
                while (dictionary.ContainsKey(newEntry))
                {
                    newEntrylength++;
                    newEntry = message.Substring(i, newEntrylength);
                }
                if (newEntrylength == 1)
                {
                    if (!dictionary.ContainsKey(newEntry))
                    {
                        dictionary.Add(newEntry, dictionary.Count + 1);
                        encodedMessage += "0" + newEntry;
                    }
                    else
                    {
                        newEntrylength++;
                        encodedMessage += dictionary[newEntry] + message[newEntrylength];
                        dictionary.Add(message.Substring(i, newEntrylength), dictionary.Count + 1);
                    }
                }
                else
                {
                    encodedMessage += dictionary[newEntry.Substring(0, newEntrylength - 1)] +
                                      newEntry.Substring(newEntrylength - 1, 1);
                    dictionary.Add(newEntry, dictionary.Count + 1);
                }
            }
            MessageFileWriter.WriteEncodedMessageToFile("output.txt", encodedMessage);
        }
    }

    internal static class MessageFileWriter
    {
        internal static void WriteEncodedMessageToFile(string fileName, string text)
        {
            var sw = new StreamWriter(fileName);
            sw.WriteLine(text);
            sw.Close();
        }
    }

    internal static class MessageExtractor
    {
        internal static string ExtractMessage(string fileName)
        {
            var sr = new StreamReader(fileName);
            var text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
    }

    internal static class SymbolFrequencyCounter
    {
        internal static SortedDictionary<char, int> CountFrequenciesForLetters(string message)
        {
            var frequencyTable = new SortedDictionary<char, int>();
            foreach (char sym in message)
            {
                if (frequencyTable.ContainsKey(sym))
                {
                    frequencyTable[sym]++;
                }
                else
                {
                    frequencyTable.Add(sym, 1);
                }
            }
            return frequencyTable;
        }
    }
}
