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
using System.Windows.Forms;
using System.Drawing;

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
        private NotifyIcon MNotifyIcon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            Title = "未命名 - MCode";
            MNotifyIcon = new NotifyIcon {
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
            }
            DragMove();
        }

        /// <summary>
        /// 在状态栏跟踪光标位置
        /// </summary>
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e) {
            //光标位置
            int index = textBox.SelectionStart;
            int row = 1,
                col = 1;
            //从光标处往前遍历
            int i;
            for (i = index - 1; i >= 0; i -= 1) {
                if (MText[i] == '\n') row += 1;
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
            } else {
                WindowState = WindowState.Normal;
                mWindow.Margin = new Thickness();
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

    }
}
