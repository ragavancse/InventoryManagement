using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HamburgerMenuControl
{
    public class HamburgerMenu : Control
    {
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(HamburgerMenu), 
                new PropertyMetadata(false, OnIsOpenPropertyChanged));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty OpenCloseDurationProperty =
            DependencyProperty.Register("OpenCloseDuration", typeof(Duration), typeof(HamburgerMenu), 
                new PropertyMetadata(Duration.Automatic));

        public Duration OpenCloseDuration
        {
            get { return (Duration)GetValue(OpenCloseDurationProperty); }
            set { SetValue(OpenCloseDurationProperty, value); }
        }

        public static readonly DependencyProperty FallbackOpenWidthProperty =
            DependencyProperty.Register("FallbackOpenWidth", typeof(double), typeof(HamburgerMenu), 
                new PropertyMetadata(100.0));

        public double FallbackOpenWidth
        {
            get { return (double)GetValue(FallbackOpenWidthProperty); }
            set { SetValue(FallbackOpenWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(HamburgerMenu), 
                new PropertyMetadata(null));

        public FrameworkElement Content
        {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        public HamburgerMenu()
        {
            Width = 75;
        }

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is HamburgerMenu hamburgerMenu)
            {
                hamburgerMenu.OnIsOpenPropertyChanged();
            }
        }

        private void OnIsOpenPropertyChanged()
        {
            if(IsOpen)
            {
                OpenMenuAnimated();
            }
            else
            {
                CloseMenuAnimated();
            }    
        }

        private void OpenMenuAnimated()
        {
            double contentWidth = GetDesiredContentWidth();

            DoubleAnimation openingAnimation = new DoubleAnimation(contentWidth, OpenCloseDuration);
            BeginAnimation(WidthProperty, openingAnimation);
        }



        //public double MinOpenWidth
        //{
        //    get { return (double)GetValue(MinOpenWidthProperty); }
        //    set { SetValue(MinOpenWidthProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MinOpenWidth.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MinOpenWidthProperty =
        //    DependencyProperty.Register("MinOpenWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(0));



        private double GetDesiredContentWidth()
        {
            if(Content == null)
            {
                return FallbackOpenWidth;
            }

            Content.Measure(new Size(MaxWidth, MaxHeight));

            return Content.DesiredSize.Width;
        }

        private void CloseMenuAnimated()
        {
            DoubleAnimation closingAnimation = new DoubleAnimation(75, OpenCloseDuration);
            BeginAnimation(WidthProperty, closingAnimation);
        }
    }
}
