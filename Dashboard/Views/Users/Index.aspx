<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-lg-8 col-lg-offset-2">
            <div class="panel panel-default">
                <div class="panel-heading">Users <%: Html.ActionLink("Add New", "New", "Users",null, new { @class = "btn btn-primary btn-xs pull-right" }) %></div>
                <div class="panel-body">
                    <% if (ViewBag.Updated == true)
                       { %>
                    <div class="alert alert-dismissible alert-success">
                        <button data-dismiss="alert" class="close" type="button"><i class="fa fa-remove"></i></button>
                        Users have been updated.               
                    </div>
                    <% } %>
                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th>Username</th>                                
                                <th>Is Enabled</th>                                
                                <th>&nbsp;</th>
                                <th>&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var User in Model.Users)
                               { %>
                            <tr>
                                <td><%: User.Username %></td>                                
                                <td><%: User.IsEnabled %></td>                                
                                <td><%: Html.ActionLink("Edit", "Edit", "Users", new { id = User.ID}, null)%></td>
                                <td><%: Html.ActionLink("Delete", "Delete", "Users", new { id = User.ID}, null)%></td>
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
