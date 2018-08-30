function processResponse(response, divId, refreshTimeout = -1) {
    if (response.startsWith("#Error: ")) {
        $("#" + divId).html("<div class=\"alert alert-danger\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>" + response.substring(8) + "</center></div>");
        return;
    }

    $("#" + divId).html("<div class=\"alert alert-success\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>" + response + "</center></div>");
    if (refreshTimeout > 0) setTimeout(function () { window.location.reload(); }, refreshTimeout);
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

function login() {
    var email = $("#loginModal #email").val();
    var password = $("#loginModal #password").val();

    if (email === "" && password === "") {
        $("#loginModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter email and password!</center></div>");
        return;
    }
    if (email === "") {
        $("#loginModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter email!</center></div>");
        return;
    }
    if (password === "") {
        $("#loginModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter password!</center></div>");
        return;
    }
    if (!validateEmail(email)) {
        $("#loginModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>Invalid email address!</center></div>");
        return;
    }

    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/Login",
            method: "POST",
            data: { email: email, password: password },
            dataType: "text",
            success: response => processResponse(response, "loginModalAlert", 700)
        });
}

function register() {
    var firstname = $("#registerModal #firstname").val();
    var lastname = $("#registerModal #lastname").val();
    var email = $("#registerModal #email").val();
    var password = $("#registerModal #password").val();

    if (firstname === "") {
        $("#registerModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter First Name!</center></div>");
        return;
    }
    if (lastname === "") {
        $("#registerModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter Last Name!</center></div>");
        return;
    }
    if (email === "") {
        $("#registerModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter Email!</center></div>");
        return;
    }
    if (password === "") {
        $("#registerModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter Password!</center></div>");
        return;
    }
    if (password.length < 8) {
        $("#registerModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>Password must be at least 8 characters long!</center></div>");
        return;
    }
    if (!validateEmail(email)) {
        $("#registerModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>Invalid email address!</center></div>");
        return;
    }

    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/Register",
            method: "POST",
            data: { firstname: firstname, lastname: lastname, email: email, password: password },
            dataType: "text",
            success: response => processResponse(response, "registerModalAlert", 700)
        });
}

function changeInfo(userID) {
    var firstname = $("#profileInfoModal #firstname").val();
    var lastname = $("#profileInfoModal #lastname").val();
    var email = $("#profileInfoModal #email").val();
    var password = $("#profileInfoModal #password").val();

    if (firstname === "") {
        $("#profileInfoModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter firstname!</center></div>");
        return;
    }
    if (lastname === "") {
        $("#profileInfoModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter lastname!</center></div>");
        return;
    }
    if (email === "") {
        $("#profileInfoModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter email!</center></div>");
        return;
    }
    if (password === "") {
        $("#profileInfoModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter password!</center></div>");
        return;
    }
    if (password.length < 8) {
        $("#profileInfoModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>Password must be at least 8 characters long!</center></div>");
        return;
    }
    if (!validateEmail(email)) {
        $("#profileInfoModalAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>Invalid email address!</center></div>");
        return;
    }

    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/ChangeInfo",
            method: "POST",
            data: { userID: userID, firstname: firstname, lastname: lastname, email: email, password: password },
            dataType: "text",
            success: response => processResponse(response, "profileInfoModalAlert")
        });
}

function logout() {
    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/Logout",
            method: "POST",
            data: {},
            dataType: "text",
            success: response => window.location.reload()
        });
}

function createAuction() {
    var
        productname = $("#productname").val(),
        startingprice = $("#startingprice").val(),
        auctionduration = $("#auctionduration").val(),
        attachment = $("#attach-files").prop("files");

    if (productname === "") {
        $("#createAuctionAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter product name!</center></div>");
        return;
    }
    if (startingprice === "") {
        $("#createAuctionAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter starting price!</center></div>");
        return;
    }
    if (attachment.length !== 1) {
        $("#createAuctionAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must choose one image!</center></div>");
        return;
    }

    var formData = new FormData();

    formData.append("productname", productname);
    formData.append("startingprice", startingprice);
    formData.append("auctionduration", auctionduration);
    formData.append("image", attachment[0], attachment[0].name);

    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/CreateAuction",
            method: "POST",
            data: formData,
            processData: false,
            contentType: false,
            dataType: "text",
            success: response => processResponse(response, "createAuctionAlert")
        });
}

function changeSystemParameters() {
    var auctioncount = $("#auctioncount").val();
    var auctionduration = $("#auctionduration").val();
    var silvercount = $("#silvercount").val();
    var goldcount = $("#goldcount").val();
    var platinumcount = $("#platinumcount").val();
    var currency = $("#currency").val();
    var tokenvalue = $("#tokenvalue").val();

    if (auctioncount === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter N!</center></div>");
        return;
    }
    if (auctionduration === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter D!</center></div>");
        return;
    }
    if (silvercount === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter S!</center></div>");
        return;
    }
    if (goldcount === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter G!</center></div>");
        return;
    }
    if (platinumcount === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter P!</center></div>");
        return;
    }
    if (currency === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter C!</center></div>");
        return;
    }
    if (tokenvalue === "") {
        $("#systemParametersAlert").html("<div class=\"alert alert-danger alert-dismissible\"><a href = \"\" class= \"close\" data-dismiss=\"alert\" aria - label=\"close\" >&times;</a><center>You must enter T!</center></div>");
        return;
    }

    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/ChangeSystemParameters",
            method: "POST",
            data: { auctioncount: auctioncount, auctionduration: auctionduration, silvercount: silvercount, goldcount: goldcount, platinumcount: platinumcount, currency: currency, tokenvalue: tokenvalue },
            dataType: "text",
            success: response => processResponse(response, "systemParametersAlert")
        });
}

function getcentilibutton(orderID, package) {
    $("#centililink").html("<a id=\"c-mobile-payment-widget\" href=\"https://stage.centili.com/payment/widget?apikey=9b424c6bf29b280430c0e233ea1119e3&reference=" + orderID + "&country=rs&package=" + package + "\"><img src=\"https://www.centili.com/images/centili-widget-button.png\"/></a>");
}

function orderTokens(userID) {
    var package = $("input:radio[name=package]:checked").val();
    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/OrderTokens",
            method: "POST",
            data: { package: package, userID: userID },
            dataType: "text"
        });
}

function auction(ID) {
    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/Auction",
            method: "GET",
            data: { ID: ID },
            dataType: "text",
            success: function (response) {
                window.location.href = "https://" + window.location.host + "/Home/Auction/" + ID;
            }
        });
}

function approveAuction(ID) {
    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/ApproveAuction",
            method: "POST",
            data: { ID: ID },
            dataType: "text"
        });
}

function createBid(userID, auctionID) {
    var newPrice = $("#newPrice").val();
    $("#bidButton").prop("disabled", true);
    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/CreateBid",
            method: "POST",
            data: { userID: userID, auctionID: auctionID, newPrice: newPrice },
            dataType: "text",
            success: function (response) {
                processResponse(response, "bidResponse");
                $("#bidButton").prop("disabled", false);
            }
        });
}

function finishAuction(userID, auctionID) {
    $("#finishAuctionButton").prop("disabled", true);
    $.ajax
        ({
            url: "https://" + window.location.host + "/Home/FinishAuction",
            method: "POST",
            data: { userID: userID, auctionID: auctionID },
            dataType: "text",
            success: function (response) {
                processResponse(response, "finishAuctionResponse");
                $("#finishAuctionButton").prop("disabled", false);
                if (!response.startsWith("#Error: ")) {
                    $("#finishAuctionButton").hide();
                }
            }
        });
}