using System;
using System.Collections.Generic;
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
                var newEntry = message.Substring(i, newEntrylength);
                while (dictionary.ContainsKey(newEntry))
                {
                    newEntrylength++;
                    newEntry = message.Substring(i, newEntrylength);
                }
                if (newEntry)
                if (newEntrylength == 1 && !dictionary.ContainsKey(newEntry))
                {
                    dictionary.Add(newEntry, dictionary.Count + 1);
                    encodedMessage += newEntry;
                }
                else
            }
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
