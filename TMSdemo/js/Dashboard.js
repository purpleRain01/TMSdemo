

function handleNavLinkClick(event, clickedLink) {
    const navLinks = document.querySelectorAll(".nav-link");
    navLinks.forEach(link => link.classList.remove("nav-link-active"));

    /*clickedLink.classList.add("nav-link","active");*/

    // Allow the default behavior to occur after class change (redirect to the URL)
    // Note: The link will be followed after this function completes execution.
}

//chart


// Add active class based on the current URL when the page loads
document.addEventListener("DOMContentLoaded", function () {
    const currentURL = window.location.href;
    const navLinks = document.querySelectorAll(".nav-link");
    var textarea1 = document.getElementById("inputSuccess");
    var charCountDisplay1 = document.getElementById("charCount1");
    var maxChars1 = 249;
    if (textarea1 != null) {
        textarea1.addEventListener("input", function () {
            debugger
            var currentChars1 = textarea1.value.length;
            var charsRemaining = maxChars1 - currentChars1;

            charCountDisplay1.textContent = charsRemaining + " characters remaining";
            if (currentChars1 > maxChars1) {
                textarea1.value = textarea1.value.substring(0, maxChars1);
                charCountDisplay1.textContent = "0 characters remaining";
            }
        });
    }
    
    navLinks.forEach(link => {
        if (currentURL.includes(link.href)) {
           
            link.classList.add("bg-black");
            /*link.classList.add("bg-indigo", "active");*/
        }
    });
});

