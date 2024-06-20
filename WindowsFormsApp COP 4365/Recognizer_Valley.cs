using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Valley : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Valley() : base("Valley", 3) { }
       

        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //checks to either return existing value or calculate
            //Gets candlestick from list
            SmartCandlestick scs = scsList[index];
            //Tries to get val from dict
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            else
            {
                //calculate offset
                int offset = Pattern_Length / 2;
                //if out of bounds
                if ((index < offset) | (index == scsList.Count() - offset))
                {
                    //write to dict the pattern and val
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                //not out of bounds
                else
                {
                    //gets prev candlestics
                    SmartCandlestick prev = scsList[index - offset];
                    //gets next candlesticks
                    SmartCandlestick next = scsList[index + offset];
                    //checks if they are a valley by comparing lows of adjacent candlesticks
                    bool valley =  (scs.low < next.low) & (scs.low < prev.low);
                    //adds key and val to candlestick
                    scs.Dictionary_Pattern.Add(Pattern_Name, valley);
                    //return val
                    return valley;
                }
            }
        }
    }
}
