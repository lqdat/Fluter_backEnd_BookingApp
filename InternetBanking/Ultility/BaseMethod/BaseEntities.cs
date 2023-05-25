using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Ultility.BaseMethod
{
    public class AuditEntities
    {
        [NotMapped]
        public string Created_By { get; set; }
        [NotMapped]
        public DateTime Created_Date { get; set; }
        [NotMapped]
        public string Updated_By { get; set; }
        [NotMapped]
        public Nullable<DateTime> Updated_Date { get; set; }
    }
    public class BaseEntities : AuditEntities
    {
        public Guid ID { get; set; }

    }
}