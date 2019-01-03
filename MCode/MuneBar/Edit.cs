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
        
        private void Find_Click(object sender, RoutedEventArgs e) {

        }
        
        /// <summary>
        /// 插入系统时间
        /// </summary>
        private void Date_Click(object sender, RoutedEventArgs e) {
            EditWindow mainEdit = (EditWindow)EditControl.SelectedItem;
            int index= mainEdit.MTextBox.SelectionStart;
            mainEdit.MTextBox.Text = mainEdit.MTextBox.Text.Insert(index, DateTime.Now.ToString());
        }

    }
}
