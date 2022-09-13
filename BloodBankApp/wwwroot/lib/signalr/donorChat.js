"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var chatWithUserId = "";
var chatWithUserFullName = "";

connection.start().then(function () {

    connection.invoke("GetConnectionId").then(function (data) {
        console.log(data);
    });;
    connection.invoke("SendMessage");

}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("Client", function (data) {
    console.log(data);
});

connection.on("loadChatConversation", function (data, recipientId, recipientFullName) {
    $("#talkingToFullName").text(recipientFullName);
    $("#chatBox").empty();

    chatWithUserId = recipientId;
    chatWithUserFullName = recipientFullName;

    $.each(data, function (index, value) {

        if (value.senderId === chatWithUserId) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-left pb-4"><div><div class="text-muted small text-nowrap">'+value.hour +":"+value.minute+'</div></div><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">' + chatWithUserFullName+':</div>' + value.content + '</div></div>')
                );
        } else {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-right pb-4"><div><div class="text-muted small text-nowrap mt-2">' + value.hour + ":" + value.minute +'</div></div><div class="flex-shrink-1 bg-info rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">You:</div>' + value.content + '</div></div>')
                );
        }
    });
});

function onSelectHospitalAdmin(hospitalAdminId) {
    connection.invoke("GetChatConversation", hospitalAdminId).catch(function (err) {
        return console.error(err.toString());
    });
}
function sendMessage() {
 
    connection.invoke("SendMessages", $("#message").val(), chatWithUserId).catch(function (err) {
        return console.error(err.toString());
    });

    $("#message").val("");
}

connection.on("recieveMessage", function (data, num) {
    console.log("Number " + num);
    if (data.senderId === chatWithUserId || data.receiverId === chatWithUserId) {
        if (num === 1) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-left pb-4"><div><div class="text-muted small text-nowrap">' + data.hour + ":" + data.minute + '</div></div><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">' + chatWithUserFullName + ':</div>' + data.content + '</div></div>')
                );
        } else if (num === 0) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-right pb-4"><div><div class="text-muted small text-nowrap mt-2">' + data.hour + ":" + data.minute + '</div></div><div class="flex-shrink-1 bg-info rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">You:</div>' + data.content + '</div></div>')
                );
        }
        $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);
    }
});