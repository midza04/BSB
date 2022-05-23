<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.UserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="center-block">
            <div class="col-md-6">
                <div class="panel panel-danger">
                    <div class="panel-heading">
                        <h3 class="panel-title">Delete User?</h3>
                    </div>
                    <div class="panel-body">
                        <% Html.BeginForm("Delete", "Users", FormMethod.Post, new { @class = "form-horizontal" }); %>
                        <h5 class="">Are you sure you want to delete the User?</h5>
                        <%: Html.HiddenFor(m => m.ID)  %>
                        <input type="submit" value="Delete" class="btn btn-danger" />
                        <a href="<%: Url.Action("Index", "Users") %>" class="btn btn-default">Back</a>

                        <% Html.EndForm(); %>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
