class AuctionItem {
    constructor(id, name, timeleft, price, lastbidder_name, lastbidder_id, generateApproveButton, status) {
        this.id = id;
        this.name = name;
        this.timeleft = timeleft;
        this.price = price;
        this.lastbidder_name = lastbidder_name;
        this.lastbidder_id = lastbidder_id;
        this.generateApproveButton = generateApproveButton;
        this.status = status;
    }

    toHTML() {
        var timeleftP;
        switch (this.status) {
            case 1:
                timeleftP = "<span class=\"text-warning\">Not opened yet.</span>";
                break;
            case 2:
                timeleftP = this.timeleft;
                break;
            case 3:
                timeleftP = "<span class=\"text-danger\">Closed.</span>";
                break;
        }
        return "" +
            "<div id=" + this.id + ">" +
            "<a href=\"" + window.location.protocol + "//" + window.location.host + "/Home/Auction/" + this.id + "\">" +
            "<img src=\"" + window.location.protocol + "//" + window.location.host + "/resources/storage/auctions/" + this.id + "/image.png\" width=\"100px\" height=\"100px\"/><p><b>" + this.name + "</b></p>" +
            "</a>" +
            "<b><p class=\"timeleft\">" + timeleftP + "</p></b>" +
            "<p style=\"color:green\"><span class=\"price\">" + this.price + "</span> t</p>" +
            "<p class=\"lastbidder\"><a href=\"" + window.location.protocol + "//" + window.location.host + "/Home/Profile/" + this.lastbidder_id + "\">" + this.lastbidder_name + "</a></p>" +
            "<button type=\"button\" class=\"btn btn-primary btn-bid\" onclick=\"auction('" + this.id + "')\"> Bid Now</button>" + " " +
            (this.generateApproveButton && this.status === 1 && isAdministrator ? "<button type=\"button\" class=\"btn btn-primary btn-approve\" onclick=\"approveAuction('" + this.id + "')\"> Approve</button>" : "") +
            "</div>";
    }
    refreshLastBidder(id, newprice, lastbidder_name, lastbidder_id) {
        if (this.id !== id) return;
        this.price = newprice;
        this.lastbidder_name = lastbidder_name;
        this.lastbidder_id = lastbidder_id;
        $("#" + this.id + " .price").html(this.price);
        $("#" + this.id + " .lastbidder").html("<a href=\"" + window.location.protocol + "//" + window.location.host + "/Home/Profile/" + this.lastbidder_id + "\">" + this.lastbidder_name + "</a>");
    }
    onAuctionApproved(id, openedOn, timeleft) {
        if (this.id !== id) return;
        this.timeleft = timeleft;
        this.status = 2;
        $("#" + this.id + " .timeleft").html(timeleft);
        $("#" + this.id + " :button.btn-bid").prop("disabled", false);
        $("#" + this.id + " :button.btn-approve").hide();
    }
    onFinishAuction(id, closedOn) {
        if (this.id !== id) return;
        this.status = 3;
        $("#" + this.id + " .timeleft").html("<span class=\"text-danger\">Closed.</span>");
        $("#" + this.id + " :button.btn-bid").prop("disabled", true);
    }
}

var pagination, itemarticles;
var pagination_pages = 0, active = 0, articlesPerPage = 5, isAdministrator = 0;
var auctionitems;
var filtereditems = [];

$(document).ready(function () {
    $("#newAuctionItem").hide();
    if (typeof auctionitems === "undefined") auctionitems = [];
    pagination = $("#auctions .pagination");
    itemarticles = $("#auctions article[id]");

    filtereditems = auctionitems.slice(0);

    pagination_pages = filtereditems.length / articlesPerPage;
    if (filtereditems.length % articlesPerPage !== 0) pagination_pages++;
    for (var i = 1; i <= pagination_pages; i++)
        pagination.append("<li><a class=\"cursor-pointer\" onclick=\"switchPagination(" + i + ");\">" + i + "</a></li>");

    switchPagination(1);
    setInterval(timer, 1000);
});

