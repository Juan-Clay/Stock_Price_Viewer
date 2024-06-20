using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Bearish_Harami : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Bearish_Harami() : base("Bearish Harami", 2) { }
        //Abstract Method to be overridden 
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if key exists in dict 
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculates pattern 
            else
            {
                //calculates offset 
                int offset = Pattern_Length / 2;
                //checks if it could be true
                if (index < offset)
                {
                    //sets dict entry to false
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                else
                {
                    //gets the prev candlesticks 
                    SmartCandlestick prev = scsList[index - offset];
                    //checks if could be bearish 
                    bool bearish = (prev.open < prev.close) & (scs.close < scs.open);
                    //checks if could be harami 
                    bool harami = (scs.topPrice < prev.topPrice) & (scs.bottomPrice > prev.bottomPrice);
                    //ands them to see if candle stick is the pattern 
                    bool bearish_harami = bearish & harami;
                    //adds key to the dict with found value
                    scs.Dictionary_Pattern.Add(Pattern_Name, bearish_harami);
                    return bearish_harami;
                }
            }
        }
    }
}
