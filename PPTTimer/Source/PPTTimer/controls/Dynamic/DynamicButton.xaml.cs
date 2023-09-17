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

namespace PPTTimer.controls.Dynamic
{
    public partial class DynamicButton : UserControl
    {
        public DynamicButton()
        {
            InitializeComponent();

            Loaded += MyButton_Loaded;
            //将鼠标进入和离开事件绑定到动画
            //MouseEnter += CustomControl_MouseEnter;
            //MouseLeave += CustomControl_MouseLeave;
        }

        public ImageBrush imageFrontStop = new ImageBrush();
        public ImageBrush imageBehindStop = new ImageBrush();
        public ImageBrush imageFrontStart = new ImageBrush();
        public ImageBrush imageBehindStart = new ImageBrush();

        public string pathFrontStop, pathFrontStart;
        public string pathBehindStop, pathBehindStart;

        public double time = 0.3;//控制动画时间

        private void MyButton_Loaded(object sender, RoutedEventArgs e)
        {
            pathFrontStop = FrontStopImage;
            pathFrontStart = FrontStartImage;
            pathBehindStop = BehindStopImage;
            pathBehindStart = BehindStartImage;

            imageFrontStop.ImageSource = new BitmapImage(new Uri(pathFrontStop, UriKind.Relative));
            imageFrontStop.Stretch = Stretch.Fill;
            imageFrontStart.ImageSource = new BitmapImage(new Uri(pathFrontStart, UriKind.Relative));
            imageFrontStart.Stretch = Stretch.Fill;
            imageBehindStop.ImageSource = new BitmapImage(new Uri(pathBehindStop, UriKind.Relative));
            imageBehindStop.Stretch = Stretch.Fill;
            imageBehindStart.ImageSource = new BitmapImage(new Uri(pathBehindStart, UriKind.Relative));
            imageBehindStart.Stretch = Stretch.Fill;

            FrontStop.Source = imageFrontStop.ImageSource;
            FrontStart.Source = imageFrontStart.ImageSource;
            BehindStop.Source = imageBehindStop.ImageSource;
            BehindStart.Source = imageBehindStart.ImageSource;
        }

        #region 鼠标移入移出动画
        //private void CustomControl_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (!IsStoping)
        //    {
        //        Storyboard storyOpacityFrontStartEnter = new Storyboard();
        //        DoubleAnimation animationOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(time)));
        //        Storyboard.SetTarget(animationOpacity, FrontStart);
        //        Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
        //        storyOpacityFrontStartEnter.Children.Add(animationOpacity);
        //        storyOpacityFrontStartEnter.Begin();
        //    }
        //    else
        //    {
        //        Storyboard storyOpacityFrontStopEnter = new Storyboard();
        //        DoubleAnimation animationOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(time)));
        //        Storyboard.SetTarget(animationOpacity, FrontStop);
        //        Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
        //        storyOpacityFrontStopEnter.Children.Add(animationOpacity);
        //        storyOpacityFrontStopEnter.Begin();
        //    }
        //}

        //private void CustomControl_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (!IsStoping)
        //    {
        //        Storyboard storyOpacityFrontStartLeave = new Storyboard();
        //        DoubleAnimation animationOpacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(time)));
        //        Storyboard.SetTarget(animationOpacity, FrontStart);
        //        Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
        //        storyOpacityFrontStartLeave.Children.Add(animationOpacity);
        //        storyOpacityFrontStartLeave.Begin();
        //    }
        //    else
        //    {
        //        Storyboard storyOpacityFrontStopLeave = new Storyboard();
        //        DoubleAnimation animationOpacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(time)));
        //        Storyboard.SetTarget(animationOpacity, FrontStop);
        //        Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
        //        storyOpacityFrontStopLeave.Children.Add(animationOpacity);
        //        storyOpacityFrontStopLeave.Begin();
        //    }
        //}
        #endregion

