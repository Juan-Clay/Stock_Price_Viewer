using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class SmartCandlestick : Candlestick
    {
        //range is the range between the top and bottom prices
        public decimal range { get; set; }
        //Top price is what the higher price was between open and close
        public decimal topPrice { get; set; }
        //Bottom price is what the lower price was between open and close 
        public decimal bottomPrice { get; set; }
        //body range is the range of the opening and closing prices
        public decimal bodyRange { get; set; }
        //upper tail is the range between the high and the top price 
        public decimal upperTail { get; set; }
        //lower tail is range of bottom price and low
        public decimal lowerTail { get; set; }


        //Dynamic dictonary that will hold where all the patterns for that specific candlestick will go 
        public Dictionary<string, bool> Dictionary_Pattern = new Dictionary<string, bool>();


        /// <summary>
        /// A constructor for smart candlesticks that takes in an input string to first make a normal candlestick through the candlestick constructor
        /// </summary>
        /// <param name="rowOfData">
        /// Data string that contains the BASIC info of a candlestick 
        /// </param>
        public SmartCandlestick(string rowOfData) : base(rowOfData)
        {
            //calls the function to compute the extra properties of the candlestick
            ComputeExtraProperties();
            //computes the patterns of the candlestick
            ComputePatternProperties();
        }

        /// <summary>
        /// This constructor function takes in a normal candlestick and makes a smart candle stick out of it 
        /// </summary>
        /// <param name="cs">
        /// The name of the normal candlestick and is used to call its member variables
        /// </param>
        public SmartCandlestick(Candlestick cs)
        {
            //grabs the date from the og candlestick and puts it to the smart candlestick
            date = cs.date;
            //grabs the open from the og candlestick and puts it to the smart candlestick
            open = cs.open;
            //grabs the close from the og candlestick and puts it to the smart candlestick
            close = cs.close;
            //grabs the high from the og candlestick and puts it to the smart candlestick
            high = cs.high;
            //grabs the low from the og candlestick and puts it to the smart candlestick
            low = cs.low;
            //grabs the volume from the og candlestick and puts it to the smart candlestick
            volume = cs.volume;
            //gets the extra properties using the properties just got
            ComputeExtraProperties();
        }

        /// <summary>
        /// Computes the extra properties that will be stored in the candlestick 
        /// </summary>
        private void ComputeExtraProperties()
        {
            //range is the range between the top and bottom prices
            range = high - low;
            //Top price is what the higher price was between open and close
            topPrice = Math.Max(open, close);
            //Bottom price is what the lower price was between open and close 
            bottomPrice = Math.Min(open, close);
            //body range is the range of the opening and closing prices
            bodyRange = topPrice - bottomPrice;
            //upper tail is the range between the high and the top price 
            upperTail = high - topPrice;
            //lower tail is range of bottom price and low
            lowerTail = bottomPrice - low;
        }

        /// <summary>
        /// Computes the pattern properties dersired for the candle stick 
        /// </summary>
        private void ComputePatternProperties()
        {
            //checks if open is lower than close
            bool bullish = close > open;
            //adds bullish to the dictionary
            Dictionary_Pattern.Add("Bullish", bullish);

            //checks if open is greater than close
            bool bearish = open > close;
            //adds bearish to the dictionary
            Dictionary_Pattern.Add("Bearish", bearish);

            //checks if body range is less than 20 percent of range
            bool neutral = bodyRange < (range * 0.2m);
            //adds neutral to the dictionary
            Dictionary_Pattern.Add("Neutral", neutral);

            //check if the range is less than or equal to 105% of the body range
            bool marubozu = (bodyRange * 1.05m) >= range;
            //adds marubozu to the dictionary
            Dictionary_Pattern.Add("Marubozu", marubozu);

            //checks if the body range is less than 25 percent of range AND the lower tail is greater than 67 percent of the range
            bool hammer = (bodyRange < range * 0.25m) & (lowerTail > range * 0.67m);
            //adds hammer to the dictionary
            Dictionary_Pattern.Add("Hammer", hammer);

            //checks if body range is less than or equal to the 1 percent of the range
            bool doji = bodyRange <= (range * 0.01m);
            //adds doji to the dictionary
            Dictionary_Pattern.Add("Doji", doji);

            //if it is a doji, and the lower tail is greater than 67 percent of the range
            bool dragonfly_doji = doji & (lowerTail > range * 0.67m);
            //adds dragonfly doji to the dictionary
            Dictionary_Pattern.Add("Dragonfly Doji", dragonfly_doji);

            //if it is a doji, and the upper tail is greater than 67 percent of the range
            bool gravestone_doji = doji & (upperTail > range * 0.67m);
            //adds gravestone doji to the dictionary
            Dictionary_Pattern.Add("Gravestone Doji", gravestone_doji);
        }


    }
}
