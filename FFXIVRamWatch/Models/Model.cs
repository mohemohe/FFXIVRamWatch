using Livet;
using Livet.EventListeners;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;

namespace FFXIVRamWatch.Models
{
    public class Model : NotificationObject
    {
        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */

        private readonly SE.SE _se;
        private readonly Timer _timer;
// ReSharper disable InconsistentNaming
        public CheckFFXIV checkFfxiv;
// ReSharper restore InconsistentNaming

        public Model()
        {
            checkFfxiv = new CheckFFXIV();
            _se = new SE.SE();

            var listener = new PropertyChangedEventListener(checkFfxiv);
            listener.RegisterHandler(UpdateHandlerProxy);

            _timer = new Timer();
            _timer.Elapsed += (sender, args) => checkFfxiv.Check();
            _timer.Interval = Config.Wait;
        }

        private void UpdateHandlerProxy(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "FFXIVchecked":
                    var worker = sender as CheckFFXIV;
                    PlaySE(worker);
                    break;
            }

            RaisePropertyChanged(e.PropertyName);
        }

        public void StartWatching()
        {
            _timer.Start();
        }

        private async void PlaySE(CheckFFXIV ffxivInfo)
        {
            if (ffxivInfo.PrivateBytesInfomation.Bytes >= Config.HighThreshold ||
                ffxivInfo.VirtualBytesInfomation.Bytes >= Config.HighThreshold)
            {
                if (Config.SE.HighThreshold.Play)
                {
                    try
                    {
                        await _se.PlaySeAsync(Config.SE.HighThreshold.SoundFile);
                    }
                    catch
                    {
                    }
                }
            }
            else if (ffxivInfo.PrivateBytesInfomation.Bytes >= Config.LowThreshold ||
                     ffxivInfo.VirtualBytesInfomation.Bytes >= Config.LowThreshold)
            {
                if (Config.SE.LowThreshold.Play)
                {
                    try
                    {
                        await _se.PlaySeAsync(Config.SE.LowThreshold.SoundFile);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    _se.SetSE(Config.SE.LowThreshold.SoundFile);
                }
            }
            else
            {
                _se.ResetSE();
            }
        }
    }
}