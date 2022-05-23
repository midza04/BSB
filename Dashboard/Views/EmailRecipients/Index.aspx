<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.EmailRecipientViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Email Recipients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-10 col-lg-offset-1">
            <div class="panel panel-default">                
                <div class="panel-heading">Email Recipients <%: Html.ActionLink("Add New", "New", "EmailRecipients",null, new { @class = "btn btn-primary btn-xs pull-right" }) %></div>
                <div class="panel-body">
                    <% if (ViewBag.Updated == true)
                       { %>
                    <div class="alert alert-dismissible alert-success">
                        <button data-dismiss="alert" class="close" type="button"><i class="fa fa-remove"></i></button>
                        Email Recipients have been updated.               
                    </div>
                    <% } %>
                    <table class="table table-bordered table-striped" id="emailRecipientsTable">
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Email Address</th>
                                <th style="width: 200px;">Is Primary Recipient</th>
                                <th>&nbsp;</th>                               
                                <th>&nbsp;</th>                               
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var email in Model.Emails)
                               { %>
                            <tr>
                                <td><%: email.Username %></td>
                                <td><%: email.EmailAddress %></td>
                                <td><%: email.PrimaryRecipient %></td>
                                <td><%: Html.ActionLink("Edit", "Edit", "EmailRecipients", new { id = email.ID}, null)%></td>
                                <td><%: Html.ActionLink("Delete", "Delete", "EmailRecipients", new { id = email.ID}, null)%></td>                                 
                            </tr>
                            <% } %>
                        </tbody>

                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
