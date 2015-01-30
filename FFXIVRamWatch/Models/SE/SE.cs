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

        private bool LOCK { get; set; }
        private string SoundFile { get; set; }

        public Task PlaySeAsync(string soundFile)
        {
            if (soundFile != SoundFile && !LOCK)
            {
                LOCK = true;

                SoundFile = soundFile;
                soundFile = Path.GetFullPath(soundFile);
                if (!String.IsNullOrEmpty(soundFile) && File.Exists(soundFile))
                {
                    var task = Task.Factory.StartNew(() =>{
                        var player = new SoundPlayer(soundFile);
                        player.PlaySync();
                    });

                    task.ContinueWith(_ => { LOCK = false; });
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