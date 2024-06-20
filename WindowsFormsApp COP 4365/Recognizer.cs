using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{

    //USE RECTNANGLE ANNOTATION TO SHOW MULTI CANDLESTICK PATTERNS, DONT 
    internal abstract class Recognizer
    {

        //Pattern name
        public string Pattern_Name;
        //Pattern length
        public int Pattern_Length;

        //constructor
        public Recognizer(string pN, int pL) 
        {
            Pattern_Name = pN;
            Pattern_Length = pL;
        }

        //Abstract method that must be filled out 
        public abstract bool Recognize(List<SmartCandlestick> lcs, int index);
        
        //Method that every recognizer will have to recognize a whole list of candlesticks
        public void RecognizeAll(List<SmartCandlestick> lcs)
        {
            for (int i = 0; i < lcs.Count; i++) 
            {
                Recognize(lcs, i);
            }
        }

    }
}
