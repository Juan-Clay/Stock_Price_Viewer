using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{

    internal class Recognizer_Bullish_Engulfing : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Bullish_Engulfing() : base("Bullish Engulfing", 2) { }
        

        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if key exists
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculates val since it doenst exist
            else
            {
                //calculate offset
                int offset = Pattern_Length / 2;
                //checks if it is out bounds
                if (index < offset)
                {
                    //makes dict entry
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                else
                {
                    //gets prev candlesticks
                    SmartCandlestick prev = scsList[index - offset];
                    //checks if pattern could be bullish
                    bool bullsih = (prev.open > prev.close) & (scs.close > scs.open);
                    //checks if pattern is engulfing by comparing top and bottom prices
                    bool engulfing = (scs.topPrice > prev.topPrice) & (scs.bottomPrice < prev.bottomPrice);
                    //ands them together to get result
                    bool bullish_engulfing = bullsih & engulfing;
                    //add entry to the dict
                    scs.Dictionary_Pattern.Add(Pattern_Name, bullish_engulfing);
                    return bullish_engulfing;
                }
            }
        }
    }
}
