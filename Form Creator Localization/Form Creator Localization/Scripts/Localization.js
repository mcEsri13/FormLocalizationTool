$(document).ready(function () {
    //Field and Values Container
    languageFieldAndValueObject = "{";

    //Select language
    $("#ddlLanguage").change(function () {
        if (IsValid()) {
            $("#errorContainer").hide();
            $("#btnUpdate").show();
            ResetLanguageFieldJSON(languageFieldAndValueObject);
            RemoveResultsFromDataTable();
            GetLanguages($("#ddlLanguage").val(), languageFieldAndValueObject);
        }
        else {
            $("#errorContainer").show();
            $("#btnUpdate").hide();
        }
    });

    //Submit update
    $("#btnUpdate").click(function () {

        //Cleans up just in case of reupdate
        languageFieldAndValueObject = "";

        //close up json object
        languageFieldAndValueObject = GetLanguagesFieldsUpdate(languageFieldAndValueObject);
        languageFieldAndValueObject = languageFieldAndValueObject.substring(0, languageFieldAndValueObject.length - 1);
        languageFieldAndValueObject += "}";

        //alert(languageFieldAndValueObject);

        //Update Fields by Language
        $.ajax({
            type: "POST",
            url: "/Helper/LocalizationHandler.ashx",
            data: { 'results': languageFieldAndValueObject },
            dataType: 'text',
            success: function (data) {
                alert(data);
            },
            error: function () {
                alert("Error Updated!");
            },
        });
    });
});

//Reads fields update
function GetLanguagesFieldsUpdate(languageFieldAndValueObject) {

    //Set the starter for JSON Object
    languageFieldAndValueObject = "{"
    $("#dtLanguageFields tbody tr").each(function (index) {
        var fieldID = $(this).find("td.language-id").html();
        var fieldLabel = $(this).find("td.language-cell").html();
        
        languageFieldAndValueObject += '"' + fieldID + '" : "' + fieldLabel + '",';
    });

    return languageFieldAndValueObject;
}

//Reset's the JSON Object for the Field Names and Field Values
function ResetLanguageFieldJSON(languageFieldAndValueObject) {
    languageFieldAndValueObject = "";
    languageFieldAndValueObject = "{"
}

//Removes existing data in table
function RemoveResultsFromDataTable() {
    $("#dtLanguageFields tbody tr").each(function () {
        this.remove();
    });
};

//Validates Against Function
function IsValid() {
    if ($("#ddlLanguage").val() == "undefined" || $("#ddlLanguage").val() == "--Select--" || $("#ddlLanguage").val() == "")
        return false;
    else {
        return true;
    }
};

function CheckFieldsForUpdate(fields) {
    if (fields.length <= 1)
        return false;
    else
        return true;
}

//Ajax call to get fields by language:
function GetLanguages(language, languageFieldAndValueObject) {
    //alert("this is the current value" + language);
    $.ajax({
        url: "/Helper/LocalizationHandler.ashx?language=" + language + "",
        dataType: 'json',
        success: function (data) {
            PopulateGrid(data, languageFieldAndValueObject);
        },
    });
};

//Fill Grid with Data
function PopulateGrid(data, languageFieldAndValueObject) {
    var tr;
    for (var i = 0; i < data.length; i++) {
        tr = $('<tr/>');
        tr.append("<td class='language-id'>" + data[i].ControlListLanguage_ID + "</td>");
        tr.append("<td>" + data[i].ControlName      + "</td>");
        tr.append("<td class='language-cell'>" + data[i].LabelText    + "</td>");
        tr.append("<td>" + data[i].LanguageName + "</td>");
        $("#tbl-body").append(tr);
    }

    //Bind Data..
    var tableGrid = $("#dtLanguageFields").dataTable({
        "oLanguage": { "sSearch": "Search Fields:" },
        "bRetrieve": true,
        "bPaginate": false,
        "aaSorting": [[0, "asc"]]
    });

    //Apply jEditable Plugin to Table
    $('.language-cell ', tableGrid.fnGetNodes()).editable(function (value, settings) {
        var fieldName = $(this).closest('td').prev('td').text();
        var result = StoreLanguageValues(value, fieldName, languageFieldAndValueObject);
        return (value);

    }).click(function (evt) {
        $(this).find('input').keydown(function (event) {
            if (event.which == 9) //'TAB'
                $(this).closest('form').submit();
        });
    });

    //Show table
    $("#dtLanguageFields").show();
};

//Store Field Values
function StoreLanguageValues(currentValue, fieldName, LanguageFieldAndValueObject) {
    languageFieldAndValueObject += '"' + fieldName + '" : "' + currentValue + '",';
};
