var typingTimer;
var doneTypingInterval = 1000;
var $phoneNumber = $('#PhoneNumber');

//on keyup, start the countdown
$phoneNumber.on('keyup', function () {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(doneTypingUserName, doneTypingInterval);
});

//on keydown, clear the countdown
$phoneNumber.on('keydown', function () {
    clearTimeout(typingTimer);
});

function doneTypingUserName() {
    $.get("/api/Availability/PhoneNumberIsTakenApi?phoneNumber=" + $phoneNumber.val(), function (data, status) {
        console.log("Trying " + data);
        if (status == 'success') {
            if (data) {
                $("#PhoneNumberIsTaken").removeClass("d-none");
            }
            else {
                $("#PhoneNumberIsTaken").addClass("d-none");
            }
            console.log($phoneNumber.val());
        }
    });
}