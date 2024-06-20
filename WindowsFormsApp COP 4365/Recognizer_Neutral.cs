using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Neutral : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Neutral() : base("Neutral", 1) { }
      

        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //check if val exists in dict
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculate value
            else
            {
                //Compares body range to the 4 percent of the range
                bool neutral = scs.bodyRange < (scs.range * 0.04m);
                //adds key and val to dict
                scs.Dictionary_Pattern.Add(Pattern_Name, neutral);
                //return val
                return neutral;
            }
        }
    }
}
