<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.UserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    New User
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <div class="panel panel-default">
                <div class="panel-heading">New User</div>
                <div class="panel-body">
                    <% Html.BeginForm("SaveUser", "Users", FormMethod.Post, new { @class = "form-horizontal" }); %>
                    <%: Html.HiddenFor(m => m.ID)  %>
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Username</label>
                        <div class="col-sm-10">
                            <%: Html.TextBoxFor( m => m.Username, new { @class = "form-control" })  %>
                        </div>
                    </div>
                    <div class="hr-dashed"></div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Is Enabled</label>
                        <div class="col-sm-8">
                            <div class="radio radio-info radio-inline">
                                <%= Html.RadioButtonFor(m => m.IsEnabled, true) %>
                                <label for="inlineRadio1">Yes </label>
                            </div>
                            <div class="radio radio-inline">
                                <%= Html.RadioButtonFor(m => m.IsEnabled, false) %>
                                <label for="inlineRadio2">No </label>
                            </div>
                        </div>
                    </div>
                    <div class="hr-dashed"></div>

                    <div class="form-group">
                        <div class="col-sm-8 col-sm-offset-2">
                            <button type="submit" class="btn btn-primary">Save User</button>
                            <a href="<%: Url.Action("Index", "Users") %>" class="btn btn-default">Back</a>
                        </div>
                    </div>

                    <% Html.EndForm(); %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
