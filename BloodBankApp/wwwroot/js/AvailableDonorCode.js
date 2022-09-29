var typingTimer;
var doneTypingInterval = 1000;
var $codeValue = $('#CodeValue');

//on keyup, start the countdown
$codeValue.on('keyup', function () {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(doneTypingUserName, doneTypingInterval);
});

//on keydown, clear the countdown
$codeValue.on('keydown', function () {
    clearTimeout(typingTimer);
});

function doneTypingUserName() {
    $.get("/api/Availability/DonorCodeIsTaken?codeValue=" + $codeValue.val(), function (data, status) {
        console.log("Trying " + data);
        if (status == 'success') {
            if (data) {
                $("#DonorCodeIsTaken").removeClass("d-none");
            }
            else {
                $("#DonorCodeIsTaken").addClass("d-none");
            }
        }
    });
}