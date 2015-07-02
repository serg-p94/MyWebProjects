using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MvcCalculator.Helper
{
    public static class StringEncoder
    {
        public static string Encode(string str, Encoding srcEnc, Encoding dstEnc)
        {
            return dstEnc.GetString(srcEnc.GetBytes(str));
        }
    }
}