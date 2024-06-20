using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Bearish_Engulfing : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Bearish_Engulfing() : base("Bearish Engulfing", 2) { }

        //Abstract Method to be overridden 
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if val exists in dict 
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculate value 
            else
            {
                //calculates offset to determine if out of bounds
                int offset = Pattern_Length / 2;
                //check if out of bounds 
                if (index < offset)
                {
                    //creates dict entry and sets false
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                //could be true
                else
                {
                    //gets previous candlesticks
                    SmartCandlestick prev = scsList[index - offset];
                    //checks if pattern is engulfing 
                    bool engulfing = (scs.topPrice > prev.topPrice) & (scs.bottomPrice < prev.bottomPrice);
                    //checks if the pattern is bearish 
                    bool bearish = (prev.open < prev.close) & (scs.close < scs.open);  
                    //ands them to check if true
                    bool bearish_engulfing = bearish & engulfing;
                    //adds entry to dict
                    scs.Dictionary_Pattern.Add(Pattern_Name, bearish_engulfing);
                    return bearish_engulfing;
                }
            }
        }
    }
}
