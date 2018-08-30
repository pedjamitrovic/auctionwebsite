namespace AuctionWebsite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public partial class AuctionModel : DbContext
    {
        public AuctionModel()
            : base("name=AuctionModel")
        {
        }

        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<SystemParameter> SystemParameters { get; set; }
        public virtual DbSet<TokenOrder> TokenOrders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>()
                .Property(e => e.TokenValue)
                .HasPrecision(16, 4);

            modelBuilder.Entity<Auction>()
                .Property(e => e.StartingPrice)
                .HasPrecision(16, 4);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.Bids)
                .WithRequired(e => e.Auction)
                .HasForeignKey(e => e.OnAuction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bid>()
                .Property(e => e.TokenAmount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.SilverCount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.GoldCount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.PlatinumCount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.TokenValue)
                .HasPrecision(16, 4);

            modelBuilder.Entity<TokenOrder>()
                .Property(e => e.TokenAmount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<TokenOrder>()
                .Property(e => e.TokenValue)
                .HasPrecision(16, 4);

            modelBuilder.Entity<User>()
                .Property(e => e.TokenAmount)
                .HasPrecision(16, 4);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bids)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Bidder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TokenOrders)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Buyer)
                .WillCascadeOnDelete(false);
        }

        public User FindUserByEmail(string email)
        {
            var query =
                from user in Users
                where user.Email == email
                select user;

            return query.SingleOrDefault();
        }

        public User FindUserByID(string ID)
        {
            var query =
                from user in Users
                where user.ID.ToString() == ID
                select user;

            return query.SingleOrDefault();
        }

        public SystemParameter GetSystemParameters()
        {
            var query =
                from sp in SystemParameters
                select sp;

            return query.SingleOrDefault();
        }

        public IEnumerable<Auction> GetAllAuctions()
        {
            var query =
                from a in Auctions
                select a;

            return query.ToList();
        }

        public IEnumerable<Auction> GetWonAuctions(string userID)
        {
            var query =
                from a in Auctions
                where a.ClosedOn != null && a.Bids.OrderBy(e => e.CreatedOn).FirstOrDefault().Bidder.ToString() == userID.ToString()
                select a;

            return query.ToList();
        }

        public Auction GetAuction(string ID)
        {
            var query =
                from a in Auctions.Include("Bids.User")
                where a.ID.ToString() == ID
                select a;

            return query.SingleOrDefault();
        }

        public User GetUser(string ID)
        {
            var query =
                from u in Users
                where u.ID.ToString() == ID
                select u;

            return query.SingleOrDefault();
        }

        public IEnumerable<TokenOrder> GetAllTranscations(User user)
        {
            var query =
                from to in TokenOrders
                where to.User.ID.ToString() == user.ID.ToString()
                select to;

            return query.ToList();
        }

        public TokenOrder FindTokenOrderByID(string ID)
        {
            var query =
                from to in TokenOrders
                where to.ID.ToString() == ID
                select to;

            return query.SingleOrDefault();
        }
    }
}
