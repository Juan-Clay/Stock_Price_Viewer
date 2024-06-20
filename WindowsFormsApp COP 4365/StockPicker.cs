using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



//JUAN LAYRISSE, U43230984
namespace WindowsFormsApp_COP_4365
{

    /// <summary>
    /// Main class that holds all the functions for the buttons and visuals on the app
    /// </summary>
    public partial class Form_Project_1 : Form
    {
        //List of all candlesticks read from file
        private List<SmartCandlestick> candlesticks = null;
        //Bound list of candlesticks that is tied to the graph and table to change 
        private BindingList<SmartCandlestick> bcandlestick = null;
        //The list after checking what the desired range is 
        private List<SmartCandlestick> filteredList = null;


        private Dictionary<string, Recognizer> Recognizer_Dict;

        
        //Global variable for the start and end date of the period
        private DateTime startDate = DateTime.Parse("1/1/2022");
        //End variable for the given period
        private DateTime endDate = DateTime.Now;

        //Highest total chart value
        private double chartMax;
        //Lowest total chart value
        private double chartMin;



        /// <summary>
        /// Constructor for the app
        /// </summary>
        public Form_Project_1()
        {
            //Initializes the program
            InitializeComponent();
            //Initializes the recognizers
            InitializeRecognizer();
            //Create the candlesticks and set the max size at 2048, just to be sure everything fits 
            candlesticks = new List<SmartCandlestick>(2048);
        }



        /// <summary>
        /// Child Form Constructor for the other windows that will appear 
        /// </summary>
        /// <param name="stockPath">"File path of the child form (2 - n file selection)"
        /// Path for the next csv file
        /// </param>
        /// <param name="start">
        /// The current selected start date for the period
        /// </param>
        /// <param name="end">"
        /// The current selected start date for the period
        /// </param>
        public Form_Project_1(string stockPath, DateTime start, DateTime end)
        {
            //Initializes the program
            InitializeComponent();
            //Initializes the recognizers
            InitializeRecognizer();

            //Set date from parent form
            //Set start
            dateTimePickerStart.Value = startDate = start;
            //Set end
            dateTimePickerEnd.Value = endDate = end;
            //Read file of child
            candlesticks = goReadFile(stockPath);
            //Filter list of child
            filter_candlesticks();
            //Display data of child
            show_candleSticks();
        }




        /// <summary>
        /// The function called whenenever the openfile button is pressed, takes default arguments for a action call in forms 
        /// The function just pops up the file explorer to find the csv file desired
        /// </summary>
        /// 
        /// <param name="sender">
        /// the object that was used to trigger the event, button
        /// </param>
        /// <param name="e">
        /// The event that fired when the action was performed
        /// </param>
        private void button_openFile_Click(object sender, EventArgs e)
        {
            //Changes the title on the window to signify we are opening file
            Text = "Opening File...";
            //The command to open up the file explorer
            openFileDialog_stockPick.ShowDialog();
        }





        /// <summary>
        /// The function called whenenever the update button is pressed, takes default arguments for a action call in forms
        /// Mainly used when changing the range of the data, only just changes the table and graphs
        /// </summary>
        /// 
        /// <param name="sender">
        /// the object that was used to trigger the event, button
        /// </param>
        /// <param name="e">
        /// The event that fired when the action was performed
        /// </param>
        private void button_update(object sender, EventArgs e)
        {
            //Checking that there was both a list that was selected AND the dates selected are valid
            if ((candlesticks.Count != 0) & (startDate <= endDate))
            {
                //Runs the list through the new filters
                filter_candlesticks();
                //Updates the table and graphs
                show_candleSticks();
            }
        }






        /// <summary>
        /// Used to start the display process to the form 
        /// </summary>
        private void openFileDialog_stockPick_FileOk_2()
        {
            //Parses the file to put all the  data in a list
            goReadFile();
            //Takes the list from the file and filters it, when first opening file, default start is 1/1/2022
            filter_candlesticks();
            //Updates the visuals on the table and the graph
            show_candleSticks();

        }


