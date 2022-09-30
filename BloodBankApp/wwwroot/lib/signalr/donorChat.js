"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {

    connection.invoke("GetChatConversation", chatWithDonorId, chatWithHospitalId).catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});

$('#message').bind('input', function () {
    if ($(this).val().length > 0) {
        connection.invoke("Typing", chatWithDonorId, 0).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        connection.invoke("NotTyping", chatWithDonorId, 0).catch(function (err) {
            return console.error(err.toString());
        });
    }
});

connection.on("typing", function (num) {
    if (num == 1) {
        $("#typing").css("display", "block");
    }   
});

connection.on("notTyping", function (num) {
    if (num == 1) {
        $("#typing").css("display", "none");
    }   
});

connection.on("loadChatConversation", function (data, donorId, hospitalId) {
    $("#chatBox").empty();

    $.each(data, function (index, value) {

            if (value.sender === 1) {

                $("#chatBox")
                    .append(
                        $('<div class="chat-message-left pb-4"><div><div class="text-muted small text-nowrap">' + value.hour + ":" + value.minute + '</div></div><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">Hospital:</div>' + value.content + '</div></div>')
                    );
            } else if (value.sender === 0){
                $("#chatBox")
                    .append(
                        $('<div class="chat-message-right pb-4"><div><div class="text-muted small text-nowrap mt-2">' + value.hour + ":" + value.minute + '</div></div><div class="flex-shrink-1 bg-info rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">You:</div>' + value.content + '</div></div>')
                    );
            } 
    });

    $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);

    connection.invoke("SetHospitalMessagesToSeen", chatWithDonorId, chatWithHospitalId).catch(function (err) {
        return console.error(err.toString());
    });

});

connection.on("deleteChat", function () {
    $("#chatBox").children().remove();
});

function sendMessage() {
    connection.invoke("SendMessages", $("#message").val(), chatWithDonorId, chatWithHospitalId, 0).catch(function (err) {
        return console.error(err.toString());
    });

    $("#message").val("");
    connection.invoke("NotTyping", chatWithDonorId, 0).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("receiveMessage", function (data) {
  
    if (data.hospitalId === chatWithHospitalId) {
        if (data.sender === 1) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-left pb-4"><div><div class="text-muted small text-nowrap">' + data.hour + ":" + data.minute + '</div></div><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">Hospital:</div>' + data.content + '</div></div>')
            );

            connection.invoke("SetMessageToSeen", data.messageId).catch(function (err) {
                return console.error(err.toString());
            });

        } else if (data.sender === 0) {
            $("#chatBox")
                .append(
                    $('<div class="chat-message-right pb-4"><div><div class="text-muted small text-nowrap mt-2">' + data.hour + ":" + data.minute + '</div></div><div class="flex-shrink-1 bg-info rounded py-2 px-3 mr-3"><div class="font-weight-bold mb-1">You:</div>' + data.content + '</div></div>')
            );
        }
        $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);
    }
});