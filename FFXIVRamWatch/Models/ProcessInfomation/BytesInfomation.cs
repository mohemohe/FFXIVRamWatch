namespace FFXIVRamWatch.Models.ProcessInfomation
{
    public class BytesInfomation
    {
        private long _Bytes;

        public long Bytes
        {
            get { return _Bytes; }
            set
            {
                _Bytes = value;
                if (value >= 0)
                {
                    BytesStr = value.ToString();
                }
                else
                {
                    BytesStr = "-";
                }
            }
        }

        public string BytesStr { get; set; }

        public string Color { get; set; }
    }
}