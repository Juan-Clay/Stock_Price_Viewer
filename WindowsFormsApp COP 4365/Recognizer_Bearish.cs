using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Bearish : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Bearish() : base("Bearish", 1) { }
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
            //calculates value 
            else
            {
                //checks if bearish 
                bool bearish = scs.open > scs.close;
                //sets key in dict with value
                scs.Dictionary_Pattern.Add(Pattern_Name, bearish);

                return bearish;
            }
        }
    }
}
