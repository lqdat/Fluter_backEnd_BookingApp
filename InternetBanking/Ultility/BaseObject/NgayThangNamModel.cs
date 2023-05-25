using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Ultility.BaseObject
{
    public class NgayThangNamModel
    {
        public int Y { get; set; }
        public int M { get; set; }
        public int D { get; set; }
        public static List<NgayThangNamModel> JsonParse(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText)) return null;
            List<NgayThangNamModel> parseResult = JsonConvert.DeserializeObject<List<NgayThangNamModel>>(jsonText);
            return parseResult;
        }
        public static string JsonStringify(IEnumerable<NgayThangNamModel> mangNgayThangNams)
        {
            if (mangNgayThangNams == null || mangNgayThangNams.Count() == 0) return null;
            return JsonConvert.SerializeObject(mangNgayThangNams);
        }
        public static List<NgayThangNamModel> get_NgayThangNam(DateTime tungay, DateTime denngay, bool ngayLamViec = true, bool is48h = false)
        {
            if (denngay == null) return null;
            return Enumerable.Range(0, 1 + denngay.Subtract(tungay).Days)
                .Select(s => tungay.AddDays(s))
                .GroupBy(g => new { g.Year, g.Month }).Select(s => new NgayThangNamModel
                {
                    Y = s.Key.Year,
                    M = s.Key.Month,
                    D = s.Count(c => !ngayLamViec || ((is48h || c.DayOfWeek != DayOfWeek.Saturday) && c.DayOfWeek != DayOfWeek.Sunday))
                }).Where(w => w.D > 0).ToList();
        }
    }
}