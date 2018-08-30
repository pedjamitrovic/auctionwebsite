var auctionID;
$(document).ready(function () {
    var timeleft = parseInt($("#timeleft").html(), 10);
    if (isNaN(timeleft) || timeleft <= 0) {
        $(":button.btn-bid").prop("disabled", true);
    }
    setInterval(timer, 1000);
});
function timer(timerObj) {
    var timeleft = parseInt($("#timeleft").html(), 10);
    if (isNaN(timeleft)) return;
    $("#timeleft").html(timeleft > 0 ? timeleft - 1 : 0);
    if (timeleft <= 0) {
        $(":button.btn-bid").prop("disabled", true);
    }
}
function newBid(id, newprice, lastbidder_name, lastbidder_id, createdOn) {
    if (auctionID !== id) return;
    var bidHistory = $("#bidHistory tbody");
    bidHistory.prepend("<tr>" +
        "<td>" + createdOn + "</td>" +
        "<td>" + "<a href=\"" + window.location.protocol + "//" + window.location.host + "/Home/Profile/" + lastbidder_id + "\">" + lastbidder_name + "</a>" + "</td>" +
        "<td>" + newprice + "</td>" +
        "</tr>");
    $("#newPrice").val(newprice);
    $("#currentPrice").html(newprice);
    $("#lastBidder").html("<a href=\"" + window.location.protocol + "//" + window.location.host + "/Home/Profile/" + lastbidder_id + "\">" + lastbidder_name + "</a>");
    $("#" + this.id + " .price").html(this.price);
    $("#" + this.id + " .lastbidder").html("<a href=\"" + window.location.protocol + "//" + window.location.host + "/Home/Profile/" + this.lastbidder_id + "\">" + this.lastbidder_name + "</a>");
}
function onAuctionApproved(id, openedOn, timeleft) {
    if (auctionID !== id) return;
    $("#openedOn").html(openedOn);
    $("#timeleft").html(timeleft);
}
function onFinishAuction(id, closedOn) {
    if (auctionID !== id) return;
    $("#closedOn").html(closedOn);
    $("#timeleft").html("Closed.");
    $(":button.btn-bid").prop("disabled", true);
}