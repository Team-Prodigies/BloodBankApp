var typingTimer;
var doneTypingInterval = 1000;
var $hospitalCode = $('#HospitalCode');

        //on keyup, start the countdown
        $hospitalCode.on('keyup', function() {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(doneTypingUserName, doneTypingInterval);
});

        //on keydown, clear the countdown
        $hospitalCode.on('keydown', function() {
    clearTimeout(typingTimer);
});

function doneTypingUserName()
{
            $.get("/api/Availability/HospitalCodeIsTaken?hospitalCode=" + $hospitalCode.val(), function(data, status) {
        console.log("Trying " + data);
        if (status == 'success')
        {
            if (data)
            {
                            $("#HospitalCodeIsTaken").removeClass("d-none");
            }
            else
            {
                            $("#HospitalCodeIsTaken").addClass("d-none");
            }
        }
    });
            }