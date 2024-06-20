using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Bullish_Harami : Recognizer
    {
        //Inherit Constructor from abstract
        public Recognizer_Bullish_Harami() : base("Bullish Harami", 2) { }
       

        //Abstract Method
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if key already exists in candlestick dict
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                //returns val
                return value;
            }
            //calculates value
            else
            {
                //Return false if out of bounds or continue to calculation
                int offset = Pattern_Length / 2;
                //checks if its possible for pattern to exist, if not returns false
                if (index < offset)
                {
                    //Adds key and val to dict
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
               
                    return false;
                }
                //If pattern could exist
                else
                {
                    //gets prev candlesticks info
                    SmartCandlestick prev = scsList[index - offset];

                    //Checks if pattern could be bullish
                    bool bullsih = (prev.open > prev.close) & (scs.close > scs.open);
                    //Checks if pattern could be harami
                    bool harami = (scs.topPrice < prev.topPrice) & (scs.bottomPrice > prev.bottomPrice);
                    //Ands them to get result
                    bool bullish_harami = bullsih & harami;
                    //Adds key and value to candlestick dict
                    scs.Dictionary_Pattern.Add(Pattern_Name, bullish_harami);
                    //return val
                    return bullish_harami;
                }
            }
        }
    }
}
