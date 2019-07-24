using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace JustRecorder.Core
{
    public class WaveSoundRecorder : IDisposable
    {
        private readonly WaveInEvent waveIn = new WaveInEvent();
        private WaveFileWriter writer = null;
        public bool IsRecording { get; private set; } = false;
        public WaveSoundRecorder()
        {
            waveIn.DataAvailable += (s, e) =>
            {
                writer?.Write(e.Buffer, 0, e.BytesRecorded);
                // TODO: 録音時間がヤバそうだったら自動で録音停止するなり出力ファイル切り替えるなりの処理をする
            };
            waveIn.RecordingStopped += (s, e) =>
            {
                writer?.Dispose();
                writer = null;
            };
        }

        public bool StartRecording(string path)
        {
            if (IsRecording) { return false; }

            writer = new WaveFileWriter(path, waveIn.WaveFormat);
            waveIn.StartRecording();
            IsRecording = true;

            return true;
        }

        public bool StopRecording()
        {
            if (!IsRecording) { return false; }

            waveIn.StopRecording();
            IsRecording = false;

            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。
                writer?.Dispose();
                writer = null;
                waveIn.Dispose();

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        ~WaveSoundRecorder()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
