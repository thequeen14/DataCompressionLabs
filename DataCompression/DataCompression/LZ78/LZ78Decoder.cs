using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.LZ78
{
    public class LZ78Decoder : IDecoder
    {
        public void Decode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var dictionary = new Dictionary<int, string>();
            var decodedMessage = string.Empty;

            int symbolPosition = 0;
            int numberPosition = 0;
            int numberLength = 0;
            int newNumber = 0;

            while (symbolPosition <= message.Length - 1 && numberPosition <= message.Length - 2)
            {
                numberLength = 0;
                while (numberPosition + numberLength <= message.Length - 2 && char.IsDigit(message[numberPosition + numberLength]))
                {
                    numberLength++;
                }

                int.TryParse(message.Substring(numberPosition, numberLength), out newNumber);
                symbolPosition = numberPosition + numberLength;

                if (newNumber == 0)
                {
                    dictionary.Add(dictionary.Count + 1, message[symbolPosition].ToString());
                    decodedMessage += message[symbolPosition];
                }
                else
                {
                    dictionary.Add(dictionary.Count + 1, dictionary[newNumber] + message[symbolPosition]);
                    decodedMessage += dictionary[newNumber] + message[symbolPosition];
                }
                numberPosition = symbolPosition + 1;
            }

            MessageFileWriter.WriteEncodedMessageToFile("output.txt", decodedMessage);
        }
    }
}
