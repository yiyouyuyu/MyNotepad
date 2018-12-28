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
using System.Windows.Forms;
using System.IO;

namespace MCode {
    public partial class MainWindow : Window {

        /// <summary>
        /// File->Open，打开文件
        /// </summary>
        private void Open_Click(object sender, RoutedEventArgs e) {
            //选择文件
            var openFileDialog = new OpenFileDialog() {
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
            };
            //result是选择文件窗体的点击结果
            var result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                //文件路径
                FilePath = openFileDialog.FileName;
                var fileStream = new FileStream(FilePath, FileMode.Open);
                //每次读8192个字节
                var buffer = new byte[8192];
                //r是一次读取的量
                var r = fileStream.Read(buffer, 0, buffer.Length);
                //清空现有的textbox
                if (MTextBox.Text.Length > 0) {
                    MTextBox.Text = "";
                }
                while (r == 8192) {
                    //读满表示没读完
                    MTextBox.Text += Encoding.Default.GetString(buffer, 0, r);
                    r = fileStream.Read(buffer, 0, buffer.Length);
                }
                MTextBox.Text += Encoding.Default.GetString(buffer, 0, r);
                fileStream.Close();
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
        }

        /// <summary>
        /// File->Save，保存文件
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e) {
            if (FilePath == null) {
                //新创建的文件或者被SaveAs_Click调用
                var saveFileDialog = new SaveFileDialog() {
                    Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                    FileName = "未命名"
                };
                //result是保存文件窗体的点击结果
                var result = saveFileDialog.ShowDialog();
                if(result== System.Windows.Forms.DialogResult.OK) {
                    //文件路径
                    FilePath = saveFileDialog.FileName;
                } else {
                    //其他的情况，若要增加要使用switch
                    return;
                }
            }
            var fileStream = new FileStream(FilePath, FileMode.Create);
            var buffer = Encoding.Default.GetBytes(MTextBox.Text);
            fileStream.Write(buffer, 0, buffer.Length);//！！！这里有个问题，一次的量可能会超过int，要改！！！
            fileStream.Close();
        }
        
        /// <summary>
        /// 另存为
        /// </summary>
        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            //选择保存地址，暂时无法解决代码重用问题
            var saveFileDialog = new SaveFileDialog() {
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                FileName = "未命名"
            };
            //result是保存文件窗体的点击结果
            var result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                //文件路径
                FilePath = saveFileDialog.FileName;
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
            Save_Click(sender,e);
        }

        
    }
}