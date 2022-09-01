
var typingTimer2;
var doneTypingInterval2 = 1000;
var $personalNumber = $('#PersonalNumber');

//on keyup, start the countdown
$personalNumber.on('keyup', function () {
    clearTimeout(typingTimer2);
    typingTimer = setTimeout(doneTypingPersonalNumber, doneTypingInterval2);

});

//on keydown, clear the countdown
$personalNumber.on('keydown', function () {
    clearTimeout(typingTimer);
});

function doneTypingPersonalNumber() {
    console.log("Inside method");

    $.get("/api/Availability/PersonalNumberIsTaken?personalNumber=" + $personalNumber.val(), function (data, status) {
        if (status == 'success') {
            if (data) {
                $("#PersonalNumberIsTaken").removeClass("d-none");
            } else {
                $("#PersonalNumberIsTaken").addClass("d-none");
            }

        }
    });
}
