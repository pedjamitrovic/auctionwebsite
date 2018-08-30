namespace AuctionWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TokenOrder")]
    public partial class TokenOrder
    {
        public Guid ID { get; set; }

        public Guid Buyer { get; set; }

        public decimal TokenAmount { get; set; }

        public decimal TokenValue { get; set; }

        [Required]
        [StringLength(50)]
        public string Currency { get; set; }

        public int State { get; set; }

        public virtual User User { get; set; }
    }
}