        #region 前景暂停图
        public static readonly DependencyProperty FrontStopImageProperty =
        DependencyProperty.Register("FrontStopImage", typeof(string),
            typeof(DynamicButton), new PropertyMetadata(null));

        public string FrontStopImage
        {
            get { return (string)GetValue(FrontStopImageProperty); }
            set { SetValue(FrontStopImageProperty, value); }
        }
        #endregion

        #region 前景开始图
        public static readonly DependencyProperty FrontStartImageProperty =
            DependencyProperty.Register("FrontStartImage", typeof(string),
                typeof(DynamicButton), new PropertyMetadata(null));

        public string FrontStartImage
        {
            get { return (string)GetValue(FrontStartImageProperty); }
            set { SetValue(FrontStartImageProperty, value); }
        }
        #endregion

        #region 背景暂停图
        public static readonly DependencyProperty BehindStopImageProperty =
            DependencyProperty.Register("BehindStopImage", typeof(string),
                typeof(DynamicButton), new PropertyMetadata(null));

        public string BehindStopImage
        {
            get { return (string)GetValue(BehindStopImageProperty); }
            set { SetValue(BehindStopImageProperty, value); }
        }
        #endregion

        #region 背景开始图
        public static readonly DependencyProperty BehindStartImageProperty =
            DependencyProperty.Register("BehindStartImage", typeof(string),
                typeof(DynamicButton), new PropertyMetadata(null));

        public string BehindStartImage
        {
            get { return (string)GetValue(BehindStartImageProperty); }
            set { SetValue(BehindStartImageProperty, value); }
        }
        #endregion

        #region 记录是否开始状态，改变状态改变时动画
        public static readonly DependencyProperty IsStopingProperty =
            DependencyProperty.Register(nameof(IsStoping), typeof(bool),
                typeof(DynamicButton), new PropertyMetadata(false, OnIsCheckedChanged));

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TheButton = d as DynamicButton;
            var BehindStart = TheButton.FindName("BehindStart") as Image;
            var BehindStop = TheButton.FindName("BehindStop") as Image;

            if (TheButton.IsStoping)
            {
                Storyboard storyOpacityChecked = new Storyboard();
                DoubleAnimation animationOpacityChecked = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.3)));
                Storyboard.SetTarget(animationOpacityChecked, BehindStop);
                Storyboard.SetTargetProperty(animationOpacityChecked, new PropertyPath("Opacity"));
                storyOpacityChecked.Children.Add(animationOpacityChecked);
                storyOpacityChecked.Begin();

                Storyboard storyOpacityUnchecked = new Storyboard();
                DoubleAnimation animationOpacityUnchecked = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.3)));
                Storyboard.SetTarget(animationOpacityUnchecked, BehindStart);
                Storyboard.SetTargetProperty(animationOpacityUnchecked, new PropertyPath("Opacity"));
                storyOpacityUnchecked.Children.Add(animationOpacityUnchecked);
                storyOpacityUnchecked.Begin();
            }
            else
            {
                Storyboard storyOpacityChecked = new Storyboard();
                DoubleAnimation animationOpacityChecked = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.3)));
                Storyboard.SetTarget(animationOpacityChecked, BehindStop);
                Storyboard.SetTargetProperty(animationOpacityChecked, new PropertyPath("Opacity"));
                storyOpacityChecked.Children.Add(animationOpacityChecked);
                storyOpacityChecked.Begin();

                Storyboard storyOpacityUnchecked = new Storyboard();
                DoubleAnimation animationOpacityUnchecked = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.3)));
                Storyboard.SetTarget(animationOpacityUnchecked, BehindStart);
                Storyboard.SetTargetProperty(animationOpacityUnchecked, new PropertyPath("Opacity"));
                storyOpacityUnchecked.Children.Add(animationOpacityUnchecked);
                storyOpacityUnchecked.Begin();
            }
        }

        public bool IsStoping
        {
            get { return (bool)GetValue(IsStopingProperty); }
            set { SetValue(IsStopingProperty, value); }
        }
        #endregion
    }
}
