"use strict";

var notificationsConnection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

notificationsConnection.start().then(function () {

    notificationsConnection.invoke('GetUnSeenMessagesFromDonor').catch(function (err) {
        return console.error(err.toString());
    });

    notificationsConnection.invoke('GetPostsNotifications').catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});

notificationsConnection.on("showMessagesNotifications", function (data) {

    $("#messagesNotificationsNumber").text(data.length);
    data.forEach(element => {
        
        $("#messagesNotificationsBell")
            .append(
                $('<li class="message-item"><div><h4>' + element.hospitalName + '</h4><p>Has responded to your message</p></div></li><li><hr class="dropdown-divider"></li>')
            );
    });
    
});

notificationsConnection.on("showPostsNotifications", function (data) {

    $("#postsNotificationsNumber").text(data.length);

    data.forEach(element => {
        console.log(element);

        $("#postsNotificationsBell")
            .append(
                $('<li class="notification-item"><div><p>' + element.description + '</p></div></li><li><hr class="dropdown-divider"></li>')
            );
    });
});