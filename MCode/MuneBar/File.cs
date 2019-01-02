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
            FilePath = null;
            MText = "";
            Title = "未命名 - MCode";
        }
        /// <summary>
        /// File->Open，打开文件
        /// </summary>
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e) {
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
                if (MText.Length > 0) {
                    MText = "";
                }
                while (r == 8192) {
                    //读满表示没读完
                    MText += Encoding.Default.GetString(buffer, 0, r);
                    r = fileStream.Read(buffer, 0, buffer.Length);
                }
                MText += Encoding.Default.GetString(buffer, 0, r);
                fileStream.Close();
                Title = Path.GetFileName(FilePath) + " - MCode";
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
        }

        /// <summary>
        /// File->Save，保存文件
        /// </summary>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e) {
            if (FilePath == null) {
                //没路径还想保存，呸
                SaveAs_Executed(sender, e);
            }
            var fileStream = new FileStream(FilePath, FileMode.Create);
            var buffer = Encoding.Default.GetBytes(MText);
            fileStream.Write(buffer, 0, buffer.Length);//！！！这里有个问题，一次的量可能会超过int，要改！！！
            fileStream.Close();
            Title = Path.GetFileName(FilePath) + " - MCode";
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
                //文件路径
                FilePath = saveFileDialog.FileName;
            } else {
                //其他的情况，若要增加要使用switch
                return;
            }
            var fileStream = new FileStream(FilePath, FileMode.Create);
            var buffer = Encoding.Default.GetBytes(MText);
            fileStream.Write(buffer, 0, buffer.Length);//！！！这里有个问题，一次的量可能会超过int，要改！！！
            fileStream.Close();
            Title = Path.GetFileName(FilePath) + " - MCode";
        }

        /// <summary>
        /// 退出
        /// </summary>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e) {
            WindowClose_Click(sender, e);
        }

    }
}