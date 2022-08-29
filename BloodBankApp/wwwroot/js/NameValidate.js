
jQuery.validator.addMethod("name",
    function (value, element, param) {
        var hasNumber = /\d/;
        if (hasNumber.test(value)) {
            return false;
        }
        else {
            return true;
        }
    });

jQuery.validator.unobtrusive.adapters.addBool("name");

