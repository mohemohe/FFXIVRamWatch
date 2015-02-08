using FFXIVRamWatch.Models.ProcessInfomation;
using Livet;
using System.Diagnostics;

namespace FFXIVRamWatch.Models
{
    public class CheckFFXIV : NotificationObject
    {
        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */

// ReSharper disable InconsistentNaming
        private const string WHITE = "#FFFFFFFF";
        private const string YELLOW = "#FFFFFF00";
        private const string RED = "#FFFF0000";
        private const string SEARCHING = "searching";
// ReSharper restore InconsistentNaming

        public string ProcessId { get; set; }

        public BytesInfomation PrivateBytesInfomation { get; set; }

        public BytesInfomation VirtualBytesInfomation { get; set; }

        private Process ffxivProcess { get; set; }

        public void Check()
        {
            if (ffxivProcess == null)
            {
                ffxivProcess = Process.GetProcessesByName("ffxiv")[0];
            }

            try
            {
                ffxivProcess.Refresh();
                long privRam = ffxivProcess.PrivateMemorySize64 / 1024 / 1024;
                long virtRam = ffxivProcess.VirtualMemorySize64 / 1024 / 1024;

                ProcessId = ffxivProcess.Id.ToString();
                PrivateBytesInfomation = new BytesInfomation {Bytes = privRam, Color = SetColor(privRam)};
                VirtualBytesInfomation = new BytesInfomation {Bytes = virtRam, Color = SetColor(virtRam)};
            }
            catch
            {
                ffxivProcess = null;
                ProcessId = SEARCHING;
                PrivateBytesInfomation = new BytesInfomation {Bytes = -1, Color = WHITE};
                VirtualBytesInfomation = new BytesInfomation {Bytes = -1, Color = WHITE};
            }
            finally
            {
                RaisePropertyChanged("FFXIVchecked");
            }
        }

        private string SetColor(long val)
        {
            if (val >= Config.HighThreshold)
            {
                return RED;
            }
            if (val >= Config.LowThreshold)
            {
                return YELLOW;
            }
            return WHITE;
        }
    }
}