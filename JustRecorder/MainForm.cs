using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using JustRecorder.Core;
using JustRecorder.UI;

namespace JustRecorder
{
    public enum Status
    {
        Recording, Idle, Unknown
    }
    public partial class MainForm : Form
    {
        private readonly WaveSoundRecorder recorder = new WaveSoundRecorder();
        private readonly string fileName = "rec";
        private readonly string appName = "JustRecorder";

        public string DefaultSaveDirectory
        {
            get
            {
                return $"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}\\Record";
            }
        }

        public string CurrentSaveDirectory
        {
            get
            {
                return Properties.Settings.Default.SaveDirectory;
            }
        }

        public string SaveFilePath
        {
            get
            {
                return $"{CurrentSaveDirectory}\\{fileName}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.wav";
            }
        }

        public MainForm()
        {
            InitializeComponent();
            if (Properties.Settings.Default.SaveDirectory == "")
            {
                Properties.Settings.Default.SaveDirectory = DefaultSaveDirectory;
            }
            SetUIStatusText(Status.Idle);
            SetButtonStatus(Status.Idle);
            btnBrowse.Click += (s, e) =>
            {
                // NOTE: 録音ファイル保存先のディレクトリを開く
                try
                {
                    System.Diagnostics.Process.Start("EXPLORER.EXE", CurrentSaveDirectory);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                            "録音ファイルの保存先ディレクトリを開けませんでした。",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                }
            };
            btnRecord.Click += (s, e) =>
            {
                if (recorder.IsRecording)
                {
                    // Note: Stop recording
                    if (!recorder.StopRecording())
                    {
                        MessageBox.Show(
                            "録音の停止に失敗しました。",
                            "エラー",
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                        return;
                    }
                    timerLabel1.Stop();
                    SetUIStatusText(Status.Idle);
                    SetButtonStatus(Status.Idle);
                }
                else
                {
                    // Note: Start recording
                    if (!MakeDirectory(CurrentSaveDirectory))
                    {
                        MessageBox.Show(
                            "保存先ディレクトリの作成に失敗したため、録音を開始できませんでした。",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    if (!recorder.StartRecording(SaveFilePath))
                    {
                        MessageBox.Show(
                            "録音の開始に失敗しました。",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    timerLabel1.Start();
                    SetUIStatusText(Status.Recording);
                    SetButtonStatus(Status.Recording);
                }
            };
            btnSetting.Click += (s, e) =>
            {
                // NOTE: 設定画面を開く
                using(var sf = new SettingForm())
                {
                    sf.ShowDialog();
                }
            };
            FormClosing += (s, e) =>
            {
                // NOTE: 録音などの処理中に閉じられないようにする
                if (recorder.IsRecording)
                {
                    MessageBox.Show(
                        "アプリケーションを終了する前に録音を停止してください。",
                        "警告",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                Properties.Settings.Default.Save();
            };
        }

        private bool MakeDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                return true;
            }

            try
            {
                Directory.CreateDirectory(dirPath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void SetUIStatusText(Status st)
        {
            switch (st)
            {
                case Status.Idle:
                    {
                        btnRecord.Text = "Record";
                        Text = $"{appName} [Ready]";
                    }
                    break;
                case Status.Recording:
                    {
                        btnRecord.Text = "Stop";
                        Text = $"{appName} [Recording]";
                    }
                    break;
                default:
                    break;
            }
        }

        private void SetButtonStatus(Status st)
        {
            switch (st)
            {
                case Status.Idle:
                    {
                        btnSetting.Enabled = true;
                    }
                    break;
                case Status.Recording:
                    {
                        btnSetting.Enabled = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
