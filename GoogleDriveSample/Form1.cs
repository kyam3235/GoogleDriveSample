using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleDriveSample
{
    public partial class Form1 : Form
    {
        private GoogleApi mGoogleApi;

        public Form1()
        {
            InitializeComponent();
            this.Text = $"{ProductName} Ver.{ProductVersion}";

            //インスタンス作成
            mGoogleApi = GoogleApi.GetInstance;
        }

        private void buttonGetFileList_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();

            //ファイル一覧の取得
            var files = mGoogleApi.GetFiles();
            if(files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    this.textBox1.AppendText($"{file.Name}({file.Id})\n");
                }
            }
            else
            {
                this.textBox1.Text = "No files found.";
            }
        }
    }
}
