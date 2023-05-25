namespace BookingApp.Ultility.BaseObject
{
    public class CacheModel
    {
        public string k { get; set; }
        public string v { get; set; }
    }
    public class CacheWithEntity
    {
        public string name { get; set; }
        public CacheModel value { get; set; }
    }
    public class CacheCanHo
    {
        public string k { get; set; }
        public ChiTietCanHoCache v { get; set; }
    }
    public class ChiTietCanHoCache
    {
        public string name { get; set; }
        public string code { get; set; }
        public int soNguoi { get; set; }
        public int dangO { get; set; }
    }
}