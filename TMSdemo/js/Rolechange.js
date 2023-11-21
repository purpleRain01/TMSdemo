
$(document).ready(function () {

    $('.radio-option').change(function () {
        debugger;
        // Check which radio button is selected
        if ($('#radioPrimary1').prop('checked')) {
            // Employee radio button is selected
            $.ajax({
                type: "GET",
                url: "/Reports/Dropdownchange",
                data: {
                    filterstring: "emp" // Use ":" instead of "="
                },
                success: function (response) {
                    $("#drpselect").empty();

                    debugger;
                    $("#drpselect").append(
                        $("<option>", {
                            value: "",
                            text: "---Select---",
                            disabled: true,
                            selected: true
                        })
                    );

                    $.each(response, function (index, item) {
                        $("#drpselect").append(
                            $("<option>", {
                                value: item.empid,
                                text: item.empname
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
        } else if ($('#radioPrimary2').prop('checked')) {
            // Task radio button is selected
            $.ajax({
                type: "GET",
                url: "/Reports/Dropdownchange",
                data: {
                    filterstring: "tsk" // Use ":" instead of "="
                },
                success: function (response) {
                    $("#drpselect").empty();

                    debugger;
                    $("#drpselect").append(
                        $("<option>", {
                            value: "",
                            text: "---Select---",
                            disabled: true,
                            selected: true
                        })
                    );

                    $.each(response, function (index, item) {
                        $("#drpselect").append(
                            $("<option>", {
                                value: item.empid,
                                text: item.empid
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
    });
    

    $("#submitforrpt").click(function () {
        
        if ($("#drpselect").val() == "") {
            $("#alertlabel").text("Please select dropdowns properly");
            $("#alertdiv").css("display", "block");
            return false;
        }

        var rpttype = "";
        var selectedRole = "";
        var selecterange = $(".reservationtime").val();
        var datetimeRange = selecterange.split(" - ");
        var startDatetimeString = datetimeRange[0];
        var endDatetimeString = datetimeRange[1];

        // Parse the datetime strings into Date objects
        var startDatetime = new Date(startDatetimeString);
        var endDatetime = new Date(endDatetimeString);

        if ($('#radioPrimary1').prop('checked')) {
            rpttype = "emp";
            selectedRole = $("#drpselect").val();
        }
        else if ($('#radioPrimary2').prop('checked')) {
            rpttype = "tsk";
            selectedRole = $("#drpselect").val();
        }

        $.ajax({
            type: "POST",
            url: "/Reports/DownloadExcel",
            cache: false,
            data: {
                rptype: rpttype,
                role: selectedRole,
                startT: startDatetime,
                EndT: endDatetime,
            },
            xhrFields: {
                responseType: 'blob' // Set the response type to 'blob'
            },
            success: function (response) {
                debugger
                var blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var link = document.createElement('a');
                link.href = URL.createObjectURL(blob);
                link.download = 'report.xlsx';
                link.click();
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });
    });


    $(".btnrolechange").click(function () {
        var empId = $(this).data("empid");
        var selectedRole = $(this).closest("tr").find(`#roledrp_${empId}`).val();
        if (!confirm("Do yoy want to continue")) {
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/Rolemaster/Rolechange",
            data: {
                empid: empId,
                role: selectedRole
            },
            success: function (response) {
                
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });
    });
    $(".reservationtime").on("change", function () {
      
        var dateTimeRange = $(this).val();
        var dateTimeParts = dateTimeRange.split(" - ");

        var startDateString = dateTimeParts[0];
        var endDateString = dateTimeParts[1];

        var startDate = new Date(startDateString);
        var endDate = new Date(endDateString);

        var timeDifference = endDate - startDate;
        var timeDifferenceInSeconds = Math.floor(timeDifference / 1000);

        var seconds = timeDifferenceInSeconds % 60;
        timeDifferenceInSeconds = Math.floor(timeDifferenceInSeconds / 60);

        var minutes = timeDifferenceInSeconds % 60;
        timeDifferenceInSeconds = Math.floor(timeDifferenceInSeconds / 60);

        var hours = timeDifferenceInSeconds % 24;
        var days = Math.floor(timeDifferenceInSeconds / 24);

        var differenceText = "";

        if (days > 0) {
            differenceText += days + " days ";
        }
        if (hours > 0) {
            differenceText += hours + " hours ";
        }
        if (minutes > 0) {
            differenceText += minutes + " minutes ";
        }
        if (seconds > 0) {
            differenceText += seconds + " seconds ";
        }

        alert("Time range " + differenceText);
    });
    $(".btntaskassign").click(function () {
        debugger
        var taskcode1 = $(this).data("empid"); // Use "empid" attribute
        var selectedRole = $(this).closest("tr").find(`.empdrp[data-empid="${taskcode1}"]`).val(); // Use "data-empid" attribute
        if (selectedRole == "") {
            alert("please select employee");
            return false;
        }
        // Get the unique ID for the date and time picker
        var selectedDateTime = $(this).closest("tr").find(".reservationtime").val();

        if (!confirm("Do yoy want to continue")) {
            return false;
        }

        $.ajax({
            type: "POST",
            url: "/Taskassign/TassignToemployee",
            data: {
                empid: selectedRole,
                taskcode: taskcode1,
                dateT: selectedDateTime // Pass the selected date and time value
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });

    });

    $(".btnsendtoclient").click(function () {
        debugger
        var taskcode1 = $(this).data("empid"); // Use "empid" attribute
        var selectedRole = $(this).closest("tr").find(`.empdrp[data-empid="${taskcode1}"]`).val(); // Use "data-empid" attribute
        if (selectedRole == "") {
            alert("please select employee");
            return false;
        }
        // Get the unique ID for the date and time picker
        var selectedDateTime = $(this).closest("tr").find(".reservationtime").val();
        if (!confirm("Do yoy want to continue")) {
            return false;
        }

        $.ajax({
            type: "POST",
            url: "/Taskassign/TassignToemployee",
            data: {
                empid: selectedRole,
                taskcode: taskcode1,
                dateT: selectedDateTime // Pass the selected date and time value
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });

    });

    $(".btntasktest").click(function () {
        debugger

        var taskcode1 = $(this).data("empid"); // Use "empid" attribute
        var selectedRole = $(this).closest("tr").find(`.empdrp[data-empid="${taskcode1}"]`).val(); // Use "data-empid" attribute
        if (selectedRole == "") {
            alert("please select tester");
            return false;
        }
        // Get the unique ID for the date and time picker
        var selectedDateTime = $(this).closest("tr").find(".reservationtime").val();
        if (!confirm("Do yoy want to continue")) {
            return false;
        }

        $.ajax({
            type: "POST",
            url: "/Taskassign/TassignToTester",
            data: {
                empid: selectedRole,
                taskcode: taskcode1,
                dateT: selectedDateTime // Pass the selected date and time value
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error("Status:", xhr.status);
                console.error("Response Text:", xhr.responseText);
                console.error("Error:", error);
            }
        });

    });
    $(".asksubmit").click(function () {
        if (!confirm("Do yoy want to continue")) {
            return false;
        }

    });

});

    
    


