using System;
using System.Collections.Generic;

using System.IO;

namespace DataCompression
{
    public interface IEncoder
    {
        void Encode(string file);
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
