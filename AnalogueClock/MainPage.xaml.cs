using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
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
        bool editTimeState = false;
        public int TweleveHrDotRadius = 120, TwentyFourHrDotRadius = 135;
        public int TweleveHrNumberRadius = 103, TwentyFourHrNumberRadius = 120;
        public int FromSecond = -90 + DateTime.Now.Second * 6;
        public int ToSecond = 360 + (-90 + DateTime.Now.Second * 6);

        bool EditTimeflag = false;
        bool TwelveHrSubscriber = false;
        bool TwentyFourHrSubscriber = false;
        int extraMinute = 0;
        int extraHour = 0;
        double TotalScreenWidth ;
        double TotalScreenHeight ;

        Windows.UI.Core.CoreCursorType CustomCursorDirectionValue= Windows.UI.Core.CoreCursorType.SizeNorthwestSoutheast;

        //public int actualButtonWidth=20;


        List<TextBlock> allDotsTextBlock = new List<TextBlock>();
        List<TextBlock> allNumberTextBlock = new List<TextBlock>();

        public MainPage()
        {

            this.InitializeComponent();
            Debug.WriteLine("Timee- " + FromSecond + " actual second: " + DateTime.Now.Second);

            //initialize to modify visiblity and access all textboxes
            //intializing textblocks 12 hr
            List<TextBlock> NumberTextBlock = new List<TextBlock>();
            for (int i = 0; i < 24; i++)
            {
                TextBlock t = new TextBlock();
                t.Text = (i + 1).ToString();
                NumberCanvas.Children.Add(t);
                NumberTextBlock.Add(t);
            }
            NumberTextBlock.Reverse();

            allNumberTextBlock = NumberTextBlock;

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
            outerBlack.Width += 20; outerBlack.Height += 20;
            innerBlack.Width += 20; innerBlack.Height += 20;
            gray.Height += 20; gray.Width += 20;
            secondHand.X1 += 10; minuteHand.X1 += 10; hourHand.X1 += 7;
        }

        public void DecreaseClockSize()
        {
            outerBlack.Width -= 20; outerBlack.Height -= 20;
            innerBlack.Width -= 20; innerBlack.Height -= 20;
            gray.Height -= 20; gray.Width -= 20;
            secondHand.X1 -= 10; minuteHand.X1 -= 10; hourHand.X1 -= 7;

        }



        //set for 12 hr format
        public void TwelveHrClock()
        {
            //set allignment
            int[] CanvasAlignmentValues = { 0, 95, 100, 0, 100, 200, 200, 100 };
            NumberCanvas.Width = 220; NumberCanvas.Height = 235;
            outerBlack.Width = 280; outerBlack.Height = 280;
            innerBlack.Width = 270; innerBlack.Height = 270;
            gray.Height = 260; gray.Width = 260;

            //subscribe to 12 hr tick
            Timer_Subscribe_12Hr(TwelveHrSubscriber);

            //calculate position align numbers r initialangle valuenumberblock buffer -> calculateNumberAlignment
            calculateNumberAlignment(TweleveHrNumberRadius, -270, allNumberTextBlock, 30);
            //align dots
            AlignDots(TweleveHrDotRadius, 6, true);

        }

        //set for 24 hr format
        public void TwentyFourHrClock()
        {
            //set allignment
            int[] CanvasAlignmentValues = { -20, 100, 100, -20, 100, 220, 220, 100 };

            NumberCanvas.Width = 220; NumberCanvas.Height = 230;
            outerBlack.Width = 310; outerBlack.Height = 310;
            innerBlack.Width = 300; innerBlack.Height = 300;
            gray.Height = 290; gray.Width = 290;

            //subscribe to 24tick
            Timer_Subscribe_24Hr(TwentyFourHrSubscriber);

            // calculate number positions - > calculateNumberAlignment
            calculateNumberAlignment(TwentyFourHrNumberRadius, -270, allNumberTextBlock, 15);
            //calculate dot positions
            AlignDots(TwentyFourHrDotRadius, 15, false);
        }

        //update when the datetime changes
        private void Timer_Tick_24(object sender, object e)
        {
            Minute.Rotation = ((DateTime.Now.Minute + extraMinute) * 6) - 90;
            Hour.Rotation = ((DateTime.Now.Hour + extraHour) * 15) + ((DateTime.Now.Minute + extraMinute) * 0.25) - 90;
        }
        private void Timer_Tick_12(object sender, object e)
        {
            Minute.Rotation = ((DateTime.Now.Minute + extraMinute) * 6) - 90;
            Hour.Rotation = ((DateTime.Now.Hour + extraHour) * 30) + ((DateTime.Now.Minute + extraMinute) * 0.5) - 90;
        }

        private void Timer_Subscribe_12Hr(bool val)
        {
            if (!val)
            {
                timer.Tick += Timer_Tick_12;
                TwelveHrSubscriber = true;
            }
        }
        private void Timer_Subscribe_24Hr(bool val)
        {
            if (!val)
            {
                timer.Tick += Timer_Tick_24;
                TwentyFourHrSubscriber = true;

            }
        }

        private void calculateNumberAlignment(int r, int initialangle, List<TextBlock> allNumberTextBlock, int incrementAngle)
        {

            double x1, y1;
            int inccount = 0;
            bool flag = false;
            if (incrementAngle == 30) flag = true;

            if (r < 90 && flag == false)
            {
                Style s = (Style)Resources["twentyfourHrFontSmall"];
                foreach (var i in allNumberTextBlock)
                {
                    i.Style = s;
                }
            }
            else
            {
                Style s = (Style)Resources["twelveHrFont"];
                foreach (var i in allNumberTextBlock)
                {
                    i.Style = s;
                }
            }

            foreach (var i in allNumberTextBlock)
            {
                inccount++;
                if (flag == true && inccount <= 12)
                {
                    i.Visibility = Visibility.Collapsed;
                    continue;
                }
                else
                {
                    i.Visibility = Visibility.Visible;
                }

                x1 = 112 + r * Math.Cos(Math.PI * initialangle / 180.0) - 10;
                y1 = 108 - r * Math.Sin(Math.PI * initialangle / 180.0) - 10;
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
            secondHand.X1 = 115; minuteHand.X1 = 105; hourHand.X1 = 70;
        }

        private void Decrement_Click(object sender, RoutedEventArgs e)
        {
            if (outerBlack.Width > 250 || (outerBlack.Width > 220 && clockState == "twelve"))
            {
                DecreaseClockSize();
                if (clockState == "twelve")
                {
                    //calaculte number textblocks resizing
                    AlignDots(TweleveHrDotRadius -= 10, 6, true);
                    calculateNumberAlignment(TweleveHrNumberRadius -= 10, -270, allNumberTextBlock, 30);
                }
                else
                {
                    //calaculte number textblocks resizing
                    calculateNumberAlignment(TwentyFourHrNumberRadius -= 10, -270, allNumberTextBlock, 15);

                    AlignDots(TwentyFourHrDotRadius -= 10, 15, false);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryBoard.Begin();
        }

        //resize on mouse drag
        private void t_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            TotalScreenWidth = ((Frame)Window.Current.Content).ActualWidth;
            TotalScreenHeight = ((Frame)Window.Current.Content).ActualHeight;

            Debug.WriteLine(e.Delta.Translation.X.ToString() + "  -------  " + e.Delta.Translation.Y.ToString());
            var XPointerVal = Window.Current.CoreWindow.PointerPosition.X;
            var YPointerVal = Window.Current.CoreWindow.PointerPosition.Y;

            Debug.WriteLine("X: "+Window.Current.CoreWindow.PointerPosition.X.ToString()+" Y: "+Window.Current.CoreWindow.PointerPosition.Y.ToString()+" "+TotalScreenHeight+" "+TotalScreenWidth);

            if ((e.Delta.Translation.X > 0 && e.Delta.Translation
             .Y > 0) && (int)e.Delta.Translation.X % 3 == 0)
            {
                CustomCursorDirectionValue = Windows.UI.Core.CoreCursorType.SizeNorthwestSoutheast;

                if (XPointerVal >= TotalScreenWidth/2 || YPointerVal >= TotalScreenHeight/2)
                    Increment_Click(sender, e);
                else Decrement_Click(sender, e);
            }
            else if ((e.Delta.Translation.X < 0 && e.Delta.Translation.Y < 0) && (int)e.Delta.Translation.X % 3 == 0)
            {
                CustomCursorDirectionValue = Windows.UI.Core.CoreCursorType.SizeNorthwestSoutheast;

                if (XPointerVal >= TotalScreenWidth/2 || YPointerVal >= TotalScreenHeight/2)
                    Decrement_Click(sender, e);
                else Increment_Click(sender, e);
            }
            else if ((e.Delta.Translation.X < 0 && e.Delta.Translation.Y > 0) && (int)e.Delta.Translation.X % 3 == 0)
            {
                CustomCursorDirectionValue = Windows.UI.Core.CoreCursorType.SizeNortheastSouthwest;

                if (XPointerVal >= TotalScreenWidth/2 && YPointerVal <= TotalScreenHeight/2)
                    Decrement_Click(sender, e);
                else Increment_Click(sender, e);
            }
            else if ((e.Delta.Translation.X > 0 && e.Delta.Translation.Y < 0) && (int)e.Delta.Translation.X % 3 == 0)
            {
                CustomCursorDirectionValue = Windows.UI.Core.CoreCursorType.SizeNortheastSouthwest;

                if (XPointerVal >= TotalScreenWidth/2 && YPointerVal <= TotalScreenHeight/2)
                    Increment_Click(sender, e);
                else Decrement_Click(sender, e);
            }

        }

        //edit clock
        private void EditTime_Click(object sender, RoutedEventArgs e)
        {

            if (EditTimeflag == false)
            {
                MyScrollViewer.PointerWheelChanged += MyScrollViewer_PointerWheelChanged;
                timer.Tick -= Timer_Tick_12;
                timer.Tick -= Timer_Tick_24;
                editTimeState = true;
                EditTimeflag = true;
                TwelveHrSubscriber = false; TwelveHrSubscriber = false;
                secondHand.Visibility = Visibility.Collapsed;
            }
            else
            {
                MyScrollViewer.PointerWheelChanged -= MyScrollViewer_PointerWheelChanged;
                if (clockState == "twelve")
                {
                    timer.Tick += Timer_Tick_12;
                }
                else
                {
                    timer.Tick -= Timer_Tick_24;
                }
                editTimeState = false;
                EditTimeflag = false;
                secondHand.Visibility = Visibility.Visible;
            }
        }


        //on mouse wheel scroll activation
        private void MyScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint CurrentPoint = e.GetCurrentPoint(OverallClockContainer);
            var value = CurrentPoint.Properties.MouseWheelDelta;
         // Debug.WriteLine("pointer: " + CurrentPoint);
           // Debug.WriteLine("value :  " + value);

            if (value < 0)
            {
                int tempMinuteCount = 0;
                Minute.Rotation += 1;

                if (((int)Minute.Rotation) % 60 == 0 && clockState == "twelve")
                {
                    Hour.Rotation += 5;
                }

                if ((int)Minute.Rotation % 6 == 0)
                {
                    extraMinute += 1;
                    tempMinuteCount += 1;
                }
                if (tempMinuteCount == 60)
                {
                    extraHour += 1;
                }
                if (((int)Minute.Rotation) % 180 == 0 && clockState == "twentyfour")
                {
                    Hour.Rotation += 7.5;
                }
            }
            else
            {
                int tempMinuteCount = 0;
                Minute.Rotation -= 1;

                if (((int)Minute.Rotation) % 60 == 0 && clockState == "twelve")
                {
                    Hour.Rotation -= 5;
                }

                if ((int)Minute.Rotation % 6 == 0)
                {
                    extraMinute -= 1;
                    tempMinuteCount -= 1;
                }
                if (tempMinuteCount == 60)
                {
                    extraHour -= 1;
                }

                if (((int)Minute.Rotation) % 180 == 0 && clockState == "twentyfour")
                {
                    Hour.Rotation -= 7.5;
                }
            }
        }

        private void MyScrollViewer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

         

            //if (e.GetCurrentPoint(OverallClockContainer).Position.X< e.GetCurrentPoint(OverallClockContainer).Position.Y)
            //{
            //     CustomCursorDirectionValue = Windows.UI.Core.CoreCursorType.SizeNorthwestSoutheast;
            //    //Debug.WriteLine("-===========-   " + e.GetCurrentPoint(OverallClockContainer).Position.X);
            //    //Debug.WriteLine("-===========-   " + e.GetCurrentPoint(OverallClockContainer).Position.Y);
            //}
            //else
            //{
            //    CustomCursorDirectionValue = Windows.UI.Core.CoreCursorType.SizeNortheastSouthwest;
            //}
           
          
            //Debug.WriteLine("pointer position "+Window.Current.CoreWindow.PointerPosition);
            Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(CustomCursorDirectionValue, 1);
        }

        private void MyScrollViewer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void OverallClockContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //
        }

        //private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
           
        //    Debug.WriteLine("Textblock size changed: "+ e.PreviousSize + " " + e.NewSize);
            
          
        //}

        //private void incbuttonsize_Click(object sender, RoutedEventArgs e)
        //{
        //    dummy.Width += 10;
        //    dummy.Height += 10;
        //    Debug.WriteLine("Inc done");
        //}

      

        //increment clock size
        private void Increment_Click(object sender, RoutedEventArgs e)
        {
            if (outerBlack.Width < 440)
            {
                IncreaseClockSize();
                if (clockState == "twelve")
                {
                    //calaculte number textblocks resizing
                    calculateNumberAlignment(TweleveHrNumberRadius += 10, -270, allNumberTextBlock, 30);
                    AlignDots(TweleveHrDotRadius += 10, 6, true);
                }
                else
                {
                    //calaculte number textblocks resizing
                    calculateNumberAlignment(TwentyFourHrNumberRadius += 10, -270, allNumberTextBlock, 15);
                    AlignDots(TwentyFourHrDotRadius += 10, 15, false);
                }
            }
        }

        //change clock between 12 and 24 hr
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (editTimeState == false)
            {
                if (clockState == "twelve")
                {
                    timer.Tick -= Timer_Tick_12;
                    TwelveHrSubscriber = false;
                    clockState = "twentyfour";
                    this.TwentyFourHrNumberRadius = 120;
                    this.TwentyFourHrDotRadius = 135;
                    ResetHandSize();
                    TwentyFourHrClock();
                }
                else
                {
                    timer.Tick -= Timer_Tick_24;
                    TwentyFourHrSubscriber = false;
                    clockState = "twelve";
                    this.TweleveHrNumberRadius = 103;
                    this.TweleveHrDotRadius = 120;
                    ResetHandSize();
                    TwelveHrClock();
                }
            }
        }
    }

}