using Countdown.controls.Close;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
using Button = System.Windows.Controls.Button;

namespace Countdown
{
    public partial class MainWindow : Window
    {
        private string pathdata = "data/data.txt";
        private double defaulttime = 0;
        private double defaultwarntime = 0;

        public MainWindow()
        {
            imageStart.ImageSource = new BitmapImage(new Uri(@"pic/start.png", UriKind.Relative));
            imageStart.Stretch = Stretch.Fill;//设置图像的显示格式
            imageStop.ImageSource = new BitmapImage(new Uri(@"pic/stop.png", UriKind.Relative));
            imageStop.Stretch = Stretch.Fill;//设置图像的显示格式

            InitializeComponent();
            Topmost = true;
            SystemIcon();//任务栏图标函数调用
            InitializePowerPoint();
            InitializeTimer();
            Initialize();
        }

        private ImageBrush imageStart = new ImageBrush();
        private ImageBrush imageStop = new ImageBrush();

        #region 托盘图标
        //以下是托盘图标
        internal NotifyIcon notifyIcon = new NotifyIcon();
        public void SystemIcon()
        {
            SetNotifyIcon();//设置托盘图标
            contextMenu();//托盘右键菜单设置
        }

        public void SetNotifyIcon()//设置托盘图标
        {
            notifyIcon.Icon = new System.Drawing.Icon("DoThingsRight.ico");
            notifyIcon.Text = "DoThingsRight";//鼠标在图标上显示的文本
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;//双击事件显示窗口

        }

        private void OnNotifyIconDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();//显示窗口
        }

        private void contextMenu()//托盘右键菜单设置
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            //关联 NotifyIcon 和 ContextMenuStrip
            notifyIcon.ContextMenuStrip = contextMenuStrip;

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem();
            exitMenuItem.Text = "退出";
            exitMenuItem.Click += exitMenuItem_Click;

