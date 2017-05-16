using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.FanoShennon
{
    public class FanoShennonEncoder : IEncoder
    {
        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var frequencyTable = SymbolFrequencyCounter.CountFrequenciesForLetters(message);

        }
    }
}
