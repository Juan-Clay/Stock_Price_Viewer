using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Dragonfly_Doji : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Dragonfly_Doji() : base("Dragonfly Doji", 1) { }
        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if key exists in key 
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculates the value manually
            else
            {
                //checks if it is a candle stick
                bool dragonfly = scs.lowerTail > (scs.range * 0.66m);
                //checks if it is a doji
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                //ands to check if both
                bool dragonfly_doji = dragonfly & doji;
                //adds key and val to the dict 
                scs.Dictionary_Pattern.Add(Pattern_Name, dragonfly_doji);
                return dragonfly_doji;
            }
        }
    }
}