        ///<summary>
        ///This is the function that is called when one presses okay in the file explorer, arguments are default 
        /// Performs all functions to get the new list and display it
        /// </summary>
        /// 
        /// <param name="sender">
        /// the object that was used to trigger the event, button</param>
        /// <param name="e">
        /// The event that fired when the action was performed
        /// </param>
        private void openFileDialog_stockPick_FileOk(object sender, CancelEventArgs e)
        {
            //Sets the amount of windows that will be opened after the ok is pressed 
            int windows = openFileDialog_stockPick.FileNames.Count();
            //Loop for each new file and data that will be displayed in the new windows 
            for (int i = 0; i < windows; ++i)
            {
                //Get the pathname of current file
                string pathName = openFileDialog_stockPick.FileNames[i];
                //PAth name without extension
                string ticker = Path.GetFileNameWithoutExtension(pathName);

                //Create new form window
                Form_Project_1 form_StockViewer;
                //So that the first form is set to parent
                if (i == 0)
                {
                    //Read the file and display the stock
                    form_StockViewer = this;
                    //Uses orginal function to start the reading of the file
                    openFileDialog_stockPick_FileOk_2();
                    //sets the title of the window to parent
                    form_StockViewer.Text = "Parent: " + ticker;
                }
                //For the rest of the new forms, that will be children
                else
                {
                    //Instantiate new form with dates
                    form_StockViewer = new Form_Project_1(pathName, startDate, endDate);
                    //Set title to child
                    form_StockViewer.Text = "Child: " + ticker;
                }

                //Display the new form
                form_StockViewer.Show();
                //Brings the parent to the front of the screen, ahead of the children 
                form_StockViewer.BringToFront();
            }



        }



        /// <summary>
        /// This function takes the name of the file and loops through it using the Streamreader to input the data into a list
        /// </summary>
        /// 
        /// <param name="filename">
        /// Passes the name of the file that was selected in the open file dialoge
        /// </param>
        /// 
        /// <returns>
        /// returns a list that contains ALL candlesticks in the data
        /// </returns>
        private List<SmartCandlestick> goReadFile(string filename)
        {
            //Makes a reference string to get the correct data in the correct order
            const string referenceString = "Date,Open,High,Low,Close,Adj Close,Volume";
            //Makes the name of the window the name of the file
            this.Text = Path.GetFileName(filename);

            //The list that will be filled with the info and returned in the function
            List<SmartCandlestick> resultingList = new List<SmartCandlestick>();
            //Pass file path and filename to the StreamReader 
            using (StreamReader sr = new StreamReader(filename))
            {
                //Turns the first line to a string
                string line = sr.ReadLine();
                //checsks if the first line is equal to the reference string, its the header of csv file
                if (line == referenceString)
                {
                    //Loop to run through the whole file
                    while((line = sr.ReadLine()) != null)
                    {
                        //create a candlestick using the data from the current line
                        SmartCandlestick cs = new SmartCandlestick(line);
                        //Adds the newly created candlestick to the results list
                        resultingList.Add(cs);
                    }
                    //Reverse the list in order to show the most recent candle sticks first 
                    resultingList.Reverse();
                }
                //case if the first string is not the header of a csv file, makes it so that the title says bad file
                else
                { 
                    //Changing the name of the window
                    Text = "Bad File: " + filename;
                }
                //IMPORTANT: Set the start date as 1/1/2022, some files only start up on january 3rd, likely due to not existing on the first
                dateTimePickerStart.Value = startDate = DateTime.Parse("1/1/2022");
                //Todays date is used for the last date
                dateTimePickerEnd.Value = endDate = resultingList.First().date;
            }


            //Run all Recognizers on list
            foreach (Recognizer r in Recognizer_Dict.Values)
            {
                //Adds dictionary entries for every pattern on every candlestick
                r.RecognizeAll(resultingList);
            }


            //Return the list with all the candlesticks from the file
            return resultingList;
        }

        /// <summary>
        /// The goReadFile that takes no parameters and doesnt return anything, used for convienance and uses the 
        /// Original function while passing the correct parameters
        /// </summary>
        private void goReadFile()
        {
            //sets the global candlesticks list to the parsed list from the goReadFile, taking the name of the file
            //From the openFileDialog
            candlesticks = goReadFile(openFileDialog_stockPick.FileName);
            //Uses the Binding list of candlesticks to use later, same as candlesticks
            bcandlestick = new BindingList<SmartCandlestick>(candlesticks);
        }


