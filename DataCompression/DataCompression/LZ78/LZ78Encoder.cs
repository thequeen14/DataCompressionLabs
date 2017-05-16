using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.LZ78
{
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
}
