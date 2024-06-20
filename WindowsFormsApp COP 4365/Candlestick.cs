using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Candlestick
    {
        //Inizializes the variable that will hold open
        public decimal open { get; set; }

        //Inizializes the variable that will hold high
        public decimal high { get; set; }

        //Inizializes the variable that will hold low
        public decimal low { get; set; }

        //Inizializes the variable that will hold close
        public decimal close { get; set; }

        //Inizializes the variable that will hold volume, ulong due to possible size of numbers
        public ulong volume { get; set; }

        //Inizializes the variable that will hold the date of  the candlestick
        public DateTime date { get; set; }


        //No parameter constructor for candlesticks
        public Candlestick()
        {
        }

            //Constructor for the candlestick
            public Candlestick(string rowOfData)
        {
            //possible Separators that the data could have
            char[] seperators = new char[] { ',', ' ', '"' };

            //Saves data to a string while separating it at the points that have a value in "seperators"
            string[] subs = rowOfData.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            //Get date string to read the date into datetime format
            string dateString = subs[0];

            //Parse date to get it in datetime format
            date = DateTime.Parse(dateString);

            //Creates temp var that will hold number to check if it is valid for that position
            decimal temp;

            //sets success depending if the open value is a decmial, and sets temp to open val
            bool success = decimal.TryParse(subs[1], out temp);
            //sets candlestick open equal to temp
            if (success) open = temp;

            //sets success depending if the high value is a decmial, and sets temp to high val
            success = decimal.TryParse(subs[2], out temp);
            //sets candlestick high equal to temp
            if (success) high = temp;


            //sets success depnding if the low value is a decmial, and sets temp to low val
            success = decimal.TryParse(subs[3], out temp);
            //sets candlestick low equal to temp
            if (success) low = temp;


            //sets success depnding if the close value is a decmial, and sets temp to close val
            success = decimal.TryParse(subs[4], out temp);
            //sets candlestick close equal to temp
            if (success) close = temp;

            //creates temp variable that holds the volume of data
            ulong tempVolume;
            //sets success depnding if the volume value is a ulong, and sets temp to volume val
            success = ulong.TryParse(subs[6], out tempVolume);
            //sets candlestick volume equal to tempVolume
            if (success) volume = tempVolume;
        }
    }
}