        /// <summary>
        /// This function takes in the full list of candlesticks from the file, the current desired start and end time,
        /// And saves the desired range in a new list that is returend
        /// </summary>
        /// 
        /// <param name="big_list"></param>
        /// The list that has all the values from the file, big list because it will likely be bigger than the resulting list
        /// <param name="start"></param>
        /// start day for the filter
        /// <param name="end"></param>
        /// end date for the filter
        /// <returns>
        /// Returns a filtered version of the list, using the start and end dates as bounds
        /// </returns>
        private List<SmartCandlestick> filter_candlesticks(List<SmartCandlestick> big_list, DateTime start, DateTime end)
        {
            //creates the list that will likely be smaller than the original, using the datetime pickers as bounds
            List<SmartCandlestick> res = new List<SmartCandlestick>(big_list.Count);
            //Loop through each candlestick in list
            foreach (SmartCandlestick cs in big_list)
            {
                //Checks if current candlesticks date is within the desried range
                if ((cs.date >= start) & (cs.date <= end))
                { 
                    //Actually adds the candlestick to the smaller list
                    res.Add(cs); 
                }
            }
            //returns the filtered list
            return res;
        }


        ///<summary>
        ///Simple function of filter_candlesticks, makes it so that it calls the other version with proper arguments <summary>
        /// </summary>
        private void filter_candlesticks()
        {
            //Saves the list from filter_candlesticks into the global variable
            filteredList = filter_candlesticks(candlesticks, startDate, endDate);
            //saves over the original binding list with the filtered list since it will be the thing controlling the visuals
            bcandlestick = new BindingList<SmartCandlestick>(filteredList);
            //Update the combo box with all the values that appear in the current list
            updateComboBox();
        }


        /// <summary>
        /// The display function that takes the binding list and makes it so the graph and table are updated
        /// </summary>
        /// 
        /// <param name="boundList">
        /// Takes a binding list that is the same as the filtered list
        /// </param>
        private void show_candleSticks(BindingList<SmartCandlestick> boundList)
        {
            //Makes the table's data source come from the binding list
            //dataGridView_Candlestick.DataSource = boundList;

            //This function makes it so that the graph has space above and below the max/min values
            normalizeGraph();

            //Deletes all the current annotations
            chart_OHLCV.Annotations.Clear();

            //Makes the graph's data source come from the binding list
            chart_OHLCV.DataSource = boundList;
            //Displays the actual lines and candlesticks on the proper graph
            chart_OHLCV.DataBind();
        }


        /// <summary>
        /// Simple function for show_candleSticks, simply runs the orginal with bcandlesticks as parameter
        /// </summary>
        private void show_candleSticks()
        {
            //runs the display function to update visuals
            show_candleSticks(bcandlestick);
        }









        ///<summary>
        ///This function is called whenever the start date is changed, parameter are default for this 
        ///It just changes the global variable everytime the change is made
        /// </summary>
        /// 
        /// <param name="sender">
        /// the object that was used to trigger the event, button</param>
        /// <param name="e">
        /// The event that fired when the action was performed
        /// </param>
        private void DateTimePicker_start_date_changed(object sender, EventArgs e)
        {
            //Sets the global variable to whatever the current date is on the start date time picker
            startDate = dateTimePickerStart.Value;
        }






        /// <summary>
        /// This function is called whenever the end date is changed, parameter are default for this
        /// It just changes the global variable everytime the change is made
        /// </summary>
        /// 
        /// <param name="sender">
        /// the object that was used to trigger the event, button</param>
        /// <param name="e">
        /// The event that fired when the action was performed
        /// </param>
        private void DateTimePicker_end_date_changed(object sender, EventArgs e)
        {
            //Sets the global variable to whatever the current date is on the start date time picker
            endDate = dateTimePickerEnd.Value;
        }







