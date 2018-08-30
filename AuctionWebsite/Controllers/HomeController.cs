using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWebsite.Hubs;
using AuctionWebsite.Models;
using log4net;

namespace AuctionWebsite.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("AuctionWebsiteLogger");

        public ActionResult Index()
        {
            return RedirectToAction("Auctions");
        }

        public ActionResult Auctions()
        {
            using (var ctx = new AuctionModel())
            {
                try
                {
                    ViewBag.Message = "Auctions";

                    List<AuctionWithLastBid> auctionsWithLastBid = new List<AuctionWithLastBid>();

                    IEnumerable<Auction> auctions = ctx.GetAllAuctions().OrderByDescending(o => o.OpenedOn.HasValue ? o.OpenedOn.Value.AddSeconds(o.Duration) : o.CreatedOn);

                    foreach (var auction in auctions)
                    {
                        var lastBid = auction.Bids.OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                        auctionsWithLastBid.Add(
                            new AuctionWithLastBid
                            {
                                AuctionID = auction.ID,
                                Title = auction.Title,
                                OpenedOn = auction.OpenedOn,
                                ClosedOn = auction.ClosedOn,
                                Duration = auction.Duration,
                                StartingPrice = auction.StartingPrice,
                                BidID = lastBid?.ID,
                                Bidder = lastBid?.Bidder,
                                FirstName = lastBid?.User.FirstName,
                                LastName = lastBid?.User.LastName,
                                TokenAmount = lastBid?.TokenAmount
                            }
                        );
                    }


                    SystemParameter system = ctx.GetSystemParameters();
                    ViewBag.AuctionCount = system.AuctionCount;

                    return View("Auctions", auctionsWithLastBid);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    ViewBag.ErrorMsg = "Internal server error.";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public ActionResult WonAuctions(string ID)
        {
            User loggedUser = (User)Session["user"];
            if (loggedUser == null || loggedUser.ID.ToString() != ID) return RedirectToAction("Auctions");
            using (var ctx = new AuctionModel())
            {
                try
                {
                    ViewBag.Message = "Auctions";

                    List<AuctionWithLastBid> auctionsWithLastBid = new List<AuctionWithLastBid>();

                    IEnumerable<Auction> auctions = ctx.GetWonAuctions(ID);

                    foreach (var auction in auctions)
                    {
                        var lastBid = auction.Bids.OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                        auctionsWithLastBid.Add(
                            new AuctionWithLastBid
                            {
                                AuctionID = auction.ID,
                                Title = auction.Title,
                                OpenedOn = auction.OpenedOn,
                                ClosedOn = auction.ClosedOn,
                                Duration = auction.Duration,
                                StartingPrice = auction.StartingPrice,
                                BidID = lastBid?.ID,
                                Bidder = lastBid?.Bidder,
                                FirstName = lastBid?.User.FirstName,
                                LastName = lastBid?.User.LastName,
                                TokenAmount = lastBid?.TokenAmount
                            }
                        );
                    }

                    SystemParameter system = ctx.GetSystemParameters();
                    ViewBag.AuctionCount = system.AuctionCount;

                    return View("WonAuctions", auctionsWithLastBid);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    ViewBag.ErrorMsg = "Internal server error.";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public new ActionResult Profile(string ID)
        {
            User loggedUser = (User)Session["user"];
            if (string.IsNullOrWhiteSpace(ID) && loggedUser != null)
            {
                ID = loggedUser.ID.ToString();
            }
            using (var ctx = new AuctionModel())
            {
                try
                {
                    ViewBag.Message = "Profile";

                    User user = ctx.GetUser(ID);

                    if (user == null)
                    {
                        ViewBag.ErrorMsg = "User with provided ID doesn't exist.";
                        return View("Error");
                    }

                    SystemParameter system = ctx.GetSystemParameters();
                    ViewBag.SilverCount = system.SilverCount;
                    ViewBag.GoldCount = system.GoldCount;
                    ViewBag.PlatinumCount = system.PlatinumCount;
                    ViewBag.Transactions = ctx.GetAllTranscations(user).OrderBy(o => o.State);

                    return View("Profile", user);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    ViewBag.ErrorMsg = "Internal server error.";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public ActionResult Auction(string ID)
        {
            using (var ctx = new AuctionModel())
            {
                try
                {
                    ViewBag.Message = "Auction";

                    Auction auction = ctx.GetAuction(ID);

                    if (auction == null)
                    {
                        ViewBag.ErrorMsg = "Auction with provided ID doesn't exist.";
                        return View("Error");
                    }

                    ViewBag.OwnerID = auction.User.ID;
                    ViewBag.OwnerFirstName = auction.User.FirstName;
                    ViewBag.OwnerLastName = auction.User.LastName;

                    var bids = auction.Bids.OrderByDescending(o => o.CreatedOn).ToList();
                    ViewBag.Bids = bids;

                    if (bids.Count > 0)
                    {
                        ViewBag.LastBidderID = bids.First().User.ID;
                        ViewBag.LastBidderFirstName = bids.First().User.FirstName;
                        ViewBag.LastBidderLastName = bids.First().User.LastName;
                    }
                    return View("Auction", auction);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    ViewBag.ErrorMsg = "Internal server error.";
                    return View("Error");
                }
            }
        }

        public ActionResult System()
        {
            using (var ctx = new AuctionModel())
            {
                ViewBag.Message = "System";
                try
                {
                    SystemParameter system = ctx.GetSystemParameters();
                    return View("System", system);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    ViewBag.ErrorMsg = "Internal server error.";
                    return View("Error");
                }
            }
        }

        public ActionResult NewAuction()
        {
            try
            {
                ViewBag.Message = "NewAuction";
                return View("NewAuction");
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                ViewBag.ErrorMsg = "Internal server error.";
                return View("Error");
            }
        }

        [HttpPost]
        public string Login(string email, string password)
        {
            using (var ctx = new AuctionModel())
            {
                try
                {
                    User user = ctx.FindUserByEmail(email);

                    if (user == null) return "#Error: User with provided email doesn't exist.";
                    if (user.Password != Utility.EncodePassword(password)) return "#Error: Wrong password.";

                    Session["user"] = user;

                    return "You have been successfully logged in.";
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return "#Error: Internal server error.";
                }
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                return RedirectToAction("Auctions");
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                ViewBag.ErrorMsg = "Internal server error.";
                return View("Error");
            }
        }

        [HttpPost]
        public string Register(string firstname, string lastname, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(firstname)) return "#Error: You must provide first name!";
            if (string.IsNullOrWhiteSpace(lastname)) return "#Error: You must provide last name!";
            if (string.IsNullOrWhiteSpace(email) || !Utility.IsValidEmailAddress(email)) return "#Error: You must provide valid email!";
            if (string.IsNullOrWhiteSpace(password)) return "#Error: You must provide password!";
            using (var ctx = new AuctionModel())
            {
                try
                {
                    if (ctx.FindUserByEmail(email) == null)
                    {
                        User user = new User
                        {
                            ID = Guid.NewGuid(),
                            FirstName = firstname,
                            LastName = lastname,
                            Email = email,
                            Password = Utility.EncodePassword(password),
                            TokenAmount = Decimal.Zero,
                            Administrator = 0
                        };
                        ctx.Users.Add(user);
                        ctx.SaveChanges();
                        return "You have been successfully registered.";
                    }
                    else
                    {
                        return "#Error: User with provided email already exists!";
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return "#Error: Internal server error.";
                }
            }
        }

        [HttpPost]
        public string ChangeInfo(string userID, string firstname, string lastname, string email, string password)
        {
            User loggedUser = (User)Session["user"];
            if (loggedUser == null || loggedUser.ID.ToString() != userID) return "";
            if (string.IsNullOrWhiteSpace(firstname)) return "#Error: You must provide first name!";
            if (string.IsNullOrWhiteSpace(lastname)) return "#Error: You must provide last name!";
            if (string.IsNullOrWhiteSpace(email) || !Utility.IsValidEmailAddress(email)) return "#Error: You must provide valid email!";
            if (string.IsNullOrWhiteSpace(password)) return "#Error: You must provide password!";
            using (var ctx = new AuctionModel())
            {
                try
                {
                    User user = ctx.FindUserByID(userID);
                    if (user == null) return "#Error: User with provided ID doesn't exists!";
                    user.FirstName = firstname;
                    user.LastName = lastname;
                    user.Email = email;
                    user.Password = Utility.EncodePassword(password);
                    ctx.SaveChanges();
                    AuctionHub.HubContext.Clients.All.onChangeInfo(user.ID.ToString(), firstname + " " + lastname, email);
                    return "You have successfully changed info.";
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return "#Error: Internal server error.";
                }
            }
        }

        [HttpPost]
        public string CreateAuction(string productname, decimal startingprice, int? auctionduration)
        {
            if (!(Session["user"] is User user)) return "";
            if (string.IsNullOrWhiteSpace(productname)) return "#Error: You must provide product name!";
            if (startingprice <= 0) return "#Error: You must provide starting price greater than zero!";
            if (auctionduration != null && auctionduration <= 0) return "#Error: You must provide auction duration greater than zero!";
            if (auctionduration <= 0) return "#Error: You must provide auction duration greater than zero!";

            Guid guid = Guid.NewGuid();
            if (Request.Files["image"] != null && Request.Files["image"].ContentType == "image/png")
            {
                Directory.CreateDirectory(Server.MapPath("~/resources/storage/auctions/" + guid.ToString() + "/"));
                Request.Files["image"].SaveAs(Server.MapPath("~/resources/storage/auctions/" + guid.ToString() + "/image.png"));
            }
            else
            {
                return "#Error: You must provide one .png picture of product!";
            }

            using (var ctx = new AuctionModel())
            {
                try
                {
                    SystemParameter system = ctx.GetSystemParameters();
                    Auction auction = new Auction
                    {
                        ID = guid,
                        Title = productname,
                        Duration = auctionduration ?? system.AuctionDuration,
                        Currency = system.Currency,
                        TokenValue = system.TokenValue,
                        StartingPrice = startingprice,
                        CreatedOn = DateTime.Now.AddHours(2),
                        Owner = user.ID
                    };
                    ctx.Auctions.Add(auction);
                    ctx.SaveChanges();
                    AuctionHub.HubContext.Clients.All.onCreateAuction(auction.ID.ToString(), auction.Title, 0, auction.StartingPrice, "", "", 0, 1);
                    return "You have successfully created auction.";
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return "#Error: Internal server error.";
                }
            }
        }

        [HttpPost]
        public string ChangeSystemParameters(int auctioncount, int auctionduration, decimal silvercount, decimal goldcount, decimal platinumcount, string currency, decimal tokenvalue)
        {
            if (!(Session["user"] is User user) || user.Administrator == 0) return "";
            if (auctioncount < 1 || auctioncount > 20) return "#Error: N must be a value between 1 and 20!";
            if (auctionduration <= 0) return "#Error: D must be a value greater than zero!";
            if (silvercount <= 0 || goldcount <= 0 || platinumcount <= 0) return "#Error: S, G and P must be values greater than zero!";
            if (string.IsNullOrWhiteSpace(currency)) return "#Error: You must provide currency!";
            if (tokenvalue <= 0) return "#Error: T must be a value greater than zero!";
            using (var ctx = new AuctionModel())
            {
                try
                {
                    SystemParameter system = ctx.GetSystemParameters();
                    system.AuctionCount = auctioncount;
                    system.AuctionDuration = auctionduration;
                    system.SilverCount = silvercount;
                    system.GoldCount = goldcount;
                    system.PlatinumCount = platinumcount;
                    system.Currency = currency;
                    system.TokenValue = tokenvalue;
                    ctx.SaveChanges();
                    return "System parameters successfully changed.";
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return "#Error: Internal server error.";
                }
            }
        }

        [HttpPost]
        public string ApproveAuction(string ID)
        {
            if (!(Session["user"] is User user) || user.Administrator == 0) return "";
            using (var ctx = new AuctionModel())
            {
                try
                {
                    Auction auction = ctx.GetAuction(ID);
                    if (auction != null && auction.OpenedOn != null) return "#Error: Auction already opened.";
                    auction.OpenedOn = DateTime.Now.AddHours(2);
                    ctx.SaveChanges();
                    var timeleft = (auction.OpenedOn.Value.AddSeconds(auction.Duration) - DateTime.Now.AddHours(2)).TotalSeconds;
                    AuctionHub.HubContext.Clients.All.onAuctionApproved(auction.ID.ToString(), auction.OpenedOn.Value.ToString("dd.MM.yyyy - HH:mm:ss"), (int)timeleft);
                    return "Auction opened.";
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return "#Error: Internal server error.";
                }
            }
        }

        [HttpPost]
        public void OrderTokens(string userID, int package)
        {
            User loggedUser = (User)Session["user"];
            if (loggedUser == null || loggedUser.ID.ToString() != userID) return;
            using (var ctx = new AuctionModel())
            {
                try
                {
                    SystemParameter system = ctx.GetSystemParameters();

                    var tokenamount = decimal.Zero;
                    switch (package)
                    {
                        case 0:
                            tokenamount = system.SilverCount;
                            break;
                        case 1:
                            tokenamount = system.GoldCount;
                            break;
                        case 2:
                            tokenamount = system.PlatinumCount;
                            break;
                    }

                    TokenOrder order = new TokenOrder()
                    {
                        ID = Guid.NewGuid(),
                        Buyer = loggedUser.ID,
                        TokenAmount = tokenamount,
                        TokenValue = system.TokenValue,
                        Currency = system.Currency,
                        State = 0
                    };
                    ctx.TokenOrders.Add(order);
                    ctx.SaveChanges();
                    AuctionHub.HubContext.Clients.All.onOrderTokens(loggedUser.ID.ToString(), order.ID.ToString(), order.TokenAmount.ToString("F4"), order.Currency, order.TokenValue.ToString("F4"), order.State);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }
            }
        }

        [HttpGet]
        public ActionResult Notify(string reference, string status)
        {
            using (var ctx = new AuctionModel())
            {
                using (var transaction = ctx.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        TokenOrder order = ctx.FindTokenOrderByID(reference);
                        if (order == null) throw new Exception("Invalid order. ID = " + reference);
                        if (order.State != 0) throw new Exception("Order already processed. ID = " + reference);
                        switch (status)
                        {
                            case "success":
                                order.State = 1;
                                break;
                            case "failed":
                                order.State = 2;
                                break;
                            case "canceled":
                                order.State = 2;
                                break;
                        }
                        User user = ctx.FindUserByID(order.Buyer.ToString());
                        if (order.State == 1)
                        {
                            user.TokenAmount += order.TokenAmount;
                        }
                        ctx.SaveChanges();
                        transaction.Commit();
                        AuctionHub.HubContext.Clients.All.onNotify(user.ID.ToString(), order.ID.ToString(), order.State, user.TokenAmount.ToString("F4"));

                        try
                        {
                            Utility.SendEmail(user.Email, "Transaction processed #" + reference,
                                "Hello " + user.FirstName + "," +
                                Environment.NewLine + Environment.NewLine +
                                "Your transaction #" + order.ID.ToString() + " has been processed." + Environment.NewLine +
                                (order.State == 1 ? "Status: COMPLETED" : "Status: CANCELED") + Environment.NewLine + Environment.NewLine +
                                "Sincerely, " + Environment.NewLine +
                                "The Auction Website Team"
                                );
                        }
                        catch (Exception e)
                        {
                            logger.Error(e.Message);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Error(e.Message);
                        transaction.Rollback();
                        ViewBag.ErrorMsg = "Internal server error.";
                        return View("Error");
                    }
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public string CreateBid(string userID, string auctionID, decimal newPrice)
        {
            User loggedUser = (User)Session["user"];
            if (loggedUser == null || loggedUser.ID.ToString() != userID) return "#Error: Please log in to create bid.";
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(auctionID)) return "";

            using (var ctx = new AuctionModel())
            {
                using (var transaction = ctx.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        Auction auction = ctx.GetAuction(auctionID);
                        if (auction == null) throw new InvalidActionException("#Error: Invalid auction ID.");
                        if (auction.Owner.ToString() == loggedUser.ID.ToString()) throw new InvalidActionException("#Error: You can't bid on your own auction.");
                        if (auction.OpenedOn == null) throw new InvalidActionException("#Error: Auction is not opened yet.");
                        if (auction.ClosedOn != null || auction.OpenedOn.Value.AddSeconds(auction.Duration) <= DateTime.Now.AddHours(2)) throw new InvalidActionException("#Error: Auction time has expired.");

                        var lastBid = auction.Bids.OrderByDescending(o => o.CreatedOn).ToList().FirstOrDefault();
                        if (lastBid != null && lastBid.Bidder.ToString() == loggedUser.ID.ToString()) throw new InvalidActionException("#Error: You are already last bidder.");
                        var currentPrice = lastBid?.TokenAmount ?? auction.StartingPrice;
                        if (currentPrice >= newPrice) throw new InvalidActionException("#Error: You can't create bid with price equal or lower than current price.");

                        User user = ctx.FindUserByID(userID);
                        if (user.TokenAmount < newPrice) throw new InvalidActionException("#Error: You don't have enough tokens to create bid.");

                        if (lastBid != null) lastBid.User.TokenAmount += lastBid.TokenAmount;
                        user.TokenAmount -= newPrice;

                        Bid bid = new Bid
                        {
                            ID = Guid.NewGuid(),
                            Bidder = user.ID,
                            OnAuction = auction.ID,
                            CreatedOn = DateTime.Now.AddHours(2),
                            TokenAmount = newPrice
                        };

                        ctx.Bids.Add(bid);
                        ctx.SaveChanges();
                        transaction.Commit();
                        AuctionHub.HubContext.Clients.All.refreshLastBidder(auction.ID.ToString(), newPrice, user.FirstName + " " + user.LastName, user.ID.ToString());
                        AuctionHub.HubContext.Clients.All.newBid(auction.ID.ToString(), newPrice.ToString("F4"), user.FirstName + " " + user.LastName, user.ID.ToString(), bid.CreatedOn.ToString("dd.MM.yyyy - HH:mm:ss"));
                        return "You have successfully created bid.";
                    }
                    catch (InvalidActionException e)
                    {
                        transaction.Rollback();
                        logger.Error(e.Message);
                        return e.Message;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        logger.Error(e.Message);
                        return e.Message;
                    }
                }
            }
        }

        [HttpPost]
        public string FinishAuction(string userID, string auctionID)
        {
            User loggedUser = (User)Session["user"];
            if (loggedUser == null || loggedUser.ID.ToString() != userID) return "#Error: Please log in to finish auction.";
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(auctionID)) return "";

            using (var ctx = new AuctionModel())
            {
                using (var transaction = ctx.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        Auction auction = ctx.GetAuction(auctionID);
                        if (auction == null) throw new InvalidActionException("#Error: Invalid auction ID.");
                        if (auction.Owner.ToString() != loggedUser.ID.ToString()) throw new InvalidActionException("#Error: You can't finish auction you didn't create.");
                        if (auction.OpenedOn == null) throw new InvalidActionException("#Error: Auction is not opened yet.");
                        if (auction.ClosedOn != null) throw new InvalidActionException("#Error: Auction is already finished.");
                        if (auction.OpenedOn.Value.AddSeconds(auction.Duration) >= DateTime.Now.AddHours(2)) throw new InvalidActionException("#Error: Wait for auction time to expire.");

                        auction.ClosedOn = DateTime.Now.AddHours(2);
                        var lastBid = auction.Bids.OrderByDescending(o => o.CreatedOn).ToList().FirstOrDefault();
                        if (lastBid != null)
                        {
                            User user = ctx.FindUserByID(userID);
                            user.TokenAmount += lastBid.TokenAmount;
                        }

                        ctx.SaveChanges();
                        transaction.Commit();
                        AuctionHub.HubContext.Clients.All.onFinishAuction(auction.ID.ToString(), auction.ClosedOn.Value.ToString("dd.MM.yyyy - HH:mm:ss"));
                        return "You have successfully finished auction.";
                    }
                    catch (InvalidActionException e)
                    {
                        transaction.Rollback();
                        logger.Error(e.Message);
                        return e.Message;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        logger.Error(e.Message);
                        return e.Message;
                    }
                }
            }
        }
    }
}

