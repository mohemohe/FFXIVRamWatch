using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace FFXIVRamWatch.Models
{
    static class AudioPlayer
    {
        private static WasapiOut _wasapiOut;

        public static void Play(string soundFile)
        {
            _wasapiOut = new WasapiOut(AudioClientShareMode.Shared, 250);
            _wasapiOut.Init(new AudioFileReader(soundFile));
            _wasapiOut.Play();
        }

        public static TimeSpan GetSEDuration(string fileName)
        {
            var afr = new AudioFileReader(fileName);
            return afr.TotalTime;
        }
    }
}
