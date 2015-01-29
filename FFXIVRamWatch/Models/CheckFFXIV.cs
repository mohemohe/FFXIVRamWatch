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

        public void Check()
        {
            try
            {
                Process ffxiv = Process.GetProcessesByName("ffxiv")[0];

                long privRam = ffxiv.PrivateMemorySize64/1024/1024;
                long virtRam = ffxiv.VirtualMemorySize64/1024/1024;

                ProcessId = ffxiv.Id.ToString();
                PrivateBytesInfomation = new BytesInfomation {Bytes = privRam, Color = SetColor(privRam)};
                VirtualBytesInfomation = new BytesInfomation {Bytes = virtRam, Color = SetColor(virtRam)};
            }
            catch
            {
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