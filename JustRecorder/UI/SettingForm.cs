using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JustRecorder.UI
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
            tbSaveDir.Text = Properties.Settings.Default.SaveDirectory;
            btnOK.Click += (s, e) =>
            {
                Properties.Settings.Default.SaveDirectory = tbSaveDir.Text;
                btnCancel.PerformClick();
            };
            btnCancel.Click += (s, e) =>
            {
                Close();
                Dispose();
            };
            btnSaveDirRef.Click += (s, e) =>
            {
                using(var fbd = new FolderBrowserDialog()
                {
                    Description = "保存先ディレクトリの選択",
                })
                {
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        tbSaveDir.Text = fbd.SelectedPath;
                    }
                }
            };
            FormClosing += (s, e) =>
            {
                e.Cancel = true;
            };
        }
    }
}
