<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Localization.ascx.cs" Inherits="Form_Creator_Localization.Controller.Localization" %>

<%--Styles--%>
<link rel="stylesheet" type="text/css" href="Styles/LocalizationStyles.css" />

<%--Scripts--%>
<script src="//code.jquery.com/jquery-1.10.2.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/jquery.dataTables.js"></script>
<script src="Scripts/jEditable.js"></script>
<script src="Scripts/Localization.js"></script>

<div class="wrapper">
        <div id="div-language-content">
            <label>Please Select a Langauge to get started:</label>
            <select id="ddlLanguage" title="Select a Language">
                <%--Dynamically Append this with langauges from the database, but for now we will statically set the languages--%>
                <option>--Select--</option>
                <option>English</option>
                <option>Germany</option>
                <option>Japanese</option>
            </select>
            <div id="errorContainer" style="display: none;"><label id="lblError" style="color: red;">Please select a Language</label></div>
        </div>
        <div id="div-language-fields">
            <table id="dtLanguageFields" style="display: none;">
                <thead>
                    <tr>
                        <td>Control ID</td>
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
        <input type="submit" value="Update" id="btnUpdate" runat="server" />
</div>