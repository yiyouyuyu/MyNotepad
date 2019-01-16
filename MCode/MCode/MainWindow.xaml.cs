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
using SWForms = System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace MCode {
    /// <summary>
    /// MCode的主编辑区
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// 打开的文件
        /// </summary>
        private ItemCollection Files {
            get => EditControl.Items;
        }

        /// <summary>
        /// 文件控制
        /// </summary>
        private TabControl EditControl {
            get => editControl;
            set => editControl = value;
        }

        /// <summary>
        /// 系统托盘图标
        /// </summary>
        private SWForms.NotifyIcon MNotifyIcon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "MCode";
            MNotifyIcon = new SWForms.NotifyIcon
            {
                Icon = Properties.Resources.icon32x32,
                Text = "MCode",
                BalloonTipText = "刚刚的文件没有保存"
            };
        }

        /// <summary>
        /// 移动窗口
        /// </summary>
        private void WindowMove(object sender, MouseButtonEventArgs e) {
            if(WindowState is WindowState.Maximized) {
                ChangeWindowState();
                //因为拖动区域在窗口顶部，所以移动到最上面就行了
                Point mousePoint = Mouse.GetPosition(this);
                Left = mousePoint.X * (1 - (Width / SystemParameters.WorkArea.Width));
                Top =0;
            }
            DragMove();
        }

        /// <summary>
        /// 在状态栏跟踪光标位置
        /// </summary>
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e) {
            int index,
                row = 1,
                col = 1;
            if (EditControl.SelectedItem is EditWindow mainEdit) {
                //从光标处往前遍历
                for (index = mainEdit.MTextBox.SelectionStart - 1; index >= 0; index -= 1) {
                    if (mainEdit.MTextBox.Text[index] == '\n') row += 1;
                    if (row == 1) col += 1;
                }
                textBoxInformation.Content = $" 第 {row} 行；第 {col} 列";
            } else {
                textBoxInformation.Content = "";
            }
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
            if (WindowState is WindowState.Normal) {
                WindowState = WindowState.Maximized;
            } else {
                WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        private void WindowClose_Click(object sender, RoutedEventArgs e) {
            if (!Debugger.IsAttached) {
                foreach (EditWindow file in Files) {
                    if (file.IsChange) {
                        MNotifyIcon.Visible = true;
                        MNotifyIcon.ShowBalloonTip(6);
                        break;
                    }
                }
            }
            Close();
        }

        /// <summary>
        /// 自动换行
        /// </summary>
        private void Wrap_Click(object sender, RoutedEventArgs e) {
            if (wrapAuto.Source is null) {
                foreach (EditWindow file in Files) {
                    file.MTextBox.TextWrapping = TextWrapping.WrapWithOverflow;
                }
                wrapAuto.Source = new BitmapImage(new Uri("Resources/check32.ico", UriKind.Relative));
            } else {

                foreach (EditWindow file in Files) {
                    file.MTextBox.TextWrapping = TextWrapping.NoWrap;
                }
                wrapAuto.Source = null;
            }
        }

        /// <summary>
        /// File->New，新建
        /// </summary>
        private void New_Executed(object sender, ExecutedRoutedEventArgs e) {
            //没有路径
            CreatFile(null);
            TextBox_SelectionChanged(sender, e);
        }

        /// <summary>
        /// File->Open，打开文件
        /// </summary>
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e) {
            //选择文件
            var openFileDialog = new OpenFileDialog() {
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
            };
            bool? result = openFileDialog.ShowDialog();
            if (result == true) {
                //通过路径创建
                CreatFile(openFileDialog.FileName);
                TextBox_SelectionChanged(sender, e);
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
        }

        /// <summary>
        /// 创建编辑窗口实例
        /// </summary>
        private void CreatFile(string FilePath) {
            EditWindow newFile = new EditWindow(FilePath);
            newFile.UserEditEvent += TextBox_SelectionChanged;
            newFile.CloseEvent += Close_Click;
            Files.Add(newFile);
            EditControl.SelectedItem = newFile;
        }

        /// <summary>
        /// File->Save，保存文件
        /// </summary>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e) {
            EditWindow mainEdit = (EditWindow)EditControl.SelectedItem;
            mainEdit?.Save();
        }

        /// <summary>
        /// 另存为
        /// </summary>
        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e) {
            //选择保存地址
            var saveFileDialog = new SaveFileDialog() {
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                FileName = "未命名"
            };
            //result是保存文件窗体的点击结果
            bool? result = saveFileDialog.ShowDialog();
            if (result == true) {
                EditWindow mainEdit = (EditWindow)EditControl.SelectedItem;
                mainEdit.Save(saveFileDialog.FileName);
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
        }

        /// <summary>
        /// 从File->Close关闭
        /// </summary>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e) {
            Close_Click(EditControl.SelectedItem,e);
        }

        /// <summary>
        /// 点击编辑窗口的x关闭
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e) {
            if (sender is EditWindow t) {
                if (t.IsChange) {
                    MNotifyIcon.Visible = true;
                    MNotifyIcon.ShowBalloonTip(6);
                    MNotifyIcon.Visible = false;
                }
                Files.Remove(t);
            }
            TextBox_SelectionChanged(sender, e);
        }

        /// <summary>
        /// 插入系统时间
        /// </summary>
        private void Date_Click(object sender, RoutedEventArgs e) {
            EditWindow mainEdit = (EditWindow)EditControl.SelectedItem;
            int index = mainEdit.MTextBox.SelectionStart;
            mainEdit.MTextBox.Text = mainEdit.MTextBox.Text.Insert(index, DateTime.Now.ToString());
        }

        private void CanClose_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            if(EditControl.SelectedItem is null) {
                e.CanExecute = false;
            } else {
                e.CanExecute = true;
            }
        }

        private void CanSave_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            if (EditControl.SelectedItem is null) {
                e.CanExecute = false;
            } else {
                e.CanExecute = true;
            }
        }
    }
}
