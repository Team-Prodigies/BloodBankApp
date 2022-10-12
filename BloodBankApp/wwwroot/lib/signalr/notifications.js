"use strict";

var notificationsConnection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

 notificationsConnection.start().then(function () {

     notificationsConnection.invoke('GetUnSeenMessagesFromDonor').catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});

notificationsConnection.on("showMessagesNotifications", function (data) {
    
    data.forEach(element => {
        console.log(element.hospitalId);
    });
});