﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AnalogueClock
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public string clockState = "twelve";
        public int TweleveHrDotRadius = 120, TwentyFourHrDotRadius = 135;
        public int TweleveHrNumberRadius = 98, TwentyFourHrNumberRadius = 120;
        public int FromSecond =-90+DateTime.Now.Second*6;
       

        List<TextBlock> allDotsTextBlock = new List<TextBlock>();
        public MainPage()
        {
            this.InitializeComponent();
            Debug.WriteLine("Timee"+ FromSecond);

            //initialize to modify visiblity and access all textboxes


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

        //increase size
        public void IncreaseClockSize()
        {
            NumberCanvas.Width += 20; NumberCanvas.Height += 20;
            outerBlack.Width += 20; outerBlack.Height += 20;
            innerBlack.Width += 20; innerBlack.Height += 20;
            gray.Height += 20; gray.Width += 20;
            secondHand.X1 += 10;minuteHand.X1 += 10;hourHand.X1 += 7;
        }

        public void DecreaseClockSize()
        {
            NumberCanvas.Width -= 20; NumberCanvas.Height -= 20;
            outerBlack.Width -= 20; outerBlack.Height -= 20;
            innerBlack.Width -= 20; innerBlack.Height -= 20;
            gray.Height -= 20; gray.Width -= 20;
            secondHand.X1 -=10;minuteHand.X1 -= 10;hourHand.X1 -= 6;
            
        }

     

        //set for 12 hr format
        public void TwelveHrClock()
        {
            //set allignment
            int[] CanvasAlignmentValues = { 0, 95, 100, 0, 100, 200, 200, 100 };
           // SetCanvasProperties(CanvasAlignmentValues);
            NumberCanvas.Width = 220; NumberCanvas.Height = 235;
            outerBlack.Width = 280; outerBlack.Height = 280;
            innerBlack.Width = 270; innerBlack.Height = 270;
            gray.Height = 260; gray.Width = 260;


        
            //subscribe to 12 hr tick
            timer.Tick += Timer_Tick_12;

            //calculate position align numbers r initialangle valuenumberblock buffer -> calculateNumberAlignment
            //align dots
            AlignDots(TweleveHrDotRadius, 6, true);

        }

        //set for 24 hr format
        public void TwentyFourHrClock()
        {
            //set allignment
            int[] CanvasAlignmentValues = { -20, 100, 100, -20, 100, 220, 220, 100 };

           // SetCanvasProperties(CanvasAlignmentValues);
            NumberCanvas.Width = 220; NumberCanvas.Height = 230;
            outerBlack.Width = 310; outerBlack.Height = 310;
            innerBlack.Width = 300; innerBlack.Height = 300;
            gray.Height = 290; gray.Width = 290;

            //subscribe to 24tick
            timer.Tick += Timer_Tick_24;

            // calculate number positions - > calculateNumberAlignment

            //calculate dot positions
            AlignDots(TwentyFourHrDotRadius, 15, false);
        }

        //update when the datetime changes
        private void Timer_Tick_24(object sender, object e)
        {
            Minute.Rotation = (DateTime.Now.Minute * 6) - 90;
            Hour.Rotation = (DateTime.Now.Hour * 15) + (DateTime.Now.Minute * 0.25) - 90;
        }
        private void Timer_Tick_12(object sender, object e)
        {
            Minute.Rotation = (DateTime.Now.Minute * 6) - 90;
            Hour.Rotation = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5) - 90;
        }

       

        private void calculateNumberAlignment(int r, int initialangle, List<TextBlock> valueNumberBlocks, int incrementSkip, int incrementAngle)
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
               
            }

        }

        //Align dots
        private void AlignDots(int rdot, int inc, bool twelveHrFormat)
        {
            int count = 0;
            foreach (var i in allDotsTextBlock)
            {
                double x1dot, y1dot;

                if (count % 5 == 0 && twelveHrFormat)
                {
                    i.FontSize = 40;
                    x1dot = (130 + rdot * Math.Cos(Math.PI * (count * inc) / 180.0) - 4);
                    y1dot = (110 - rdot * Math.Sin(Math.PI * (count * inc) / 180.0) - 20);
                }
                else
                {
                    i.FontSize = 20;
                    x1dot = (130 + rdot * Math.Cos(Math.PI * (count * inc) / 180.0));
                    y1dot = (110 - rdot * Math.Sin(Math.PI * (count * inc) / 180.0));
                }

                i.SetValue(Canvas.LeftProperty, x1dot);
                i.SetValue(Canvas.TopProperty, y1dot);
                count++;
            }
        }
        //reset hand size
        private void ResetHandSize()
        {
            secondHand.X1 = 115; minuteHand.X1 = 105;hourHand.X1 = 70;
        }

        private void Decrement_Click(object sender, RoutedEventArgs e)
        {
            if (outerBlack.Width > 220)
            {
                DecreaseClockSize();
                if (clockState == "twelve")
                {
                    //calaculte number textblocks resizing
                    AlignDots(TweleveHrDotRadius -= 10, 6, true);
                }
                else
                {
                    //calaculte number textblocks resizing
                    AlignDots(TwentyFourHrDotRadius -= 10, 15, false);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryBoard.Begin();
        }

        private void Increment_Click(object sender, RoutedEventArgs e)
        {
            if (outerBlack.Width < 440)
            {
                IncreaseClockSize();
                if (clockState == "twelve")
                {
                    //calaculte number textblocks resizing
                    AlignDots(TweleveHrDotRadius += 10, 6, true);
                }
                else
                {
                    //calaculte number textblocks resizing
                    AlignDots(TwentyFourHrDotRadius += 10, 15, false);
                }
            }
        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (clockState == "twelve")
            {
                clockState = "twentyfour";
                this.TwentyFourHrNumberRadius = 120;
                this.TwentyFourHrDotRadius = 135;
                ResetHandSize();
                TwentyFourHrClock();

            }
            else
            {
                clockState = "twelve";
                this.TweleveHrNumberRadius = 98;
                this.TweleveHrDotRadius = 120;
                ResetHandSize();
                TwelveHrClock();

            }
        }
    }
}