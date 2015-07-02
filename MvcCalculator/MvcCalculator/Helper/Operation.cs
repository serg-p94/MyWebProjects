using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCalculator.Helper
{
    public enum Operation
    {
        Plus, Minus, Mult, Div
    }

    public static class Translator
    {
        public static string OperationToString(Operation op)
        {
            switch (op)
            {
                case Operation.Plus:
                    return "+";
                case Operation.Minus:
                    return "-";
                case Operation.Mult:
                    return "*";
                case Operation.Div:
                    return "/";
                default:
                    return null;
            }
        }
    }
}