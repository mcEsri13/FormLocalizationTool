<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Form_Creator_Localization.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Form Creator Localization Tool</title>
    
    <%--Styles--%>
    <link rel="stylesheet" type="text/css" href="Styles/LocalizationStyles.css" />
    <link href="http://select-box.googlecode.com/svn/tags/0.2/jquery.selectbox.css" type="text/css" rel="stylesheet" />

    <%--Scripts--%>
    <script src="//code.jquery.com/jquery-1.10.2.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/jquery.dataTables.js"></script>
    <script type="text/javascript" src="http://select-box.googlecode.com/svn/tags/0.2/jquery.selectbox-0.2.min.js"></script>
    <script src="Scripts/jEditable.js"></script>
    <script src="Scripts/Localization.js"></script>
    
    
</head>
<body>
    <div class="wrapper">
        <div id="div-language-content">
            <label>Please Select a Langauge to get started:</label>
            <select id="ddlLanguage" runat="server" title="Select a Language">
                <%--Dynamically Append this with langauges from the database, but for now we will statically set the languages--%>
                <%--<option>--Select--</option>
                <option>English</option>
                <option>Germany</option>
                <option>Japanese</option>--%>
            </select>
            <div id="errorContainer" style="display: none;"><label id="lblError" style="color: red;">Please select a Language</label></div>
        </div>
        <div id="dataTables_wrapper">
            <table id="dtLanguageFields" class="dataTables_empty" style="display: none;">
                <thead>
                    <tr>
                        <td>ID</td>
                        <td>Control Name</td>
                        <td>Language Label</td>
                        <td>Language</td>
                    </tr>
                </thead>
                <tbody id="tbl-body">
                    <%--dynamically added here--%>
                </tbody>
            </table>  
        </div>
        <input type="submit" value="Update" id="btnUpdate" runat="server" style="display:none;" />
    </div>
</body>
</html>