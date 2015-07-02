using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcCalculator.Helper;

namespace MvcCalculator.Models
{
    public class HomeCalculateViewModel
    {
        public double v1 { get; set; }
        public double v2 { get; set; }
        public Operation operation { get; set; }
     
        public HomeCalculateViewModel(double v1, double v2, Operation op)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.operation = op;
        }

        public double? GetResult()
        {
            switch (operation)
            {
                case Operation.Plus:
                    return v1 + v2;
                case Operation.Minus:
                    return v1 - v2;
                case Operation.Mult:
                    return v1 * v2;
                case Operation.Div:
                    return v1 / v2;
                default:
                    return null;
            }
        }
    }
}