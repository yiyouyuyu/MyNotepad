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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace MCode {
    /// <summary>
    /// MCode的主编辑区
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// 当前打开的文件路径
        /// </summary>
        private string FilePath { get; set; }

        /// <summary>
        /// 文本编辑框体
        /// </summary>
        private string MText {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        /// <summary>
        /// 系统托盘图标
        /// </summary>
        private System.Windows.Forms.NotifyIcon MNotifyIcon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            Title = "未命名 - MCode";
            MNotifyIcon = new System.Windows.Forms.NotifyIcon {
                Icon = Properties.Resources.icon,
                Text = @"MCode",
                BalloonTipText = @"刚刚的文件没有保存"
            };
        }
        
        /// <summary>
        /// 移动窗口
        /// </summary>
        private void WindowMove(object sender, MouseButtonEventArgs e) {
            if(WindowState == WindowState.Maximized) {
                ChangeWindowState();
                //因为拖动区域在窗口顶部，所以移动到最上面就行了
                Point mousePoint = Mouse.GetPosition(this);
                Left = mousePoint.X - Width * mousePoint.X / SystemParameters.WorkArea.Width;
                Top =0;
            }
            DragMove();
            if (WindowState == WindowState.Maximized) {
                mWindow.Margin = new Thickness(7);
                windowSize.Source = new BitmapImage(new Uri("Resources/windowNormal32X24.ico", UriKind.Relative));
            }
        }

        /// <summary>
        /// 在状态栏跟踪光标位置
        /// </summary>
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e) {
            int index,
                row = 1,
                col = 1;
            //从光标处往前遍历
            for (index = textBox.SelectionStart - 1; index >= 0; index -= 1) {
                if (MText[index] == '\n') row += 1;
                if (row == 1) col += 1;
            }
            textBoxInformation.Content = " 第 " + row + " 行；第 " + col + " 列";
        }

        /// <summary>
        /// 最小化
        /// </summary>
        private void WindowMinimized_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 改变窗口大小
        /// </summary>
        private void WindowSize_Click(object sender, RoutedEventArgs e) {
            ChangeWindowState();
        }

        /// <summary>
        /// 改变窗口大小
        /// </summary>
        private void ChangeWindowState() {
            if (WindowState == WindowState.Normal) {
                WindowState = WindowState.Maximized;
                mWindow.Margin = new Thickness(7);
                windowSize.Source= new BitmapImage(new Uri("Resources/windowNormal32X24.ico", UriKind.Relative));
            } else {
                WindowState = WindowState.Normal;
                mWindow.Margin = new Thickness();
                windowSize.Source = new BitmapImage(new Uri("Resources/windowMaximized32X24.ico", UriKind.Relative));
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void WindowClose_Click(object sender, RoutedEventArgs e) {
            Exit();
        }

        /// <summary>
        /// 退出
        /// </summary>
        private void Exit() {
            if (FilePath == null) {
                MNotifyIcon.Visible = true;
                MNotifyIcon.ShowBalloonTip(6);
                MNotifyIcon.Visible = false;
            }
            Close();
        }

        private void Wrap_Click(object sender, RoutedEventArgs e) {
            if (textBox.TextWrapping == TextWrapping.NoWrap) {
                textBox.TextWrapping = TextWrapping.Wrap;
                wrapAuto.Source = new BitmapImage(new Uri("Resources/check32.ico", UriKind.Relative));
            } else {
                textBox.TextWrapping = TextWrapping.NoWrap;
                wrapAuto.Source = null;
            }
        }
    }
}
