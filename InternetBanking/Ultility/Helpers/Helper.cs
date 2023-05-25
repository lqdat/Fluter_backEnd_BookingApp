using BookingApp.DataProvider.EF;
using BookingApp.Ultility.BaseControllers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;

namespace BookingApp.Ultility.Helpers
{
    public class Helper
    {
        private static Entities db = new Entities();

        public class Message
        {
            public const string PATCH_IS_NULL = "patch is null";
            public const string PASSWORD_IS_NOT_NULL = "Mật khẩu không được rỗng";
        }

        public static class MD5
        {
            private static readonly string SECRET = "_DGT";

            public static string Crypto(string chuoi)
            {
                //Hash the password
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedDataBytes;
                UTF8Encoding encoder = new UTF8Encoding();
                hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(chuoi));
                string str;
                str = BitConverter.ToString(hashedDataBytes).Replace("-", String.Empty).ToLower();
                return str;
            }

            public static string CryptoPassword(string password)
            {
                return Crypto(password + SECRET);
            }
        }

        public static string Query(string query)
        {
            var q = query.Replace("\r\n", "");
            q = q.Replace("\t", "");
            return q;
        }

       

        public static string getUserName()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return identity?.FindFirst("ma_tai_khoan")?.Value ?? (identity?.IsAuthenticated == true ? "Anonymous" : "Public");
        }
        public static T AddOrUpdate<T>(T source, T to)
        {
            var type = typeof(T);

            foreach (var prop in type.GetProperties().Where(w => w.CanRead && w.CanWrite))
            {
                var selfValue = type.GetProperty(prop.Name).GetValue(source);
                var toValue = type.GetProperty(prop.Name).GetValue(to);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                {
                    type.GetProperty(prop.Name).SetValue(source, toValue);
                }
            }
            return source;
        }

        public static string PhanCachHangNgan(object str)
        {
            try
            {
                decimal so = Convert.ToDecimal(str);
                bool so_am = so < 0;
                if (so_am)
                {
                    return "(" + (-1 * so).ToString("0,000").Replace(",", ".") + ")";
                }
                if (Math.Abs(so) == 0)
                {
                    return "-";
                }
                if (Math.Abs(so) >= 1000)
                    return so.ToString("0,000").Replace(",", ".");
                else
                    return Math.Round(so, 0).ToString();
            }
            catch { }
            return "";
        }

        public static string FormatDatetime(DateTime? d)
        {
            return d != null ? d.Value.ToString("dd/MM/yyyy") : "";
        }

        public static string FormatNgayGio(DateTime? d)
        {
            return d != null ? d.Value.ToString("dd/MM/yyyy HH:mm") : "";
        }
        public static DateTime ParseTime(string date)
        {
            return Convert.ToDateTime(date);
        }
        public static bool CheckDate(string date)
        {
            DateTime dDate;
            return DateTime.TryParse(date, out dDate);
        }
        // Trả về ngày cuối cùng của tháng.
        public static int GetLastDayInMonth(int year, int month)
        {
            DateTime aDateTime = new DateTime(year, month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            return retDateTime.Day;
        }
        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        /*        public static List<TreeSanPhamModel> BuildTree(List<DanhMucModel> q, Guid? parent_ID)
                {
                    var list = q.Where(item => item.Parent_ID == parent_ID)
                        .Select(item => new TreeSanPhamModel
                        {
                            key = item.ID.ToString(),
                            title = (item.STT != null ? item.STT + ". " : "") + item.Ten,
                            children = BuildTree(q, item.ID),
                            orderno = item.STT
                        })
                        .OrderBy(item => item.orderno)
                        .ThenBy(item => item.title).ToList();

                    return list;
                }*/

        public static Image ResizeImageKeepAspectRatio(Image source, int width, int height)
        {
            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = (Image)target.Clone();
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = (Image)source.Clone();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
        public static string SaveFileFromBase64(string base64, string folder, string title, bool crop = false)
        {
            int pos = base64.IndexOf(",") + 1;
            base64 = base64.Substring(pos, base64.Length - pos);
            byte[] bytes = Convert.FromBase64String(base64);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                var folderDomain = HttpContext.Current.Server.MapPath("~/Uploads/" + BaseController.MaDonVi);
                if (!Directory.Exists(folderDomain))
                {
                    Directory.CreateDirectory(folderDomain);
                }
                var foldderControl = Path.Combine(folderDomain, folder);
                if (!Directory.Exists(foldderControl))
                {
                    Directory.CreateDirectory(foldderControl);
                }
                var path = Path.Combine(foldderControl, title + ".jpg");
                if (crop)
                {
                    using (var resizeImg = ResizeImageKeepAspectRatio(image, 800, 800))
                    {
                        resizeImg.Save(path);
                    }
                }
                else
                {
                    image.Save(path);
                }

                return path.Replace(HttpContext.Current.Server.MapPath("~/"), "/").Replace("\\", "/");
            }
        }

        public static string SaveFileFromBase64(string base64, string folder, string title, int? height, int? width, bool crop = false)
        {
            int pos = base64.IndexOf(",") + 1;
            base64 = base64.Substring(pos, base64.Length - pos);
            byte[] bytes = Convert.FromBase64String(base64);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                var folderDomain = HttpContext.Current.Server.MapPath("~/Uploads/" + BaseController.MaDonVi);
                if (!Directory.Exists(folderDomain))
                {
                    Directory.CreateDirectory(folderDomain);
                }
                var foldderControl = Path.Combine(folderDomain, folder);
                if (!Directory.Exists(foldderControl))
                {
                    Directory.CreateDirectory(foldderControl);
                }
                var path = Path.Combine(foldderControl, title);
                if (crop)
                {
                    using (var resizeImg = ResizeImageKeepAspectRatio(image, (int)height, (int)width))
                    {
                        resizeImg.Save(path);
                    }
                }
                else
                {
                    image.Save(path);
                }

                return path.Replace(HttpContext.Current.Server.MapPath("~/"), "/").Replace("\\", "/");
            }
        }

        public static string getBase64(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        /*  public static iTextSharp.text.Image buildLogo(float x, float y)
          {
              // Load the image (probably from your stream)
              string path = db.s.SingleOrDefault(dv => dv.MaDonVi == BaseController.MaDonVi).Thumbnail80;
              System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~\\template\\image\\80x80.png"));
              *//*System.Drawing.Image image2 = System.Drawing.Image.FromFile(path);*/
        /*using (Graphics g = Graphics.FromImage(image))
    {
        using (StringFormat string_format = new StringFormat())
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(SO.ToString(), new FontFamily("Arial"), 0, 16, new Point(90, 23),
                    string_format);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(Brushes.Red, path);
            }

            using (GraphicsPath path2 = new GraphicsPath())
            {
                path2.AddString(NGAY.Value.ToString("dd/MM/yyyy"), new FontFamily("Arial"), 0, 16, new Point(90, 43),
                    string_format);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(Brushes.Red, path2);
            }
        }
    }*//*

    iTextSharp.text.Image img = null;
    using (var ms = new MemoryStream())
    {
        image.Save(ms, image.RawFormat);
        img = iTextSharp.text.Image.GetInstance(ms.ToArray());
    }
    img.SetAbsolutePosition(x, y);

    return img;
}
*/
        public static string formatTienThanhChuoi(double so)
        {
            if (so == 0)
                return mNumText[0];
            string chuoi = "", hauto = "";
            Int64 ty;
            do
            {
                //Lấy số hàng tỷ
                ty = Convert.ToInt64(Math.Floor((double)so / 1000000000));
                //Lấy phần dư sau số hàng tỷ
                so = so % 1000000000;
                if (ty > 0)
                {
                    chuoi = DocHangTrieu(so, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(so, false) + hauto + chuoi;
                }
                hauto = " tỷ";
            } while (ty > 0);
            string s = chuoi + " đồng";

            return s.Substring(0, 2).ToUpper() + s.Substring(2);
        }

        private static string[] mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');

        private static string DocHangChuc(double so, bool daydu)
        {
            string chuoi = "";
            //Hàm để lấy số hàng chục ví dụ 21/10 = 2
            Int64 chuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
            //Lấy số hàng đơn vị bằng phép chia 21 % 10 = 1
            Int64 donvi = (Int64)so % 10;
            //Nếu số hàng chục tồn tại tức >=20
            if (chuc > 1)
            {
                chuoi = " " + mNumText[chuc] + " mươi";
                if (donvi == 1)
                {
                    chuoi += " mốt";
                }
            }
            else if (chuc == 1)
            {//Số hàng chục từ 10-19
                chuoi = " mười";
                if (donvi == 1)
                {
                    chuoi += " một";
                }
            }
            else if (daydu && donvi > 0)
            {//Nếu hàng đơn vị khác 0 và có các số hàng trăm ví dụ 101 => thì biến daydu = true => và sẽ đọc một trăm lẻ một
                chuoi = " lẻ";
            }
            if (donvi == 5 && chuc >= 1)
            {//Nếu đơn vị là số 5 và có hàng chục thì chuỗi sẽ là " lăm" chứ không phải là " năm"
                chuoi += " lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + mNumText[donvi];
            }
            return chuoi;
        }

        private static string DocHangTram(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng trăm ví du 434 / 100 = 4 (hàm Floor sẽ làm tròn số nguyên bé nhất)
            Int64 tram = Convert.ToInt64(Math.Floor((double)so / 100));
            //Lấy phần còn lại của hàng trăm 434 % 100 = 34 (dư 34)
            so = so % 100;
            if (daydu || tram > 0)
            {
                chuoi = " " + mNumText[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;
        }

        private static string DocHangTrieu(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng triệu
            Int64 trieu = Convert.ToInt64(Math.Floor((double)so / 1000000));
            //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
            so = so % 1000000;
            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " triệu";
                daydu = true;
            }
            //Lấy số hàng nghìn
            Int64 nghin = Convert.ToInt64(Math.Floor((double)so / 1000));
            //Lấy phần dư sau số hàng nghin
            so = so % 1000;
            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " nghìn";
                daydu = true;
            }
            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }
            return chuoi;
        }

        public static string convertStringFromVNToEN(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower().Replace(" ", "");
        }

        public static string ParstMediaTypeHeaderValue(string s)
        {
            switch (s)
            {
                case ".doc": return "application/msword";
                case ".dot": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".dotx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case ".docm": return "application/vnd.ms-word.document.macroEnabled.12";
                case ".dotm": return "application/vnd.ms-word.template.macroEnabled.12";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlt": return "application/vnd.ms-excel";
                case ".xla": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xltx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case ".xlsm": return "application/vnd.ms-excel.sheet.macroEnabled.12";
                case ".xltm": return "application/vnd.ms-excel.template.macroEnabled.12";
                case ".xlam": return "application/vnd.ms-excel.addin.macroEnabled.12";
                case ".xlsb": return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".pot": return "application/vnd.ms-powerpoint";
                case ".pps": return "application/vnd.ms-powerpoint";
                case ".ppa": return "application/vnd.ms-powerpoint";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".potx": return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case ".ppsx": return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case ".ppam": return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                case ".pptm": return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case ".potm": return "application/vnd.ms-powerpoint.template.macroEnabled.12";
                case ".ppsm": return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                case ".mdb": return "application/vnd.ms-access";
                case ".jpeg": return "image/jpeg";
                case ".jpg": return "image/jpeg";
                case ".jpe": return "image/jpeg";
                default: return "application/pdf";
            }
        }

        public static string getFolder(string MaDonVi, string sub_folder)
        {
            string upload_folder = HttpContext.Current.Server.MapPath("~/Uploads");
            if (!Directory.Exists(upload_folder))
            {
                Directory.CreateDirectory(upload_folder);
            }

            //Tạo thư mục cho đơn vị
            string donvi_folder = Path.Combine(upload_folder, MaDonVi);
            if (!Directory.Exists(donvi_folder))
            {
                Directory.CreateDirectory(donvi_folder);
            }

            //Tạo thư mục cho từng hạng mục
            string path_hangmuc_folder = Path.Combine(donvi_folder, sub_folder);
            if (!Directory.Exists(path_hangmuc_folder))
            {
                Directory.CreateDirectory(path_hangmuc_folder);
            }

            return path_hangmuc_folder;
        }

    }
}