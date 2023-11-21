$(document).ready(function () {
    
    $('#signup-button').click(function (e) {
        

        // Perform your validation logic here
        if (!validateForm()) {
            //displayValidationMessage('Validation failed. Please check your inputs.');
            e.preventDefault(); // Prevent form submission
            return; // Do not submit form if validation fails
        }

        // If validation passes, submit the form
        $('#your-form-id').submit();
    });
    $('#login-button').click(function (e) {


        // Perform your validation logic here
        if (!validateForm1()) {
            //displayValidationMessage('Validation failed. Please check your inputs.');
            e.preventDefault(); // Prevent form submission
            return; // Do not submit form if validation fails
        }

        // If validation passes, submit the form
        $('#your-form-id').submit();
    });
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

function validateForm() {
  

    var empName = $('#empName').val(); 
    if (empName.trim() === '') {
        $('#namevalidation').text('Required field').show();
        return false
    }
    else {
        $('#namevalidation').text('').show();
    }
    var designation = $('#designation option:selected').text(); 
    if (designation === '-- Select designation --') {
        $('#designationvalidation').text('Required field').hide();
        return false; 
    }
    else {
        $('#designationvalidation').text('').hide();
    }
    var address = $('#address').val();
   
    if (address.trim() === '') {
        $('#addressvalidation').text('Required field').show();
        return false;
    }
    else {
        $('#addressvalidation').text('').hide();
    }

   
    var contact = $('#contact').val();
    if (contact.trim() === '') {
        $('#contactvalidation').text('Required field').show();
        return false;
    }
    else {
        $('#contactvalidation').text('').hide();
    }
    // Regex pattern for contact validation (example: 10 digits)
    var contactPattern = /^\d{10}$/;
    if (!contactPattern.test(contact)) {
        $('#contactvalidation').text('Invalid contact format').show();
        return false;
    } else {
        $('#contactvalidation').text('').hide();
    }

    var password = $('#password').val();
    var passwordc = $('#passwordc').val();
    if (password != passwordc) {
        $('#passwordvalidation').text('Passwords doesnt match').show();
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
