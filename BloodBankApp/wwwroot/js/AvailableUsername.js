var typingTimer;
var doneTypingInterval = 1000;
var $userName = $('#UserName');

//on keyup, start the countdown
$userName.on('keyup', function () {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(doneTypingUserName, doneTypingInterval);
});

//on keydown, clear the countdown
$userName.on('keydown', function () {
    clearTimeout(typingTimer);
});

function doneTypingUserName() {
    $.get("/api/Availability/UsernameIsTaken?username=" + $userName.val(), function (data, status) {
        console.log("Trying " + data);
        if (status == 'success') {
            if (data) {
                $("#UserNameIsTaken").removeClass("d-none");
            } else {
                $("#UserNameIsTaken").addClass("d-none");
            }

        }
    });
}