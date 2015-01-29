using FFXIVRamWatch.Models.SE;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Serialization;

namespace FFXIVRamWatch.Models
{
    /// <summary>
    ///     XMLに書き出すための動的クラス
    /// </summary>
    public class Settings
    {
        public Point WindowPos { get; set; }

        public int Wait { get; set; }

        public int LowThreshold { get; set; }

        public int HighThreshold { get; set; }

        public SE.SE SE { get; set; }
    }

    /// <summary>
    ///     設定を読み書きするクラス
    /// </summary>
    public static class Config
    {
        #region Accessor

        public static Point WindowPos { get; set; }

        public static int Wait { get; set; }

        public static int LowThreshold { get; set; }

        public static int HighThreshold { get; set; }

        public static SE.SE SE { get; set; }

        #endregion Accessor

        private const string FileName = "Settings.xml";
        private static readonly string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string FilePath = Path.Combine(AppPath, FileName);

        public static void Initialize()
        {
            ReadSettings();
        }

        private static dynamic TryReadValue(dynamic source, dynamic check, dynamic defaultValue)
        {
            if (source != check)
            {
                return source;
            }
            return defaultValue;
        }

        /// <summary>
        ///     ファイルから設定を読み込む
        /// </summary>
        private static void ReadSettings()
        {
            var settings = new Settings();
            var xmls = new XmlSerializer(typeof (Settings));

            if (File.Exists(FilePath))
            {
                using (var fs = new FileStream(FilePath, FileMode.Open))
                {
                    settings = (Settings) xmls.Deserialize(fs);
                    fs.Close();
                }
            }

            WindowPos = TryReadValue(settings.WindowPos, new Point(0, 0), new Point(0, 0));
            Wait = TryReadValue(settings.Wait, 0, 1000);
            LowThreshold = TryReadValue(settings.LowThreshold, 0, 1700);
            HighThreshold = TryReadValue(settings.HighThreshold, 0, 1900);
            SE = TryReadValue(settings.SE, null,
                new SE.SE
                {
                    HighThreshold = new HighThreshold
                    {
                        SoundFile = @".\SE\high.wav",
                        Play = true
                    },
                    LowThreshold = new LowThreshold
                    {
                        SoundFile = @".\SE\low.wav",
                        Play = false
                    }
                });
        }

        /// <summary>
        ///     ファイルへ設定を書き込む
        /// </summary>
        public static void WriteSettings()
        {
            var xmls = new Settings
            {
                WindowPos = WindowPos,
                Wait = Wait,
                LowThreshold = LowThreshold,
                HighThreshold = HighThreshold,
                SE = new SE.SE
                {
                    LowThreshold = SE.LowThreshold,
                    HighThreshold = SE.HighThreshold
                }
            };

            var xs = new XmlSerializer(typeof (Settings));
            using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                xs.Serialize(fs, xmls);
            }
        }
    }
}