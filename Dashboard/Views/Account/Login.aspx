<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.LoginModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log in
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4 col-md-offset-4">
            <div class="well row pt-2x pb-3x bk-light">
                <div class="col-md-8 col-md-offset-2">
                    <% using (Html.BeginForm()) { %>
                        <%--<label class="text-uppercase text-sm" for="">Username</label>
                        <input type="text" class="form-control mb" placeholder="Username">--%>

                    <%: Html.LabelFor(m => m.UserName, new { @class = "text-uppercase text-sm"} )%>
                    <%: Html.TextBoxFor(m => m.UserName, new { @class="form-control mb", placeholder="Username" })%>

                    <%: Html.LabelFor(m => m.Password, new { @class = "text-uppercase text-sm"} )%>
                    <%: Html.PasswordFor(m => m.Password, new { @class = "form-control mb", placeholder="Password"})%>
                    <button type="submit" class="btn btn-bsb btn-block">LOGIN</button>

                    <% } %>
                </div>
            </div>
            <div class="text-center text-light">
                <a class="text-light" href="#">Forgot password?</a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="scriptsContent" ContentPlaceHolderID="ScriptsSection" runat="server">
    <%: Scripts.Render("~/bundles/jqueryval") %>
</asp:Content>