function timer() {
    auctionitems.forEach(function (item) {
        if (item.timeleft > 0) item.timeleft--;
    });
    $(".timeleft").each(function () {
        var timeleft = parseInt($(this).html());
        if (isNaN(timeleft)) {
            $(this).parent().siblings(":button.btn-bid").prop("disabled", true);
            return;
        }
        if (timeleft <= 0) {
            $(this).parent().siblings(":button.btn-bid").prop("disabled", true);
        }

        $(this).html(timeleft > 0 ? timeleft - 1 : 0);
    });
}

function refreshLastBidder(id, newprice, lastbidder_name, lastbidder_id) {
    auctionitems.forEach(function (item) {
        item.refreshLastBidder(id, newprice, lastbidder_name, lastbidder_id);
    });
}

function onCreateAuction(id, name, timeleft, price, lastbidder_name, lastbidder_id, generateApproveButton, status) {
    var i = 0;
    for (; i < auctionitems.length; i++) {
        if (auctionitems[i].status === 2) break;
    }
    auctionitems.splice(i + 1, 0, new AuctionItem(id, name, timeleft, price, lastbidder_name, lastbidder_id, generateApproveButton, status));
    $("#newAuctionItem").html("<center><a onclick=\"searchAuctions()\">There are new auctions available to check out. Click here to include them into search results.</a></center>");
    $("#newAuctionItem").show();
}

function onAuctionApproved(id, openedOn, timeleft) {
    auctionitems.forEach(function (item) {
        item.onAuctionApproved(id, openedOn, timeleft);
    });
    $("#newAuctionItem").html("<center><a onclick=\"searchAuctions()\">There are new auctions available to bid on. Click here to include them into search results.</a></center>");
    $("#newAuctionItem").show();
}

function onFinishAuction(id, closedOn) {
    auctionitems.forEach(function (item) {
        item.onFinishAuction(id, closedOn);
    });
}

function switchPagination(page) {

    $("#auctions").hide();
    itemarticles.show();

    pagination.find("li.active").removeClass("active");
    pagination.find("li:nth-child(" + page + ")").addClass("active");

    var j = 0;
    for (var i = (page - 1) * articlesPerPage; i < filtereditems.length && i < page * articlesPerPage; i++)
        itemarticles[j++].innerHTML = filtereditems[i].toHTML();

    $(".timeleft").each(function () {
        var timeleft = parseInt($(this).html());
        if (isNaN(timeleft)) {
            $(this).parent().siblings(":button.btn-bid").prop("disabled", true);
            return;
        }
        if (timeleft <= 0) {
            $(this).parent().siblings(":button.btn-bid").prop("disabled", true);
        }
    });

    for (; j < itemarticles.length; j++) $(itemarticles[j]).hide();
    $("#auctions").fadeIn();

    active = page;
}

function refreshItemArticles() {
    pagination_pages = 0;
    active = 0;

    pagination.empty();

    pagination_pages = filtereditems.length / articlesPerPage;
    if (filtereditems.length % articlesPerPage !== 0) pagination_pages++;
    for (var i = 1; i <= pagination_pages; i++)
        pagination.append("<li><a class=\"cursor-pointer\" onclick=\"switchPagination(" + i + ");\">" + i + "</a></li>");
    switchPagination(1);
}

function titleFilter(arr, title) {
    return arr.filter(function (e) { return e.name.toLowerCase().indexOf(title.toLowerCase()) !== -1; }).slice(0);
}

function minPriceFilter(arr, value) {
    return arr.filter(function (e) { return e.price >= value; }).slice(0);
}

function maxPriceFilter(arr, value) {
    return arr.filter(function (e) { return e.price <= value; }).slice(0);
}

function statusFilter(arr, status) {
    if (status === 0) return arr.slice(0);
    return arr.filter(function (e) { return e.status === status; }).slice(0);
}

function searchAuctions() {
    title = $("#search #title").val();
    minprice = $("#search #minprice").val();
    maxprice = $("#search #maxprice").val();
    status = $("#search #status").val();

    filtereditems = auctionitems.slice(0);

    if (title !== "") filtereditems = titleFilter(filtereditems, title);
    if (minprice !== "") filtereditems = minPriceFilter(filtereditems, parseInt(minprice, 10));
    if (maxprice !== "") filtereditems = maxPriceFilter(filtereditems, parseInt(maxprice, 10));
    filtereditems = statusFilter(filtereditems, parseInt(status, 10));
    refreshItemArticles();
    $("#newAuctionItem").hide();
}

