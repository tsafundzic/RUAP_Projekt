﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;



namespace RUAP_ProcjenaSnageBetona
{
   
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

      
        static async Task<string[]> InvokeRequestResponseService(StringTable input)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            input
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "QGKyr83PuEtjk8MQK/C6ecGVY24jlcjYBk0UH1MgwuVslOSIPHnp6de7WEenK9KcRQSR7b9SSkVU6n55k5htJw=="; 
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/df0b4927a7ee46f1b3ff308a9779338a/services/f2c6c80c447d4a78ac19747f8c7cf50b/execute?api-version=2.0&details=true");
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    string[] resultArray = result.Split(',');
                    Console.WriteLine("Result: {0}", result);
                    return resultArray; 
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    string[] resultArray = responseContent.Split(',');
                    return resultArray;
                }
            }
        }

        private void label_result_Click(object sender, EventArgs e)
        {

        }

        double function(double x)
        {
            StringTable inputValue = new StringTable()
            {
                ColumnNames = new string[] { "Cement (component 1)(kg in a m^3 mixture)", "Blast Furnace Slag (component 2)(kg in a m^3 mixture)", "Fly Ash (component 3)(kg in a m^3 mixture)", "Water  (component 4)(kg in a m^3 mixture)", "Superplasticizer (component 5)(kg in a m^3 mixture)", "Coarse Aggregate  (component 6)(kg in a m^3 mixture)", "Fine Aggregate (component 7)(kg in a m^3 mixture)", "Age (day)", "Concrete compressive strength(MPa, megapascals)" },
                Values = new string[,] { { textBox1_Cement.Text, textBox2_BlastFurnaceSlag.Text, textBox3_FlyAsh.Text, textBox4_Water.Text, textBox5_Superplasticizer.Text, textBox6_CoarseAggregate.Text, textBox7_FineAggregate.Text, x.ToString(), "0" }, { "0", "0", "0", "0", "0", "0", "0", "0", "0" }, }
            };

            string[] res = InvokeRequestResponseService(inputValue).Result;
            string rez = res[31];
            double rezultat = Double.Parse(rez.Substring(1, rez.Length - 8), CultureInfo.InvariantCulture);
            if (rezultat < 0)
            {
                return 0;
            }else return rezultat;
        }
        private void graph() {
            chartGraphic.Series[0].Points.Clear(); 
            chartGraphic.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartGraphic.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartGraphic.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartGraphic.ChartAreas[0].AxisX.IsMarginVisible = false;
            chartGraphic.ChartAreas[0].AxisX.Title = "Age [Day]";
            chartGraphic.ChartAreas[0].AxisY.Title = "Concrete compressive strength [MPa]";
            
            int x = int.Parse(textBox8_Day.Text);
            int i;
            for (i = 0 ; i<= 10*x; i+=10)
            {
                if (i > 365) break;
                chartGraphic.Series[0].Points.AddXY(i, function(i));
            }
            chartGraphic.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartGraphic.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            StripLine stripLine = new StripLine();
            stripLine.Interval = 0; // only show 1 line
            stripLine.IntervalOffset = x; // start it at x=0
            stripLine.StripWidth = 1; // the width is 1
            stripLine.BackColor = Color.Red;
            chartGraphic.ChartAreas[0].AxisX.StripLines.Add(stripLine);
            

        }

        private void button_calculate_Click_1(object sender, EventArgs e)
        {
            

            double test;
            int daytest;
            if (!double.TryParse(textBox1_Cement.Text, out test) || !double.TryParse(textBox2_BlastFurnaceSlag.Text, out test) || !double.TryParse(textBox3_FlyAsh.Text, out test) || !double.TryParse(textBox4_Water.Text, out test) || !double.TryParse(textBox5_Superplasticizer.Text, out test) || !double.TryParse(textBox6_CoarseAggregate.Text, out test) || !double.TryParse(textBox7_FineAggregate.Text, out test) || !int.TryParse(textBox7_FineAggregate.Text, out daytest))
            {   
  
                MessageBox.Show("You must enter numbers!");
                return;
            }
            else if (int.Parse(textBox8_Day.Text) > 365 || int.Parse(textBox8_Day.Text) <0)
            {
                MessageBox.Show("Out of range");
                return;
            }

            StringTable inputValue = new StringTable()
            {
                ColumnNames = new string[] { "Cement (component 1)(kg in a m^3 mixture)", "Blast Furnace Slag (component 2)(kg in a m^3 mixture)", "Fly Ash (component 3)(kg in a m^3 mixture)", "Water  (component 4)(kg in a m^3 mixture)", "Superplasticizer (component 5)(kg in a m^3 mixture)", "Coarse Aggregate  (component 6)(kg in a m^3 mixture)", "Fine Aggregate (component 7)(kg in a m^3 mixture)", "Age (day)", "Concrete compressive strength(MPa, megapascals)" },
                Values = new string[,] { { textBox1_Cement.Text,textBox2_BlastFurnaceSlag.Text, textBox3_FlyAsh.Text, textBox4_Water.Text, textBox5_Superplasticizer.Text, textBox6_CoarseAggregate.Text, textBox7_FineAggregate.Text, textBox8_Day.Text, "0" }, { "0", "0", "0", "0", "0", "0", "0", "0", "0" }, }
            };

            string[] res = InvokeRequestResponseService(inputValue).Result;
            string rez = res[31];
            double rezultat = Double.Parse(rez.Substring(1, rez.Length - 8),CultureInfo.InvariantCulture);
            if (rezultat < 0)
            {
                MessageBox.Show("Check the entered data, there is an error");
                return;
            }
            else
            {
                label_result.Text = String.Format("{0:0.00}", rezultat);
            }
            graph();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}
