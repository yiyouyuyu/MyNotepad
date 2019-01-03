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
using System.Windows.Forms;
using System.IO;

namespace MCode {
    public partial class MainWindow : Window {

        /// <summary>
        /// File->New，新建
        /// </summary>
        private void New_Executed(object sender, ExecutedRoutedEventArgs e) {
            EditWindow newFile = new EditWindow();
            newFile.MTextBox.SelectionChanged += TextBox_SelectionChanged;
            newFile.MTextBox.GotFocus += TextBox_SelectionChanged;
            Files.Add(newFile);
            EditControl.Items.Add(newFile);
            editControl.SelectedItem = newFile;
            newFile.MTextBox.Focus();
        }

        /// <summary>
        /// File->Open，打开文件
        /// </summary>
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e) {
            //选择文件
            var openFileDialog = new OpenFileDialog() {
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {

                EditWindow newFile = new EditWindow(openFileDialog.FileName);
                newFile.MTextBox.SelectionChanged += TextBox_SelectionChanged;
                newFile.MTextBox.GotFocus += TextBox_SelectionChanged;
                Files.Add(newFile);
                EditControl.Items.Add(newFile);
                editControl.SelectedItem = newFile;
                newFile.MTextBox.Focus();
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
        }

        /// <summary>
        /// File->Save，保存文件
        /// </summary>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e) {
            EditWindow mainEdit = (EditWindow)EditControl.SelectedItem;
            mainEdit.Save();
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
            var result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                EditWindow mainEdit = (EditWindow)EditControl.SelectedItem;
                mainEdit.Save(saveFileDialog.FileName);
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }

        }

        /// <summary>
        /// 退出
        /// </summary>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e) {
            EditControl.Items.Remove(EditControl.SelectedItem);
        }

    }
}