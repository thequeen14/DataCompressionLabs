using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression
{
    public interface IDecoder
    {
        void Decode(string file);
    }

    public class HaffmanDecoder
    {
    }

    public class ArythmeticDecoder
    {
        
    }

    public class LZ77Decoder
    {
        
    }

    public class AdaptiveArythmeticDecoder
    {
        
    }
}
