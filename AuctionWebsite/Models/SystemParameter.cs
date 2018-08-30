namespace AuctionWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemParameter
    {
        public Guid ID { get; set; }

        public int AuctionCount { get; set; }

        public int AuctionDuration { get; set; }

        public decimal SilverCount { get; set; }

        public decimal GoldCount { get; set; }

        public decimal PlatinumCount { get; set; }

        [Required]
        [StringLength(50)]
        public string Currency { get; set; }

        public decimal TokenValue { get; set; }
    }
}
