using PPTTimer.controls.Static;
using PPTTimer.windows.Setting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace PPTTimer
{
    public partial class MainWindow : Window
    {
        private string pathdata = "data/data.txt";
        public double defaulttime = 0;
        public double defaultwarntime = 0;

        public MainWindow()
        {
            InitializeComponent();
            Topmost = true;//窗口保持前置
            SystemIcon();//任务栏图标函数调用

            InitializeTimer();//初始化Timer
            Initialize();//初始化数据
        }

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
            notifyIcon.Icon = new System.Drawing.Icon("PPTTimer.ico");
            notifyIcon.Text = "PPTTimer";//鼠标在图标上显示的文本
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
            Save();
            this.Close();
        }
        //以上是托盘图标
        #endregion

        #region 窗口操作
        private void minimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//隐藏窗口
        {
            this.Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        }//实现窗口拖动，禁止放大与缩小
        #endregion

        #region 窗口等比例放缩
        //最后的宽度与高度
        private int LastWidth;
        private int LastHeight;
        //这个属性是指 窗口的宽度和高度的比例（宽度/高度）(240:118)
        private float AspectRatio = 2.0f / 1.0f;

        /// <summary>
        /// 捕获窗口拖拉消息
        /// (Capturing window drag messages)
        /// </summary>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = HwndSource.FromVisual(this) as HwndSource;
            if (source != null)
            {
                source.AddHook(new HwndSourceHook(WinProc));
            }
        }

        public const Int32 WM_EXITSIZEMOVE = 0x0232;

        /// <summary>
        /// 重载窗口消息处理函数
        /// (Overload window message processing function)
        /// </summary>
        private IntPtr WinProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            IntPtr result = IntPtr.Zero;
            switch (msg)
            {
                //处理窗口消息 (Handle window messages)
                case WM_EXITSIZEMOVE:
                    {
                        //上下拖拉窗口 (Drag window vertically)
                        if (this.Height != LastHeight)
                        {
                            this.Width = this.Height * AspectRatio;
                        }
                        // 左右拖拉窗口 (Drag window horizontally)
                        else if (this.Width != LastWidth)
                        {
                            this.Height = this.Width / AspectRatio;
                        }

                        LastWidth = (int)this.Width;
                        LastHeight = (int)this.Height;
                        break;
                    }
            }

            return result;
        }
        #endregion

        #region 各种操作按钮
        private void AddThing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EllipseEmerge();

        }//添加键

        private void restart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                change = 0;
                times = 0;
                timeDisplay(string.Empty, defaulttime);
                if (StartAndStop.IsStoping)
                {
                    SimulateStartAndStopMouseLeftButtonDown();
                }
                time.Foreground = new SolidColorBrush(Colors.Black);
            }
        }//重启键

        public void Receive(string a,string b)
        {
            defaulttime = Double.Parse(a);
            defaultwarntime = Double.Parse(b) ;
            Save();
            timeDisplay(string.Empty, defaulttime);
        }

        private void Setting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(this);
            settingWindow.sendMessage = Receive;
            settingWindow.Show();
        }
        //设置键
        #endregion

        #region 动画部分代码
        private void EllipseEmerge()
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
        }
        #endregion

        #region 退出
        //以下是退出部分的代码
        private double animationTime = 0.4;

        private void exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EllipseEmerge();

            var close = MyGrid.FindName("close") as StaticButton;
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

            var close = MyGrid.FindName("close") as StaticButton;
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
            string save = "默认倒计时时间=" + defaulttime.ToString() + "\n" + "默认问题时间=" + defaultwarntime.ToString();

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
            read = File.ReadAllLines(pathdata);
            defaulttime = ReadNum(read[0]);
            defaultwarntime = ReadNum(read[1]);
            timeDisplay(string.Empty, defaulttime);

            this.MouseEnter += MainWindow_MouseEnter;
            this.MouseLeave += MainWindow_MouseLeave;
        }

        private void MainWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)//设置移入移出鼠标显示与隐藏按钮
        {
            AddThing.Visibility = Visibility.Visible;
            StartAndStop.Visibility = Visibility.Visible;
            restart.Visibility = Visibility.Visible;
        }

        private void MainWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AddThing.Visibility = Visibility.Hidden;
            StartAndStop.Visibility = Visibility.Hidden;
            restart.Visibility = Visibility.Hidden;
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
            string judge = "";
            if (timeshow <= defaultwarntime && timeshow >= 0)//若小于警告时间则字体变蓝
            {
                time.Foreground = new SolidColorBrush(Colors.Blue);
            }
            else if (timeshow < 0)
            {
                time.Foreground = new SolidColorBrush(Colors.Red);
                timeshow = Math.Abs(timeshow);
                judge = "-";
            }
            time.Text = state + judge + timeFormat(Math.Floor(timeshow / 60).ToString())
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
        private double times = 0;

        private void StartAndStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!StartAndStop.IsStoping)
            {
                StartAndStop.ToolTip = "暂停";
                StartAndStop.IsStoping = true;
                starttime = DateTime.Now.ToLongTimeString();
                timer.Tick += timer_Tick;
                timer.Start();

                time.Opacity = 1.0;
                //time.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                StartAndStop.ToolTip = "开始计时";
                StartAndStop.IsStoping = false;
                starttime = nowtime;
                times = change;
                timer.Stop();

                time.Opacity = 0.6;
                //time.Foreground = new SolidColorBrush(Colors.Gray);

                timeDisplay(string.Empty, defaulttime - change);
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

            timeDisplay(string.Empty, defaulttime - change);
        }  //时间显示刷新
        #endregion

        #region 检查PPT是否打开
        private DispatcherTimer judgePPTRunning = new DispatcherTimer();
        private Application pptApp;
        private bool isPPTRunning = true;

        private void InitializePowerPoint()
        {
            try
            {
                isPPTRunning = true;
                pptApp = Marshal.GetActiveObject("PowerPoint.Application") as Application;
            }
            catch (COMException)
            {
                isPPTRunning = false;
            }
        }
        #endregion

        #region 判断PPT是否处于放映状态
        private DispatcherTimer ppttimer = new DispatcherTimer();
        private bool t;

        private void InitializeTimer()
        {
            timer.Interval = TimeSpan.FromSeconds(1);

            judgePPTRunning.Interval = TimeSpan.FromMilliseconds(1);
            judgePPTRunning.Tick += JudgePPTRunning_Tick;
            judgePPTRunning.Start();

            ppttimer.Interval = TimeSpan.FromMilliseconds(1); // 每1毫秒检查一次
            ppttimer.Tick += CheckSlideShowStatus;
            ppttimer.Start();
        }

        private void JudgePPTRunning_Tick(object sender, EventArgs e)
        {
            InitializePowerPoint();
        }

        private void CheckSlideShowStatus(object sender, EventArgs e)
        {
            bool isInSlideShow = false;
            if (isPPTRunning)
            {
                try
                {
                    isInSlideShow = pptApp.SlideShowWindows.Count > 0;
                }
                catch (COMException)
                {
                    isInSlideShow = false;  //PowerPoint 不在幻灯片放映模式下
                    t = false;
                }

                if (isInSlideShow && !StartAndStop.IsStoping && !t)
                {
                    t = true;
                    SimulateStartAndStopMouseLeftButtonDown();
                }
            }
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
