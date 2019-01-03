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
using System.IO;

namespace MCode {
    /// <summary>
    /// EditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditWindow : TabItem {

        /// <summary>
        /// 此窗口对应的文件
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// 文本编辑框体
        /// </summary>
        public TextBox MTextBox {
            get => textBox;
            set => textBox = value;
        }

        private static int num = 1;
        /// <summary>
        /// 构造函数，新建，打开
        /// </summary>
        public EditWindow(string path=null) {
            InitializeComponent();
            if (path == null) {
                Header = "未命名" + num + ".txt";
                num += 1;
            } else {
                FilePath = path;
                //设置选项框名字
                Header = Path.GetFileName(FilePath);
                var fileStream = new FileStream(FilePath, FileMode.Open);
                var buffer = new byte[8192];
                var r = fileStream.Read(buffer, 0, buffer.Length);
                while (r == 8192) {
                    //读满表示没读完
                    MTextBox.Text += Encoding.Default.GetString(buffer, 0, r);
                    r = fileStream.Read(buffer, 0, buffer.Length);
                }
                MTextBox.Text += Encoding.Default.GetString(buffer, 0, r);
                fileStream.Close();
            }
        }
        
        /// <summary>
        /// File->Save，保存文件
        /// </summary>
        public void Save(string path=null) {
            if (path!=null) {
                FilePath = path;
                Header = Path.GetFileName(FilePath);
            } else if(FilePath==null) {
                var saveFileDialog = new System.Windows.Forms.SaveFileDialog() {
                    Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                    FileName = "未命名"
                };
                var result = saveFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    FilePath = saveFileDialog.FileName;
                    Header = Path.GetFileName(FilePath);
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
        /// 退出
        /// </summary>
        private void Close() {

        }

    }
}
