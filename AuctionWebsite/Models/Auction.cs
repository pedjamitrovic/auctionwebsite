namespace AuctionWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Auction")]
    public partial class Auction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auction()
        {
            Bids = new HashSet<Bid>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public int Duration { get; set; }

        [Required]
        [StringLength(50)]
        public string Currency { get; set; }

        public decimal TokenValue { get; set; }

        public decimal StartingPrice { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? OpenedOn { get; set; }

        public DateTime? ClosedOn { get; set; }

        public Guid Owner { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
