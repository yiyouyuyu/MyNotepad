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
    /// MainWindow.xaml 的交互逻辑
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

        private string MTitle {
            get => title.Content.ToString();
            set => title.Content = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            MTitle = "未命名 - MCode";
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
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

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

    }
}
