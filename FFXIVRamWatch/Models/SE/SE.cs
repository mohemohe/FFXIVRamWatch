using System;
using System.IO;
using System.Threading.Tasks;

namespace FFXIVRamWatch.Models.SE
{
    public class SE
    {
        public ThresholdBasedSE LowThreshold { get; set; }

        public ThresholdBasedSE HighThreshold { get; set; }

        public int ReNotificationIngnoreSeconds { get; set; }

        private DateTime LOCK { get; set; }
        private string SoundFile { get; set; }

        public SE()
        {
            if (Config.Initialized)
            {
                ReNotificationIngnoreSeconds = Config.SE.ReNotificationIngnoreSeconds;
                LOCK = DateTime.Now;
            }
        }

        public Task PlaySeAsync(string soundFile)
        {
            if (soundFile != SoundFile)
            {
                SoundFile = soundFile;
                if (LOCK < DateTime.Now)
                {
                    LOCK = DateTime.Now.AddSeconds(ReNotificationIngnoreSeconds).Add(AudioPlayer.GetSEDuration(soundFile));
                    soundFile = Path.GetFullPath(soundFile);
                    if (!String.IsNullOrEmpty(soundFile) && File.Exists(soundFile))
                    {
                        AudioPlayer.Play(soundFile);
                    }
                }
            }

            return null;
        }

        public void ResetSE()
        {
            SoundFile = null;
        }

        public void SetSE(string soundFile)
        {
            SoundFile = soundFile;
        }
    }
}