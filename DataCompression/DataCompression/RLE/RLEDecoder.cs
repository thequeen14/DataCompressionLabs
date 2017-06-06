using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.RLE
{
    public class RLEDecoder : IDecoder
    {
        public void Decode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var messageBeforeRLE = ApplyReverseRLETransformation(message);
            var decodedMessage = BuildReverseBWTable(messageBeforeRLE);
            MessageFileWriter.WriteEncodedMessageToFile("output.txt", decodedMessage);
        }

        public string ApplyReverseRLETransformation(string message)
        {
            string newMessage = string.Empty;
            int newCount = 0;
            string newSymbol = string.Empty;
            int j = 0;

            for (int i = 0; i < message.Length - 2; i += j)
            {
                j = 1;
                while (char.IsDigit(message[i + j]))
                {
                    j++;
                }
                int.TryParse(message.Substring(i + 1, j - 1), out newCount);
                newSymbol = message[i].ToString();

                for (int k = 0; k < newCount; k++)
                {
                    newMessage += newSymbol;
                }
            }
            newMessage += message.Substring(message.Length - 3, 2);
            return newMessage;
        }

        public string BuildReverseBWTable(string messageReversedFromRLE)
        {
            int count = 0;
            var tableForInterchange = new string[messageReversedFromRLE.Length - 2];
            while (count < messageReversedFromRLE.Length - 2)
            {
                tableForInterchange[count] = messageReversedFromRLE[count].ToString();
                count++;
            }
            count = 0;
            var position = 0;
            tableForInterchange = SortTable(tableForInterchange);
            while(position < messageReversedFromRLE.Length - 1 - 2)
            {
                for (int m = 0; m < messageReversedFromRLE.Length - 2; m++)
                {
                    tableForInterchange[m] = messageReversedFromRLE[count] + tableForInterchange[m];
                    count++;
                }
                tableForInterchange = SortTable(tableForInterchange);
                count = 0;
                position++;
            }
            var resultPlace = 0;
            int.TryParse(messageReversedFromRLE.Substring(messageReversedFromRLE.Length - 2, 1), out resultPlace);
            var finalResult = tableForInterchange[resultPlace - 1];
            var realFinalResult = finalResult.Reverse();
            var reallyRealFinalResult = string.Empty;
            foreach (char c in realFinalResult)
            {
                reallyRealFinalResult += c.ToString();
            }
            return reallyRealFinalResult;
        }

        public string[] SortTable(string[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 0; j < table.Length - 1; j++)
                {
                    if (Unordered(table[j], table[j + 1]))
                    {
                        string line = table[j];
                        table[j] = table[j + 1];
                        table[j + 1] = line;
                    }
                }
            }

            return table;
        }

        public bool Unordered(string line1, string line2)
        {
            var limit = 0;
            var result = false;
            if (line1.Length > line2.Length)
            {
                limit = line2.Length;
            }
            else
            {
                limit = line1.Length;
            }
            for (int i = 0; i < limit; i++)
            {
                if (line1[i] > line2[i])
                {
                    result = true;
                    return result;
                }
                else if (line1[i] < line2[i])
                {
                    return result;
                }
            }

            return result;
        }
    }
}
