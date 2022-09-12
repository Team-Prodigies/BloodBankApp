"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    console.log("Connected");

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
