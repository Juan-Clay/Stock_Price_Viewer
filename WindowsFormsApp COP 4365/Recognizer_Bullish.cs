using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Bullish : Recognizer
    {
        //Inherit Constructor from abstract class
        public Recognizer_Bullish() : base("Bullish", 1) { }
        

        //Abstract Method being overridded
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if the pattern exists in dict
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                //return bool of bullish
                return value;
            }
            else
            {
                //checks the candlesticks properties to see if bullish
                bool bullish = scs.close > scs.open;
                //Adds bullish to dict
                scs.Dictionary_Pattern.Add(Pattern_Name, bullish);
                //returns found value
                return bullish;
            }
        }
    }
}
