using System.Text;
using System.Windows.Controls;
using System.IO;
using SWForms = System.Windows.Forms;
using System.Windows;

namespace MCode {
    /// <summary>
    /// EditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditWindow : TabItem {

        /// <summary>
        /// 正在编辑的文件
        /// </summary>
        private string FilePath { get; set; }

        /// <summary>
        /// 已经保存的文件hash
        /// </summary>
        private int FileHash { get; set; }
        
        /// <summary>
        /// 文件名称
        /// </summary>
        private string FileName {
            get => textBlock.Text;
            set => textBlock.Text = value;
        }

        /// <summary>
        /// 文本编辑框体
        /// </summary>
        public TextBox MTextBox {
            get => textBox;
        }

        /// <summary>
        /// 在窗口内编辑，选择编辑窗口
        /// </summary>
        public event RoutedEventHandler UserEditEvent {
            add {
                MTextBox.SelectionChanged += value;
                MTextBox.GotFocus += value;
            }
            remove {
                MTextBox.SelectionChanged -= value;
                MTextBox.GotFocus -= value;
            }
        }

        private static int num = 1;
        /// <summary>
        /// 构造函数，新建，打开
        /// </summary>
        public EditWindow(string path) {
            InitializeComponent();
            if (path is null) {
                FileName = "未命名" + num + ".txt";
                num += 1;
                FileHash = MTextBox.Text.GetHashCode();
            } else {
                FilePath = path;
                //设置选项框名字
                FileName = Path.GetFileName(FilePath);
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
                FileHash = MTextBox.Text.GetHashCode();
            }
        }

        /// <summary>
        /// File->Save，保存文件
        /// </summary>
        public void Save(string path = null) {
            if (path != null) {
                FilePath = path;
                FileName = Path.GetFileName(FilePath);
            } else if (FilePath is null) {
                var saveFileDialog = new SWForms.SaveFileDialog() {
                    Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                    FileName = "未命名"
                };
                var result = saveFileDialog.ShowDialog();
                if (result is SWForms.DialogResult.OK) {
                    FilePath = saveFileDialog.FileName;
                    FileName = Path.GetFileName(FilePath);
                } else {
                    //其他的情况，若要增加要使用switch
                    return;
                }
            }
            var fileStream = new FileStream(FilePath, FileMode.Create);
            var buffer = Encoding.Default.GetBytes(MTextBox.Text);
            fileStream.Write(buffer, 0, buffer.Length);//！！！这里有个问题，一次的量可能会超过int，要改！！！
            fileStream.Close();
            FileHash = MTextBox.Text.GetHashCode();
        }
        
        /// <summary>
        /// 是否更改过
        /// </summary>
        public bool IsChange => FileHash != MTextBox.Text.GetHashCode();

        /// <summary>
        /// 关闭button所在的
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            CloseEvent?.Invoke(this, e);
        }
        public event RoutedEventHandler CloseEvent;
    }
}
