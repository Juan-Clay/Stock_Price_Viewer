using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Peak : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Peak() : base("Peak", 3) { }
        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //Checks if key exists
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculates value
            else
            {
                //calculates offset
                int offset = Pattern_Length / 2;
                //Checks if out of bounds
                if ((index < offset) | (index == scsList.Count() - offset))
                {
                    //adds key value as false
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                //value could be true
                else
                {
                    //gets prev candlesticks
                    SmartCandlestick prev = scsList[index - offset];
                    //gets next candlesticks
                    SmartCandlestick next = scsList[index + offset];
                    //checks if middle candlestick is a peak 
                    bool peak = (scs.high > prev.high) & (scs.high > next.high);
                    //makes entry in dict
                    scs.Dictionary_Pattern.Add(Pattern_Name, peak);
                    return peak;
                }
            }
        }
    }
}
