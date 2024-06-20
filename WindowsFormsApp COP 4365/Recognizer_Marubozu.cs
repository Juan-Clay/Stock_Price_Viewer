using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{

    internal class Recognizer_Marubozu : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Marubozu() : base("Marubozu", 1) { }
        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if val already exists
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculates value
            else
            {
                //checks if marubozu
                bool marubozu = scs.bodyRange > (scs.range * 0.96m);
                //adds entry to dict
                scs.Dictionary_Pattern.Add(Pattern_Name, marubozu);
                return marubozu;
            }
        }
    }
}
