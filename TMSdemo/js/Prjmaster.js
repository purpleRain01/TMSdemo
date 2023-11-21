$(document).ready(function () {

    //window.onunload  = function () {
    //     /*Clear specific cookies here if needed*/
    //    debugger
    //    document.cookie = "Name=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "Id=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "Admin=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "Admin1=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "Dsgn=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "starttime=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "totalwork=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "totalbreak=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //    document.cookie = "tester=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    //     /*Add more lines to clear additional cookies as necessary*/
    //};
    $("#filterdrop1").change(function () {
        onFilter1Change();
    });
    function onFilter1Change() {
        debugger
        const selectedValue = $("#filterdrop1").val();
        const filterdrop2 = $("#filterdrop2");

        if (selectedValue == "status") {
            // Clear existing options in filterdrop2
            filterdrop2.empty();
            filterdrop2.append($('<option>', {
                value: '',
                text: '--select--',
                disabled: true,
                selected: true
            }));
            // Add new options for "new," "cwb," and "reopen"
            filterdrop2.append($('<option>', {
                value: 'new',
                text: 'New'
            }));

            filterdrop2.append($('<option>', {
                value: 'cwb',
                text: 'CWB'
            }));

            filterdrop2.append($('<option>', {
                value: 'reopen',
                text: 'Reopen'
            }));
            filterdrop2.append($('<option>', {
                value: 'hold',
                text: 'Hold'
            }));
            filterdrop2.append($('<option>', {
                value: 'pending',
                text: 'Pending'
            }));
            filterdrop2.append($('<option>', {
                value: 'closed',
                text: 'Closed'
            }));
            filterdrop2.append($('<option>', {
                value: 'finished',
                text: 'Finished'
            }));
            filterdrop2.append($('<option>', {
                value: 'running',
                text: 'Running'
            }));
            filterdrop2.append($('<option>', {
                value: 'completed',
                text: 'Completed'
            }));
            filterdrop2.append($('<option>', {
                value: 'break',
                text: 'Break'
            }));
            filterdrop2.append($('<option>', {
                value: 'eod',
                text: 'EOD'
            }));
        }
        else if (selectedValue == "client") {
            $.ajax({
                type: "GET",
                url: "/Dashboard1/GetDropdowndetails1",
                data: {
                    id: selectedValue,
                    option: "0",
                    prjid: ""
                },
                success: function (response) {
                    debugger
                    $("#filterdrop2").empty();
                    

                    $("#filterdrop2").append(
                        $("<option>", {
                            value: "",
                            text: "---Select---",
                            disabled: true,
                            selected: true
                        })
                    );

                    $.each(response, function (index, item) {
                        $("#filterdrop2").append(
                            $("<option>", {
                                value: item.clcode,
                                text: item.clname
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
        else if (selectedValue == "project") {
            $.ajax({
                type: "GET",
                url: "/Dashboard1/GetDropdowndetails1",
                data: {
                    id: selectedValue,
                    option: "1",
                    prjid: ""
                },
                success: function (response) {
                    debugger
                    $("#filterdrop2").empty();


                    $("#filterdrop2").append(
                        $("<option>", {
                            value: "",
                            text: "---Select---",
                            disabled: true,
                            selected: true
                        })
                    );

                    $.each(response, function (index, item) {
                        $("#filterdrop2").append(
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
        else if (selectedValue == "module") {
            $.ajax({
                type: "GET",
                url: "/Dashboard1/GetDropdowndetails1",
                data: {
                    id: selectedValue,
                    option: "2",
                    prjid: ""
                },
                success: function (response) {
                    debugger
                    $("#filterdrop2").empty();


                    $("#filterdrop2").append(
                        $("<option>", {
                            value: "",
                            text: "---Select---",
                            disabled: true,
                            selected: true
                        })
                    );

                    $.each(response, function (index, item) {
                        $("#filterdrop2").append(
                            $("<option>", {
                                value: item.modcode,
                                text: item.modname
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
        
        else if (selectedValue == "employee") {
            $.ajax({
                type: "GET",
                url: "/Dashboard1/GetDropdowndetails1",
                data: {
                    id: selectedValue,
                    option: "3",
                    prjid: ""
                },
                success: function (response) {
                    debugger
                    $("#filterdrop2").empty();


                    $("#filterdrop2").append(
                        $("<option>", {
                            value: "",
                            text: "---Select---",
                            disabled: true,
                            selected: true
                        })
                    );

                    $.each(response, function (index, item) {
                        $("#filterdrop2").append(
                            $("<option>", {
                                value: item.empname,
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
        }
        else {
            // If filterdrop1 value is not "status," clear filterdrop2
            filterdrop2.empty();
            filterdrop2.append($('<option>', {
                value: 'none',
                text: 'Select Filter 2'
            }));
        }
        
    }
    // Add an event handler for filterdrop2 change event
    $("#filterdrop2").change(function () {
        debugger
        filterTable();
    });

    function filterTable() {
        const selectedFilter1Value = $("#filterdrop1").val(); // Get the value from filterdrop1
        const selectedFilter2Value = $("#filterdrop2").val(); // Get the value from filterdrop2

        // Show all table rows initially
        $("table tbody tr").show();

        if (selectedFilter1Value === 'status') {
            // Filter based on selected status value (selectedFilter2Value)
            $("table tbody tr").not(":has(td span:contains(" + selectedFilter2Value + "))").hide();
        } else if (selectedFilter1Value === 'client') {
            // Filter based on selected client value (selectedFilter2Value)
            $("table tbody tr").not(":has(td:contains(" + selectedFilter2Value + "))").hide();
        } else if (selectedFilter1Value === 'project') {
            // Filter based on selected project value (selectedFilter2Value)
            $("table tbody tr").not(":has(td:contains(" + selectedFilter2Value + "))").hide();
        } else if (selectedFilter1Value === 'module') {
            // Filter based on selected module value (selectedFilter2Value)
            $("table tbody tr").not(":has(td:contains(" + selectedFilter2Value + "))").hide();
        } else if (selectedFilter1Value == 'employee') {
            // Filter based on selected module value (selectedFilter2Value)
            $("table tbody tr").not(":has(td span:contains(" + selectedFilter2Value + "))").hide();
        }

    }


    // Call filterTable initially to show all rows when the page loads
    /*filterTable();*/



    $("#submitprj").click(function () {
        var flag = 0;
        if ($("#prjname").val() == "") {

            $("#alertlabelprj").text("Please give project name");
            $("#alertdivprj").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#prjabr").val() == "") {
            debugger
            $("#alertlabelprj").text("Please give abbreviation for Project");
            $("#alertdivprj").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#clientSelect").val() == "" || $("#clientSelect").val()==null) {
            $("#alertlabelprj").text("Please select client first");
            $("#alertdivprj").css("display", "block");
            flag = 1;
            return false;
        }
        
        //posting the form to server
        if (flag == 0) {
            if (!confirm("Do yoy want to add this Project")) {
                return false;
            }
            var task = {
                projecttName: $("#prjname").val(),
                prjAbbreviation: $("#prjabr").val(),
                clientid: $("#clientSelect").val(), 
            };

            $.ajax({
                type: "POST",
                url: "/Client/InsertProject",
                data: task, // Pass the task object directly
                dataType: "json", // Specify the expected response type
                success: function (response) {
                    if (response !== null) {
                        alert(response);
                    } else {
                        alert("Project adding failed please retry...");
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


    $("#submitmod").click(function () {
        var flag = 0;
        debugger
        if ($("#modname").val() == "") {

            $("#alertlabelmod").text("Please give module name");
            $("#alertdivmod").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#modabr").val() == "") {
            debugger
            $("#alertlabelmod").text("Please give abbreviation for module");
            $("#alertdivmod").css("display", "block");
            flag = 1;
            return false;
        }
        
        if ($("#prjselect").val() == "" || $("#prjselect").val() ==null) {
            $("#alertlabelmod").text("Please select Project first");
            $("#alertdivmod").css("display", "block");
            flag = 1;
            return false;
        }

        //posting the form to server
        if (flag == 0) {
            debugger
            var task = {
                moduleName: $("#modname").val(),
                modAbbreviation: $("#modabr").val(),
                projectid: $("#prjselect").val(),
            };

            $.ajax({
                type: "POST",
                url: "/Client/InsertModule",
                data: task, // Pass the task object directly
                dataType: "json", // Specify the expected response type
                success: function (response) {
                    if (response !== null) {
                        alert(response);
                    } else {
                        alert("Module adding failed please retry...");
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

    $("#submitform").click(function () {
        var flag = 0;
        if ($("#clientSelect").val() == "" || $("#clientSelect").val()==null) {

            $("#alertlabelfrm").text("Please select a client first");
            $("#alertdivfrm").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#projectSelect").val() == "" || $("#projectSelect").val()==null) {
            debugger
            $("#alertlabelfrm").text("Please select a Project");
            $("#alertdivfrm").css("display", "block");
            flag = 1;
            return false;
        }

        if ($("#moduleSelect").val() == "" || $("#moduleSelect").val()==null) {
            $("#alertlabelfrm").text("Please select module first");
            $("#alertdivfrm").css("display", "block");
            flag = 1;
            return false;
        }
        if ($("#formname1").val() == "") {
            $("#alertlabelfrm").text("Please give the module name");
            $("#alertdivfrm").css("display", "block");
            flag = 1;
            return false;
        }
        //posting the form to server
        if (flag == 0) {
            if (!confirm("Do yoy want to add this module")) {
                return false;
            }
            var task = {
                clientSelect: $("#clientSelect").val(),
                projectid: $("#projectSelect").val(),
                moduleid: $("#moduleSelect").val(),
                formname: $("#formname1").val(),
            };

            $.ajax({
                type: "POST",
                url: "/Client/InsertForm",
                data: task, // Pass the task object directly
                dataType: "json", // Specify the expected response type
                success: function (response) {
                    if (response !== null) {
                        alert(response);
                    } else {
                        alert("Form adding failed please retry...");
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

});

