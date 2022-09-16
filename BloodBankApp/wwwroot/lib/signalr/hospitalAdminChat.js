"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
/*var chatWithDonorId = "";
var chatWithHospitalId = "";*/

connection.start().then(function () {

    connection.invoke("GetWaitingDonors", chatWithHospitalId).catch(function (err) {
        return console.error(err.toString());
    });;

}).catch(function (err) {
    return console.error(err.toString());
});

function onLoadWaitinDonors(hospitalAdminId) {

    chatWithHospitalId = hospitalAdminId;

    connection.invoke("GetWaitingDonors", hospitalAdminId).catch(function (err) {
        return console.error(err.toString());
    });;

}

connection.on("loadChatConversation", function (data) {

    
        $("#chatBox").empty();

        $.each(data, function (index, value) {

                if (value.sender === 0) {
                    $("#chatBox")
                        .append(
                            $('<div class="chat-message-left pb-4"><div><div class="text-muted small text-nowrap">' + value.hour + ":" + value.minute + '</div></div><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">Donor:</div>' + value.content + '</div></div>')
                        );
                } else if (value.sender === 1) {
                    $("#chatBox")
                        .append(
                            $('<div class="chat-message-right pb-4"><div><div class="text-muted small text-nowrap mt-2">' + value.hour + ":" + value.minute + '</div></div><div class="flex-shrink-1 bg-info rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">You:</div>' + value.content + '</div></div>')
                        );
                }       
        });
    
});

connection.on("loadWaitingDonors", function (data) {

    $.each(data, function (index, value) {

        var divSenderId = "#donor" + value.donorId+"a";

        if (!$(divSenderId).length) {
            divSenderId = "donor" + value.donorId+"a";
            $("#waitingDonors")
                .append(
                    $('<div class="list-group-item list-group-item-action border-0" id=' + divSenderId + '><div class="d-flex align-items-start"><img src="https://ui-avatars.com/api/?name=' + value.name + '+' + value.surname + '& background=d1001f&color=fff" alt="Profile" class="p-2" width="60" height="60"><div class="flex-grow-1 ml-3 p-2"><button class="btn btn-outline-secondary p-2" onclick="onSelectDonor(' + "'" + value.donorId + "'" + ')">' + value.name + ' ' + value.surname + '</button></div></div><hr /></div>')
                );
        }
              
    });
});

function onSelectDonor(donorId) {

    chatWithDonorId = donorId;
    connection.invoke("GetChatConversation", chatWithDonorId, chatWithHospitalId).catch(function (err) {
        return console.error(err.toString());
    });
}

function sendMessage() {

    connection.invoke("SendMessages", $("#message").val(), chatWithDonorId, chatWithHospitalId, 1).catch(function (err) {
        return console.error(err.toString());
    });

    $("#message").val("");
}

$('#message').bind('input', function () {
    console.log($(this).val());
    if ($(this).val().length > 0) {
        connection.invoke("Typing", chatWithDonorId).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        connection.invoke("NotTyping", chatWithDonorId).catch(function (err) {
            return console.error(err.toString());
        });
    }
});

connection.on("typing", function () {
    $("#typing").css("display", "block");
});

connection.on("notTyping", function () {
    $("#typing").css("display", "none");
});

connection.on("recieveMessage", function (data) {

    if (data.donorId === chatWithDonorId) {
        if (data.sender === 0) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-left pb-4"><div><div class="text-muted small text-nowrap">' + data.hour + ":" + data.minute + '</div></div><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">Donor:</div>' + data.content + '</div></div>')
                );
        } else if (data.sender === 1) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-right pb-4"><div><div class="text-muted small text-nowrap mt-2">' + data.hour + ":" + data.minute + '</div></div><div class="flex-shrink-1 bg-info rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">You:</div>' + data.content + '</div></div>')
                );
        }
        $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);
    }
    
});