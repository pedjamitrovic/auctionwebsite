namespace AuctionWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    public partial class AuctionWithLastBid
    {
        public AuctionWithLastBid()
        {
        }
        public Guid AuctionID { get; set; }
        public string Title { get; set; }
        public DateTime? OpenedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public int Duration { get; set; }
        public decimal StartingPrice { get; set; }

        public Guid? BidID { get; set; }
        public Guid? Bidder { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? TokenAmount { get; set; }
    }
}
