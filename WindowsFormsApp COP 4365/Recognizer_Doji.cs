using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Recognizer_Doji : Recognizer
    {
        //Inherit Constructor
        public Recognizer_Doji() : base("Doji", 1) { }
      

        //Abstract Method to be overridden
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            //Return existing value or calculate
            SmartCandlestick scs = scsList[index];
            //checks if key exists in the dict
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            //calculates valuie
            else
            {
                //calculates if the value could be a doji
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                //inserts key into dict
                scs.Dictionary_Pattern.Add(Pattern_Name, doji);
                return doji;
            }
        }
    }
}
