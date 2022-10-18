"use strict";

var notificationsConnection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

notificationsConnection.start().then(function () {

    notificationsConnection.invoke('GetUnSeenMessagesFromHospital').catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});

notificationsConnection.on("showMessagesNotificationsFromDonors", function (data) {

    $("#messagesNotificationsNumber").text(data.length);
    console.log(data);
    data.forEach(element => {
        
        $("#messagesNotificationsBell")
            .append(
                $('<li class="message-item"><div><h4>' + element.name +" "+ element.surname + '</h4><p>Sended a message</p></div></li><li><hr class="dropdown-divider"></li>')
            );
    });
    
});