        /// <summary>
        /// This funcction is to normalize the graph, which means that there will be enough space on the graph to 
        /// display both the lowest and highest value
        /// Takes the current binding list
        /// </summary>
        /// Takes the binding list connected to graph
        /// <param name="boundList"></param>
        private void normalizeGraph (BindingList<SmartCandlestick> boundList)
        {
            //Sets the lowest value first as the first value in the list
            decimal min = boundList.First().low;
            //sets the max value as zero
            decimal max = 0;
            //runs a loop to find the low and max of the whole list
            foreach (SmartCandlestick c in boundList)
            {
                //Comparing if the current is lower than the current list low
                if (c.low < min) 
                { 
                    //Replaces the list low with the current val
                    min = c.low;
                }

                //Comparing if the current is higher than the current list max
                if (c.high > max) 
                {
                    //Replaces the list max with the current val
                    max = c.high; 
                }
            }
            //Sets the OHLCV graph min in the Y axis as what was found in the loop with an extra space
            chartMin = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = Math.Round(Decimal.ToDouble(min) * 0.98, 2);
            //Sets the OHLCV graph max in the Y axis as what was found in the loop with an extra space
            chartMax = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = Math.Round(Decimal.ToDouble(max) * 1.02, 2);
        }






        /// <summary>
        /// Simple version of normalize graph, simply passes the current binding list into the normalizeGraph function
        /// </summary>
        private void normalizeGraph()
        {
            //passes the current binding list into the normalizeGraph function
            normalizeGraph(bcandlestick);
        }





        /// <summary>
        /// Overload of updateComboBox
        /// </summary>
        private void updateComboBox()
        {
            //calling the function with binding list of candlesticks 
            updateComboBox(bcandlestick);
        }







        /// <summary>
        /// Main update combo box function that updates the values in the combo box based on every pattern thats appeared in the data
        /// </summary>
        /// <param name="bindList">
        /// inputs the current binding list for that stock 
        /// </param>
        private void updateComboBox(BindingList<SmartCandlestick> bindList)
        {
            //Check if the list is not empty
            if (bindList.Count != 0)
            {
                //Clear the current selection of the combo box
                comboBox_PatternSelect.Items.Clear();
                //Create a new candle stick based on the first one of the list 
                SmartCandlestick scs = (SmartCandlestick)bindList[0];
                //Check every key in the candlestick's dictionary and add the 
                foreach (string key in scs.Dictionary_Pattern.Keys)
                {
                    //add the pattern to the combo box 
                    comboBox_PatternSelect.Items.Add(key);
                }
            }
        }



        /// <summary>
        /// Initializes every pattern Recognizer class and stores them in a dictionary
        /// Uses keys to fill up the combo box with all the items
        /// </summary>
        private void InitializeRecognizer()
        {
            Recognizer_Dict = new Dictionary<string, Recognizer>();

            //Bullish Recognizer
            Recognizer r = new Recognizer_Bullish();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Bearish Recognizer
            r = new Recognizer_Bearish();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Neutral Recognizer
            r = new Recognizer_Neutral();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Marubozu Recognizer
            r = new Recognizer_Marubozu();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Hammer Recognizer
            r = new Recognizer_Hammer();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Doji Recognizer
            r = new Recognizer_Doji();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Dragonfly Doji Recognizer
            r = new Recognizer_Dragonfly_Doji();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Gravestone Doji Recognizer
            r = new Recognizer_Gravestone_Doji();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Bullish Engulfing Recognizer
            r = new Recognizer_Bullish_Engulfing();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Bearish Engulfing Recognizer
            r = new Recognizer_Bearish_Engulfing();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Bullish Harami Recognizer
            r = new Recognizer_Bullish_Harami();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Bearish Harami Recognizer
            r = new Recognizer_Bearish_Harami();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Peak Recognizer
            r = new Recognizer_Peak();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            //Valley Recognizer
            r = new Recognizer_Valley();
            Recognizer_Dict.Add(r.Pattern_Name, r);
            




            //Initialize Combo Box
            comboBox_PatternSelect.Items.AddRange(Recognizer_Dict.Keys.ToArray());
        }


     