            contextMenuStrip.Items.Add(exitMenuItem);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)//退出键
        {
            this.notifyIcon.Dispose();
            this.Close();
        }
        //以上是托盘图标
        #endregion

        #region 窗口操作，删除，添加，窗口前置

        private void minimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//隐藏窗口
        {
            this.Hide();
        }

        private const double MinimizedHeight = 110; //缩小后的高度
        private bool isMinimized = false; // 标记窗口是否已缩小

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // 界面空白处
            {
                if (isMinimized)
                    RestoreWindow();
                else
                    MinimizeWindow();
            }
            else
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    var windowMode = this.ResizeMode;
                    if (this.ResizeMode != ResizeMode.NoResize)
                    {
                        this.ResizeMode = ResizeMode.NoResize;
                    }
                    this.UpdateLayout();

                    DragMove();
                    if (this.ResizeMode != windowMode)
                    {
                        this.ResizeMode = windowMode;
                    }
                    this.UpdateLayout();
                }
            }
        }//实现窗口拖动，禁止放大与缩小

        private void MinimizeWindow()
        {
            isMinimized = true;

            //WindowState = WindowState.Normal;
            //Width = MinimizedWidth;
            Height = MinimizedHeight;

        }//缩小窗口

        private void RestoreWindow()
        {
            isMinimized = false;
            Height = 110;
        }//恢复窗口大小
        #endregion

        #region 各种操作按钮
        private void AddThing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }//添加键

        private void restart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                SimulateStartAndStopMouseLeftButtonDown();
                starttime= DateTime.Now.ToLongTimeString();
                nowtime= DateTime.Now.ToLongTimeString();
            }
        }//重启键

        private void Setting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        //设置键
        #endregion

        #region 退出
        //以下是退出部分的代码
        private double animationTime = 0.4;

        private void exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var exitEllipse = MyGrid.FindName("ExitEllipse") as EllipseGeometry;
            exitEllipse.Center = Mouse.GetPosition(this);
            var animationEllipse = new DoubleAnimation()
            {
                From = 0,
                To = 600,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime))
            };
            exitEllipse.BeginAnimation(EllipseGeometry.RadiusXProperty, animationEllipse);
            var animationOpacity = new DoubleAnimation()
            {
                From = 0,
                To = 0.8,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime))
            };
            var target = MyGrid.FindName("ExitPath") as System.Windows.Shapes.Path;
            target.BeginAnimation(System.Windows.Shapes.Path.OpacityProperty, animationOpacity);
            target.MouseLeftButtonDown += ExitPath_MouseLeftButtonDown;

            var close = MyGrid.FindName("close") as CloseButton;
            close.Visibility = Visibility.Visible;
        }//退出动画

        private void ExitPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//点击其他部位的动画
        {
            var target = sender as System.Windows.Shapes.Path;
            target.MouseLeftButtonDown -= ExitPath_MouseLeftButtonDown;

            close.Visibility = Visibility.Hidden;

            var exitEllipse = MyGrid.FindName("ExitEllipse") as EllipseGeometry;
            exitEllipse.Center = Mouse.GetPosition(this);

            var animationEllipse = new DoubleAnimation()
            {
                From = 600,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime))
            };
            exitEllipse.BeginAnimation(EllipseGeometry.RadiusXProperty, animationEllipse);
            var animationOpacity = new DoubleAnimation()
            {
                From = 0.9,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime))
            };
            //var target = MyGrid.FindName("ExitPath") as System.Windows.Shapes.Path;
            target.BeginAnimation(System.Windows.Shapes.Path.OpacityProperty, animationOpacity);
        }

        private void close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//退出
        {
            var exitPath = MyGrid.FindName("ExitPath") as System.Windows.Shapes.Path;
            exitPath.MouseLeftButtonDown -= ExitPath_MouseLeftButtonDown;

            var baseplate = MyGrid.FindName("baseplate") as Border;

            var exitEllipse = MyGrid.FindName("ExitEllipse") as EllipseGeometry;
            exitEllipse.Center = Mouse.GetPosition(this);

            baseplate.Opacity = 0;

            var animationEllipse = new DoubleAnimation()
            {
                From = 600,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime))
            };
            exitEllipse.BeginAnimation(EllipseGeometry.RadiusXProperty, animationEllipse);

            var close = MyGrid.FindName("close") as CloseButton;
            close.MouseLeftButtonDown -= close_MouseLeftButtonDown;

            var animationOpacity = new DoubleAnimation()
            {
                From = 0.9,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime))
            };
            MyGrid.BeginAnimation(System.Windows.Shapes.Path.OpacityProperty, animationOpacity);

            DispatcherTimer exitTimer = new DispatcherTimer();
            exitTimer.Interval = TimeSpan.FromMilliseconds(1);
            exitTimer.Tick += ExitTimer_Tick;
            exitTimer.Start();
        }

        private void ExitTimer_Tick(object sender, EventArgs e)//等待动画播放完毕后退出
        {
            if (MyGrid.Opacity == 0)
            {
                Save();
                this.notifyIcon.Dispose();
                this.Close();
            }
        }
        //以上是退出部分的代码
        #endregion

        #region 保存信息
        //如下一段为保存信息的代码
        public void Save()
        {
            string save = "默认倒计时时间="+defaulttime.ToString()+"\n"+"默认警告时间="+defaultwarntime.ToString();
            
            if (!StartAndStop.IsStoping)//保证未暂停状态下时间依旧能保存
            {
                times = change;
            }//保证未暂停状态下时间依旧能保存

            File.WriteAllText(pathdata, save);
        }

        private bool JudgeFormat(string format)//判断是否为时间格式
        {
            string[] s = format.Split('/');
            if (s.Length == 3 && s[0].Length == 4 && (s[1].Length >= 1 && s[1].Length <= 2)
                && (s[2].Length >= 1 && s[2].Length <= 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //如上一段为保存信息的代码
        #endregion

        #region 读取信息并初始化
        //如下一段为读取信息并初始化的代码
        private void Initialize()
        {
            string[] read;
            read=File.ReadAllLines(pathdata);
            defaulttime = ReadNum(read[0]);
            defaultwarntime = ReadNum(read[1]);
            timeDisplay(string.Empty,defaulttime);

            this.MouseEnter += MainWindow_MouseEnter;
            this.MouseLeave += MainWindow_MouseLeave;
        }

        private void MainWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AddThing.Visibility= Visibility.Visible;
            StartAndStop.Visibility= Visibility.Visible;
            restart.Visibility= Visibility.Visible;
        }

        private void MainWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AddThing.Visibility = Visibility.Hidden;
            StartAndStop.Visibility= Visibility.Hidden;
            restart.Visibility= Visibility.Hidden;
        }

        private double ReadNum(string data)
        {
            string s = data.Split('=')[1];
            return Double.Parse(s);
        }

        private void JudgeFileExists(string path)//判断文件是否存在，若不存在则创建
        {
            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                fs.Close();
            }
        }
        //如上一段为读取信息并初始化的代码
        #endregion

        #region 规范化时间显示
        private void timeDisplay(string state, double timeshow)//时间显示
        {
            if(timeshow <= defaultwarntime)
            {
                time.Foreground = new SolidColorBrush(Colors.Red);
            }
            time.Text = state + timeFormat(Math.Floor(timeshow / 60).ToString())
                + ":" + timeFormat(Math.Floor(timeshow % 60).ToString());
        }
        private string timeFormat(string s)
        {
            if (s.Length == 1)
            {
                return "0" + s;
            }
            else
            {
                return s;
            }
        }//规范化时间
        #endregion

        #region 控制计时
        public DispatcherTimer timer = new DispatcherTimer();
        public string starttime, nowtime;
        public bool isStop;//记录是否处于开始计时状态
        public double change;
        private double times=0;

        private void StartAndStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!StartAndStop.IsStoping)
            {
                StartAndStop.ToolTip = "暂停";
                StartAndStop.IsStoping = true;
                starttime = DateTime.Now.ToLongTimeString();
                timer.Tick += timer_Tick;
                timer.Start();
            }
            else
            {
                StartAndStop.ToolTip = "开始计时";
                StartAndStop.IsStoping= false;
                starttime = nowtime;
                times = change;
                timer.Stop();

                timeDisplay("暂停中", defaulttime - change);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            nowtime = DateTime.Now.ToLongTimeString();
            dopass();
        }
        public void dopass()
        {
            if (starttime.Length == 7)
            {
                starttime = "0" + starttime;
            }
            if (nowtime.Length == 7)
            {
                nowtime = "0" + nowtime;
            }

            char[] starts = starttime.ToCharArray();
            char[] nows = nowtime.ToCharArray();
            int shour, sminute, ssecond;
            int nhour, nminute, nsecond;

            shour = (starts[0] - 48) * 10 + starts[1] - 48;
            sminute = (starts[3] - 48) * 10 + starts[4] - 48;
            ssecond = (starts[6] - 48) * 10 + starts[7] - 48;

            nhour = (nows[0] - 48) * 10 + nows[1] - 48;
            nminute = (nows[3] - 48) * 10 + nows[4] - 48;
            nsecond = (nows[6] - 48) * 10 + nows[7] - 48;
            int changehour, changeminute, changesecond;

            changehour = nhour - shour;
            changeminute = nminute - sminute;
            changesecond = nsecond - ssecond;
            if (changehour >= 0)
            {
                change = changehour * 60 * 60 + changeminute * 60 + changesecond + times;
            }
            else
            {
                change = (changehour + 24) * 60 * 60 + changeminute * 60 + changesecond + times;
            }

            timeDisplay(string.Empty, defaulttime-change);
        }  //时间显示刷新
        #endregion

        #region 判断PPT是否处于放映状态
        private Application pptApp;
        private DispatcherTimer ppttimer;
        private void InitializePowerPoint()
        {
            try
            {
                pptApp = Marshal.GetActiveObject("PowerPoint.Application") as Application;
            }
            catch (COMException)
            {
                System.Windows.MessageBox.Show("PowerPoint is not running.");
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void InitializeTimer()
        {
            ppttimer = new DispatcherTimer();
            ppttimer.Interval = TimeSpan.FromMilliseconds(1); // 每1毫秒检查一次
            ppttimer.Tick += CheckSlideShowStatus;
            ppttimer.Start();
        }

        private void CheckSlideShowStatus(object sender, EventArgs e)
        {
            bool isInSlideShow = false;
            try
            {
                isInSlideShow = pptApp.SlideShowWindows.Count > 0;
            }
            catch (COMException)
            {
                isInSlideShow = false;  // PowerPoint 不在幻灯片放映模式下
            }

            if (isInSlideShow && !StartAndStop.IsStoping)
            {
                SimulateStartAndStopMouseLeftButtonDown();
            }
            //else if(!isInSlideShow && isStop)
            //{
            //    SimulateStartAndStopMouseLeftButtonDown();
            //    timeDisplay("暂停中", defaulttime - change);
            //}
        }

        private void SimulateStartAndStopMouseLeftButtonDown()
        {
            // 创建一个鼠标左键按下事件
            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            args.RoutedEvent = UIElement.MouseLeftButtonDownEvent;

            // 触发按钮的鼠标左键按下事件
            StartAndStop.RaiseEvent(args);
        }
        #endregion
    }

    public partial class App : System.Windows.Application
    {
        private static System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "OnlyRun_CRNS");
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);
            }
            else
            {
                this.Shutdown();
            }
        }
    }
}
