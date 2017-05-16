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
}
