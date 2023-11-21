$(document).ready(function () {
  
});

function validateForm1() {


    var empid = $('#empid').val();
    if (empid.trim() === '') {
        $('#idvalidation').text('Enter your id').show();
        return false
    }
    else {
        $('#idvalidation').text('').hide();
    }
   
    var password = $('#password').val();

    if (password.trim() === '') {
        $('#passwordvalidation').text('Enter your password').show();
        return false;
    }
    else {
        $('#passwordvalidation').text('').hide();
    }


    
  


    return true; // Validation passed
}

function displayValidationMessage(message) {
    $('#validation-message').text(message).show();
}
