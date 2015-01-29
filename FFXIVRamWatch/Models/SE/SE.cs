using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace FFXIVRamWatch.Models.SE
{
    public class SE
    {
        public LowThreshold LowThreshold { get; set; }

        public HighThreshold HighThreshold { get; set; }

        private string SoundFile { get; set; }

        public Task PlaySeAsync(string soundFile)
        {
            if (soundFile != SoundFile)
            {
                SoundFile = soundFile;
                soundFile = Path.GetFullPath(soundFile);
                if (!String.IsNullOrEmpty(soundFile) && File.Exists(soundFile))
                {
                    var player = new SoundPlayer(soundFile);
                    player.PlaySync();
                }
            }
            return null;
        }

        public void ResetSE()
        {
            SoundFile = null;
        }
    }
}