$(document).ready(function () {
    debugger
    
    var textarea = document.getElementById("reasonforhold");
    var charCountDisplay = document.getElementById("charCount");
   
    
    var maxChars = 149;
    

    if (textarea != null) {
        textarea.addEventListener("input", function () {

            var currentChars = textarea.value.length;
            var charsRemaining = maxChars - currentChars;

            charCountDisplay.textContent = charsRemaining + " characters remaining";
            if (currentChars > maxChars) {
                textarea.value = textarea.value.substring(0, maxChars);
                charCountDisplay.textContent = "0 characters remaining";
            }
        });
    }
   
    $(".btnchangetime").click(function (e) {
        
        debugger
        if (!confirm("Are you sure to change alloted time")) {
            return false;
        }

        var selectedDateTime = $(this).closest("tr").find(".reservationtime").val();
        var taskcode1 = $(this).data("empid");

        $.ajax({
            type: "GET",
            url: "/Taskassign/TaskchangeTime",
            data: {
                taskcode: taskcode1,
                DateT: selectedDateTime,
            },
            success: function (response) {
                location.reload();
                alert(response);
              
                
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });
    });


    $(".btnholdreason").click(function (e) {
        e.preventDefault(); // Prevent the default link behavior

        var reason = $('#reasonforhold').val(); // Get the value of the input field
        if (reason === "") {
            alert("Please write a reason");
            return false;
        }
        if (!confirm("Are you sure")) {
            return false;
        }
        // Construct the URL with the correct reason value
        var url = $(this).attr("href") + "?reason=" + encodeURIComponent(reason);

        // Redirect to the HoldTask action with the reason parameter
        window.location.href = url;
    });


    $("#filterdrop").change(function () {
        onFilterChange();
    });

    $("#btngo").click(function () {
        debugger
        var dateInput = $("#datemask").val();
        var dateInput1 = $("#datemask1").val();
        if (!validateDate(dateInput) || !validateDate(dateInput1)) {
            return false;
        }
        Searchbydate();
    });

    function Searchbydate() {
        debugger
        var dt1 = $("#datemask").val();   
        var dt2 = $("#datemask1").val(); 

        $.ajax({
            type: "GET",
            url: "/Dashboard1/IndexFiltered",
            data: {
                id: "3",
                sD: dt1,
                eD: dt2
            },
            success: function (response) {
                
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });
    }


    function onFilterChange() {
        
        const selectedValue = $("#filterdrop").val();
        if (selectedValue != "3") {
            $("#divdate").css("display", "none");
            $.ajax({
                type: "GET",
                url: "/Dashboard1/IndexFiltered",
                data: {
                    id: selectedValue,
                    sD: "0",
                    eD: "0"
                },
                success: function (data) {
                    debugger
                    $('#content').html(data.newContent);
                    location.reload();
                },
                error: function (xhr, status, error) {
                    debugger
                    console.error("Status:", xhr.status);
                    console.error("Response Text:", xhr.responseText);
                    console.error("Error:", error);
                }
            });
        }
        else if (selectedValue == "3") {
            debugger
            
            $("#divdate").css("display", "block");
        }
    }

    $("#clientSelect").change(function () {
        onClientSelectionChange();
    });

    function onClientSelectionChange() {
        const selectedValue = $("#clientSelect").val();

        $.ajax({
            type: "GET",
            url: "/Dashboard1/GetDropdowndetails",
            data: {
                id: selectedValue,
                option: "1",
                prjid:""
            },
            success: function (response) {
                $("#projectSelect").empty();
                $("#moduleSelect").prop("disabled", true);
                $("#projectSelect").prop("disabled", false);

                $("#projectSelect").append(
                    $("<option>", {
                        value: "",
                        text: "---Select---",
                        disabled: true,
                        selected: true
                    })
                );

                $.each(response, function (index, item) {
                    $("#projectSelect").append(
                        $("<option>", {
                            value: item.prjcode,
                            text: item.prjname
                        })
                    );
                });
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });
    }

    $("#projectSelect").change(function () {
        onProjectSelectionChange();
    });

    function onProjectSelectionChange() {
        const selectedValue = $("#projectSelect").val();

        $.ajax({
            type: "GET",
            url: "/Dashboard1/GetDropdowndetails",
            data: {
                id: selectedValue,
                option: "2",
                prjid:""
            },

            success: function (response) {
                $("#formSelect").empty();
                $("#moduleSelect").prop("disabled", true);
                $("#moduleSelect").empty();
                $("#moduleSelect").prop("disabled", false);
                $("#moduleSelect").append(
                    $("<option>", {
                        value: "",
                        text: "---Select---",
                        disabled: true,
                        selected: true
                    })
                );

                $.each(response, function (index, item) {
                    $("#moduleSelect").append(
                        $("<option>", {
                            value: item.modcode,
                            text: item.modname
                        })
                    );
                });
            }
        });
    }

    $("#moduleSelect").change(function () {
        onModuleSelectionChange();
    });

    function onModuleSelectionChange() {
       
        const selectedValue = $("#moduleSelect").val();
        const selectedvalueprj = $("#projectSelect").val();
        $.ajax({
            type: "GET",
            url: "/Dashboard1/GetDropdowndetails",
            data: {
                id: selectedValue,
                option: "3",
                prjid: selectedvalueprj
            },
            success: function (response) {
                
                $("#formSelect").empty();
                $("#formSelect").prop("disabled", false);

                $("#formSelect").append(
                    $("<option>", {
                        value: "",
                        text: "---Select---",
                        disabled: true,
                        selected: true
                    })
                );
                
                var taskcode = response.AdditionalData;

                $("#inputWarning").removeAttr("disabled"); // Remove the disabled attribute
                $("#inputWarning").val(taskcode); // Set the value using .val()
                $("#inputWarning").prop("disabled", true);

                $.each(response.Rows, function (index, item) {
                    $("#formSelect").append(
                        $("<option>", {
                            value: item.formid,
                            text: item.formname
                        })
                    );
                });

            }
        });
    }

    $("#submit").click(function () {
        var flag = 0;
        if ($("#inputWarning").val() == "") {
            
            $("#alertlabel").text("Please select dropdowns properly"); 
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false; 
        }

        if ($("#formSelect").find(":selected").text() == "---Select---") {
            debugger
            $("#alertlabel").text("Please select form code");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#inputSuccess").val() == "") {
            $("#alertlabel").text("Please add some Task description");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }
        var dateInput = $("#datemask").val();
        if (!validateDate(dateInput)) {
            $("#alertlabel").text("Invalid date format. Please use dd/mm/yyyy format.");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }
        var radioSelected = $("input[name='radio1']:checked").length > 0;
        if (!radioSelected) {
            $("#alertlabel").text("Please select a priority level");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }
        //posting the form to server
        if (flag == 0) {
            if (!confirm("Do yoy want to Submit")) {
                return false;
            }
            var task = {
                clientID: $("#clientSelect").val(),
                projectCode: $("#projectSelect").val(),
                moduleCode: $("#moduleSelect").val(),
                FormID: $("#formSelect").val(),
                taskCode: $("#inputWarning").val(),
                Priority: $("input[name='radio1']:checked").val(),
                taskDetails: $("#inputSuccess").val(),
                endDate: $("#datemask").val()

            };

            $.ajax({
                type: "POST",
                url: "/Dashboard1/SubmitTask",
                data: task, // Pass the task object directly
                dataType: "json", // Specify the expected response type
                success: function (response) {
                    if (response !== null) {
                        alert(response);
                    } else {
                        alert("task submission failed please retry...");
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", error);
                    console.error("Response Status:", xhr.status);
                    console.error("Response Status Text:", xhr.statusText);
                }
            });


           

        }

    });
    function validateEmail(input) {
        var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        return regex.test(input);
    }

    
    // Date validation function
    function validateDate(input) {
        
        var regex = /^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/[0-9]{4}$/;
        return regex.test(input);
    }
    $("#submitclient").click(function () {
        debugger
        var flag = 0;
        if ($("#clientname").val() == "") {

            $("#alertlabel").text("Please give client name");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#clientabr").val() == "") {
            debugger
            $("#alertlabel").text("Please give abbreviation for client");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#contactprsn").val() == "") {
            $("#alertlabel").text("Please add contact person");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }
        if ($("#contactno").val() == "") {
            $("#alertlabel").text("Please give the contact for client");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }
        var email = $("#email").val();
        if (!validateEmail(email)) {
            $("#alertlabel").text("invalid email");
            $("#alertdiv").css("display", "block");
            flag = 1;
            return false;
        }
        //posting the form to server
        if (flag == 0) {
            if (!confirm("Do yoy want to add this client")) {
                return false;
            }
            var task = {
                clientName: $("#clientname").val(),
                CLAbbreviation: $("#clientabr").val(),
                contactNo: $("#contactno").val(),
                contactPerson: $("#contactprsn").val(),
                Email: $("#email").val()      
            };

            $.ajax({
                type: "POST",
                url: "/Client/InsertClient",
                data: task, // Pass the task object directly
                dataType: "json", // Specify the expected response type
                success: function (response) {
                    if (response !== null) {
                        alert(response);
                    } else {
                        alert("client adding failed please retry...");
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", error);
                    console.error("Response Status:", xhr.status);
                    console.error("Response Status Text:", xhr.statusText);
                }
            });




        }

    });

    $("#refreshButton").click(function () {
        var dataSource = $(this).data("source");

        $.ajax({
            url: "/Dashboard/GetTime",
            type: "GET",
            dataType: "html",
            success: function (response) {
                console.log("AJAX Success:", response);
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", error);
            }
        });
    });
});