        /// <summary>
        /// Adds the arrow to every candlestick that meets the wanted combobox value
        /// </summary>
        /// <param name="sender">
        /// the object that was used to trigger the event, button
        /// </param>
        /// <param name="e">
        /// The event that fired when the action was performed
        /// </param>
        private void comboBox_Patterns_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clears whatever current annotations exist 
            chart_OHLCV.Annotations.Clear();


            if(bcandlestick != null)
            {
                //loop to check every candlestick in the list
                for (int i = 0; i < bcandlestick.Count; i++)
                {
                    //creates a smart candlestick from current candlestick
                    SmartCandlestick scs = (SmartCandlestick)bcandlestick[i];
                    //gets the index for that candlestick
                    DataPoint center = chart_OHLCV.Series[0].Points[i];

                    //Selected combobox pattern
                    string pattern = comboBox_PatternSelect.SelectedItem.ToString();

                    //check if the dummy exists
                    if (scs.Dictionary_Pattern[pattern])
                    {
                        //Gets the length of the pattern
                        int length = Recognizer_Dict[pattern].Pattern_Length;




                        //Annotate candlesticks for multi-candlestick patterns
                        if (length > 1)
                        {
                            //Skip indexes that cause out of bounds error
                            if (i == 0 | ((i == bcandlestick.Count() - 1) & length == 3))
                            {
                                continue;
                            }
                            //Initialize rectangle annotation
                            RectangleAnnotation rectangle = new RectangleAnnotation();
                            //sets the place where the center of the rectangle will go
                            rectangle.SetAnchor(center);

                            //The vertical bounds of the rectangle
                            double Ymax, Ymin;
                            //width of the box
                            double width = (90.0 / bcandlestick.Count()) * length; //Scale width to number of candlesticks
                             //Even number pattern
                            //Find the min and max between every candlestick in pattern
                            if (length == 2)    //Even number pattern
                            {
                                //sets the upper limit
                                Ymax = (int)(Math.Max(scs.high, bcandlestick[i - 1].high));
                                //sets lower limit
                                Ymin = (int)(Math.Min(scs.low, bcandlestick[i - 1].low));
                                //sets the offset for the anchor 
                                rectangle.AnchorOffsetX = ((width / length) / 2 - 0.25) * (-1);  //Offset even pattern for previous candlestick
                            }
                            //Odd number pattern
                            else
                            {
                                //sets the upper limit
                                Ymax = (int)(Math.Max(scs.high, Math.Max(bcandlestick[i + 1].high, bcandlestick[i - 1].high)));
                                //set the lower limit
                                Ymin = (int)(Math.Min(scs.low, Math.Min(bcandlestick[i + 1].low, bcandlestick[i - 1].low)));
                            }
                            //Sets the heigth of the box
                            double height = 40.0 * (Ymax - Ymin) / (chartMax - chartMin); 
                            //sets the heignth of the box 
                            rectangle.Height = height; rectangle.Width = width;        
                            //sets the y of the box
                            rectangle.Y = Ymax;                             
                            //sets the color
                            rectangle.BackColor = Color.Transparent;         
                            //set the line width
                            rectangle.LineWidth = 2;                   
                            //set the line style 
                            rectangle.LineDashStyle = ChartDashStyle.Dash;                  
                            //Add annotation to chart
                            chart_OHLCV.Annotations.Add(rectangle);
                        }

                        //For any single candlestick patterns

                        //make the annotation an arrow 
                        ArrowAnnotation arrow = new ArrowAnnotation();
                        //Grabs the x axis for the arrow
                        arrow.AxisX = chart_OHLCV.ChartAreas[0].AxisX;
                        //Grabs the y axis for the arrow
                        arrow.AxisX = chart_OHLCV.ChartAreas[0].AxisY;
                        //Sets width of arrow
                        arrow.Width = 0.5;
                        //HEight of arrow
                        arrow.Height = 0.5;
                        //Checks if the current selection is true in the candlesticks dictionary
                        if (scs.Dictionary_Pattern[comboBox_PatternSelect.SelectedItem.ToString()])
                        {
                            //Sets the anchor at the datapoint
                            arrow.SetAnchor(center);
                            //Adds the arrow to the annotations of the series
                            chart_OHLCV.Annotations.Add(arrow);
                        }
                    }
                }
            }



            
        }






















    }
}
