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

namespace PPTTimer.controls.Static
{
    /// <summary>
    /// StaticButton.xaml 的交互逻辑
    /// </summary>
    public partial class StaticButton : UserControl
    {
        public StaticButton()
        {
            InitializeComponent();
            Loaded += CloseButton_Loaded;

            //将鼠标进入和离开事件绑定到动画
            MouseEnter += CustomControl_MouseEnter;
            MouseLeave += CustomControl_MouseLeave;
            //MouseLeftButtonDown += MyCheckBox_MouseLeftButtonDown;
        }

        public ImageBrush imageFront = new ImageBrush();
        public ImageBrush imageBehind = new ImageBrush();

        public string pathFront;
        public string pathBehind;

        public double time = 0.3;//控制动画时间

        private void CloseButton_Loaded(object sender, RoutedEventArgs e)
        {
            pathFront = FrontImage;
            pathBehind = BehindImage;

            imageFront.ImageSource = new BitmapImage(new Uri(pathFront, UriKind.Relative));
            imageFront.Stretch = Stretch.Fill;
            imageBehind.ImageSource = new BitmapImage(new Uri(pathBehind, UriKind.Relative));
            imageBehind.Stretch = Stretch.Fill;

            Front.Source = imageFront.ImageSource;
            Behind.Source = imageBehind.ImageSource;
        }

        #region 鼠标移入移出动画
        private void CustomControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard storyOpacity = new Storyboard();
            DoubleAnimation animationOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(time)));
            Storyboard.SetTarget(animationOpacity, Front);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
            storyOpacity.Children.Add(animationOpacity);
            storyOpacity.Begin();
        }

        private void CustomControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard storyOpacity = new Storyboard();
            DoubleAnimation animationOpacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(time)));
            Storyboard.SetTarget(animationOpacity, Front);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
            storyOpacity.Children.Add(animationOpacity);
            storyOpacity.Begin();
        }
        #endregion

        #region 前景图
        public static readonly DependencyProperty FrontImageProperty =
        DependencyProperty.Register("FrontImage", typeof(string),
            typeof(StaticButton), new PropertyMetadata(null));

        public string FrontImage
        {
            get { return (string)GetValue(FrontImageProperty); }
            set { SetValue(FrontImageProperty, value); }
        }
        #endregion

        #region 背景图
        public static readonly DependencyProperty BehindImageProperty =
            DependencyProperty.Register("BehindImage", typeof(string),
                typeof(StaticButton), new PropertyMetadata(null));

        public string BehindImage
        {
            get { return (string)GetValue(BehindImageProperty); }
            set { SetValue(BehindImageProperty, value); }
        }
        #endregion
    }
}
