using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AnalogueClock
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public string clockState = "twelve";
     
        TextBlock[] VisibilityTextBlocks;
        TextBlock[] AllTextBlocks;
        List<TextBlock> allDotsTextBlock = new List<TextBlock>();
        public MainPage()
        {
            this.InitializeComponent();
            //initialize to modify visiblity and access all textboxes
            TextBlock[] addtextBlocks = { textblock13, textblock14, textblock15, textblock16, textblock17, textblock19, textblock20, textblock21, textblock22, textblock23, textblock3, textblock9 };
            TextBlock[] addall = {top,left,right,bottom, textblock13, textblock14, textblock15, textblock16, textblock17, textblock19, textblock20, textblock21, textblock22, textblock23, textblock3, textblock9,textblock1,textblock10,textblock11,textblock2,textblock4,textblock5,textblock7,textblock8 };

            VisibilityTextBlocks = addtextBlocks;
            AllTextBlocks = addall;

            //set dots representing minutes 
            for (int i = 0; i < 60; i++)
            {
                TextBlock t = new TextBlock();
                t.Text = ".";
                t.FontSize = 20;
                DotCanvas.Children.Add(t);
                allDotsTextBlock.Add(t);
            }

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            TwelveHrClock();
        }
            
        //set for 12 hr format
        public void TwelveHrClock()
        {
            //set allignment
            int[] CanvasAlignmentValues = { 0, 95, 100, 0, 100, 200, 200, 100 };
            SetCanvasProperties(CanvasAlignmentValues);
            NumberCanvas.Width = 220;NumberCanvas.Height = 235;
            outerBlack.Width = 280;outerBlack.Height = 280;
            innerBlack.Width = 270; innerBlack.Height = 270;
            gray.Height = 260; gray.Width = 260;

            //switch style,visibility and text
            Style s = (Style)Resources["twelveHrFont"];
            foreach(var i in AllTextBlocks)
            {
                i.Style = s;
            }
            foreach (var i in VisibilityTextBlocks)
            {
                i.Visibility = Visibility.Collapsed;
            }
            
            top.Text = "12"; bottom.Text = "6"; right.Text = "3"; left.Text = "9";

            //subscribe to 12 hr tick
            timer.Tick += Timer_Tick_12;

            //calculate position align numbers r initialangle valuenumberblock buffer
            List<TextBlock> valueNumberBlocks = new List<TextBlock> { textblock1, textblock2, textblock4, textblock5, textblock7, textblock8, textblock10, textblock11 };
            valueNumberBlocks.Reverse();
            calculateNumberAlignment(98, -240, valueNumberBlocks, 2, 30);

            //align dots
            double rdot = 120, inc = 6, count = 0;

            foreach (var i in allDotsTextBlock)
            {
                double x1dot, y1dot;
               
                if (count % 5 == 0)
                {
                    i.FontSize = 40;
                     x1dot = (130 + rdot * Math.Cos(Math.PI * (count * inc) / 180.0)-4);
                     y1dot = (110 - rdot * Math.Sin(Math.PI * (count * inc) / 180.0)-20);
                }
                else
                {
                     x1dot = (130 + rdot * Math.Cos(Math.PI * (count * inc) / 180.0));
                     y1dot = (110 - rdot * Math.Sin(Math.PI * (count * inc) / 180.0));
                }
             
                i.SetValue(Canvas.LeftProperty, x1dot);
                i.SetValue(Canvas.TopProperty, y1dot);
                count++;
            }
        }

     //set for 24 hr format
        public void TwentyFourHrClock()
        {
            //set allignment
            int[] CanvasAlignmentValues = { -20, 100, 100, -20, 100, 220, 220, 100 };
            SetCanvasProperties(CanvasAlignmentValues);
            NumberCanvas.Width = 220;NumberCanvas.Height = 230;
            outerBlack.Width = 310;outerBlack.Height = 310;
            innerBlack.Width = 300; innerBlack.Height = 300;
            gray.Height = 290; gray.Width = 290;

        
            //switch style,visibility and text
            Style s = (Style)Resources["twentyfourHrFont"];
            foreach (var i in AllTextBlocks)
            {
                i.Style = s;
            }

            foreach (var i in VisibilityTextBlocks)
            {
                i.Visibility = Visibility.Visible;
            }
            top.Text = "24"; bottom.Text = "12"; right.Text = "6"; left.Text = "18";


            //subscribe to 24tick
            timer.Tick += Timer_Tick_24;

            // calculate number positions
            List<TextBlock> valueNumberBlocks = new List<TextBlock> { textblock1, textblock2, textblock3, textblock4, textblock5, textblock7, textblock8, textblock9, textblock10, textblock11, textblock13, textblock14, textblock15, textblock16, textblock17, textblock19, textblock20, textblock21, textblock22, textblock23 };
            valueNumberBlocks.Reverse();
            calculateNumberAlignment(120, -255, valueNumberBlocks, 5, 15);
           

            //calculate dot positions
            double rdot = 135, inc = 15, count = 0;
            foreach (var i in allDotsTextBlock)
            {
                i.FontSize = 20;
                double x1dot = (130 + rdot * Math.Cos(Math.PI * (count * inc) / 180.0));
                double y1dot = (110 - rdot * Math.Sin(Math.PI * (count * inc) / 180.0));
                i.SetValue(Canvas.LeftProperty, x1dot);
                i.SetValue(Canvas.TopProperty, y1dot);
                count++;
            }
        }

        //update when the datetime changes
        private void Timer_Tick_24(object sender, object e)
        {
            Second.Rotation = (DateTime.Now.Second * 6) - 90;
            Minute.Rotation = (DateTime.Now.Minute * 6) -90;
            Hour.Rotation = (DateTime.Now.Hour * 15) + (DateTime.Now.Minute * 0.25) - 90;
        }
        private void Timer_Tick_12(object sender, object e)
        {
            Second.Rotation = (DateTime.Now.Second * 6) -90;
            Minute.Rotation = (DateTime.Now.Minute * 6) -90;
            Hour.Rotation = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5) -90;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (clockState == "twelve")
            {
                clockState = "twentyfour";
                TwentyFourHrClock();
            }
            else
            {
                clockState = "twelve";
                TwelveHrClock();
            }
        }

        private void SetCanvasProperties(int[] propertyValues)
        {
            int i = 0;
            List<TextBlock> fixedTextBlock = new List<TextBlock> { top, left, right, bottom };
            foreach (var t in fixedTextBlock)
            {
                t.SetValue(Canvas.TopProperty, propertyValues[i++]);
                t.SetValue(Canvas.LeftProperty, propertyValues[i++]);
            }
        }

        private void calculateNumberAlignment(int r, int initialangle,List<TextBlock> valueNumberBlocks,int incrementSkip,int incrementAngle)
        {
            double x1, y1;
            int inccount = 0;

            foreach (var i in valueNumberBlocks)
            {
                inccount++;
                x1 = 110 + r * Math.Cos(Math.PI * initialangle / 180.0) - 10;
                y1 = 110 - r * Math.Sin(Math.PI * initialangle / 180.0) - 10;
                i.SetValue(Canvas.LeftProperty, x1);
                i.SetValue(Canvas.TopProperty, y1);
                initialangle += incrementAngle;
                if (inccount % incrementSkip == 0)
                {
                    initialangle += incrementAngle;
                }
            }

        }
    }
}
