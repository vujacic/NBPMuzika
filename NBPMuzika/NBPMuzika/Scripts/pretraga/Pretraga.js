
function GetSve(page,search) {
    $.ajax(
                {
                    type: "GET", //GET or POST or PUT or DELETE verb
                    url: "api/pretraga", // Location of the service
                    data: '{"page":' + page + ",pretraga:"+search+ '}', //Data sent to server
                    contentType: "application/json; charset=utf-8", // content type sent to server
                    dataType: "json", //Expected data format from server
                    processdata: true, //True or False
                    success: function (msg) //On Successfull service call
                    {
                        Nacrtaj(msg);
                    },
                    error: function () // When Service call fails
                    {
                        alert("Error loading products" + result.status + " " + result.statusText);
                    }
                }
            );
}

function Nacrtaj(res) {
    console.log(res);
    res.forEach(function (element) {
        var ele = document.createElement("div");
        $(ele).attr("class", "row");
        $
    })
}

function jedan(poc,ele) {
    var kol = poc.createElement("div");
    kol
}

$('#btnSrc').click(function () {
    var search = $('#iptSrc').val();
    location.href = "/search/detail?page=1&pretraga=" + search;
})