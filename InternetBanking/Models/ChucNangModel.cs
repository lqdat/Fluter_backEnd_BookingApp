using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiGDTVietnam.Models
{
    private class ChucNangModel
    {
        [Required]
        public Guid Id { get; set; }
        public string TenChucNang { get; set; }
        public bool Checked { get; set; }
        public int? STT { get; set; }
    }
}