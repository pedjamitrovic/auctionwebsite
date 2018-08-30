namespace AuctionWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bid")]
    public partial class Bid
    {
        public Guid ID { get; set; }

        public Guid Bidder { get; set; }

        public Guid OnAuction { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TokenAmount { get; set; }

        public virtual Auction Auction { get; set; }

        public virtual User User { get; set; }
    }
}
