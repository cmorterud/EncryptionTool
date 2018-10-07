using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionToolLib
{
    public interface IVerify
    {
        bool verify(byte[] text, byte[] key, byte[] signature);
        byte[] computeMAC(byte[] text, byte[] key);
    }
}
