using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCommonPicSharing
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Directory.Exists("MyPictures"))
            {

            }
            else
            {
                Directory.CreateDirectory("MyPictures");
                File.Create(@"MyPictures/推荐将图片统一保存在此文件夹.bmp");
            }
            picSource.AllowDrop = true;
        }

        private void 重启应用程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void 退出应用程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WebPicAddText.Text = null;
            using (OpenFileDialog S = new OpenFileDialog())
            {
                S.DefaultExt = "";
                S.Filter = "图像文件 (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
                S.InitialDirectory = "";
                if (S.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        picSource.Image = Image.FromFile(S.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("图像文件打开失败。原因如下：\n\n1.该图像文件被损坏；\n2.该文件不是有效的图像文件。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double u, x0;
            double.TryParse(textu.Text, out u);
            double.TryParse(textX0.Text, out x0);

            if (u < 3.57 || u > 4)
            {
                MessageBox.Show("参数u应大于等于3.57且小于等于4。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (x0 <= 0 || x0 >= 1)
            {
                MessageBox.Show("参数x应大于0且小于1。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Bitmap src = this.picSource.Image as Bitmap;
            if (src == null)
            {
                MessageBox.Show("请先载入需要处理的图像再进行操作。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            picDest.Image = Chaos.Encrypt(src, u, x0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap dest = this.picDest.Image as Bitmap;
            if (dest == null)
            {
                MessageBox.Show("你还没有进行图像处理，无法保存。请先点击“处理”按钮进行处理后再进行保存操作。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog S = new SaveFileDialog())
            {
                S.DefaultExt = "png";
                S.Filter = "图像文件 (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
                S.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "MyPictures";
                if (S.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        dest.Save(S.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("图像保存失败。请检查文件是否被安全软件拦截或者有相同的文件被占用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void picSource_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            this.picSource.Image = Image.FromFile(fileName);
        }

        private void picSource_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textu.Text = "3.7";
            textX0.Text = "0.123";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (WebPicAddText.Text == "")
            {
                MessageBox.Show("请输图片网址。\n\n格式：http://example.com/image.png(jpg/bmp)  (此为例子)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.picSource.LoadAsync(WebPicAddText.Text);
            }
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/KormiMeiko/OpenCommonPicSharing");
        }

        private void 使用帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/KormiMeiko/OpenCommonPicSharing/wiki");
        }

        private void 关于软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OpenCommonPicSharing\n版本：0.1.0\n编译日期：20200402\n\n作者：KormiMeiko\nQQ：1070050130\nE-Mail：KormiMeiko@gmail.com\n\n*本项目使用GPLv3协议开源");
        }
    }
}
