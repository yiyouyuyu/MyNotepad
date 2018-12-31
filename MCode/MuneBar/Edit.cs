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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// 撤销
        /// </summary>
        private void Undo_Click(object sender, RoutedEventArgs e) {
            textBox.Undo();
        }

        /// <summary>
        /// 重做
        /// </summary>
        private void Redo_Click(object sender, RoutedEventArgs e) {
            textBox.Redo();
        }

        private void Find_Click(object sender, RoutedEventArgs e) {

        }

        /// <summary>
        /// 剪切
        /// </summary>
        private void Cut_Click(object sender, RoutedEventArgs e) {
            textBox.Cut();
        }
        
        /// <summary>
        /// 复制
        /// </summary>
        private void Copy_Click(object sender, RoutedEventArgs e) {
            textBox.Copy();
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        private void Paste_Click(object sender, RoutedEventArgs e) {
            textBox.Paste();
        }

        /// <summary>
        /// 插入系统时间
        /// </summary>
        private void Date_Click(object sender, RoutedEventArgs e) {
            int index=textBox.SelectionStart;
            MText = MText.Insert(index, DateTime.Now.ToString());
        }


    }
}
