using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Hammer : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Hammer() : base("Hammer", 1) { }
      

        //Abstract Method to be overricden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if key exists
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //key doesnt exist, calculate
            else
            {
                //Checks pattern by comparing values and ranges of candlesticks, values through trial and error
                bool hammer = ((scs.range * 0.20m) < scs.bodyRange) & (scs.bodyRange < (scs.range * 0.33m)) & (scs.lowerTail > scs.range * 0.66m);
                //adds key to dict
                scs.Dictionary_Pattern.Add(Pattern_Name, hammer);
                //return val
                return hammer;
            }
        }
    }
}
