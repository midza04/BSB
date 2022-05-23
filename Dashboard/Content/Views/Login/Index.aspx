<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.LoginModel>" %> 
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>BSB goAML Service Log in</title>
    <link type="image/png" href="http://www.bsb.bw/sites/default/files/logo_0.png" rel="shortcut icon">
    <meta name="viewport" content="width=device-width" />
    <%--<%: Styles.Render("~/Content/css") %>--%>
    <%: Styles.Render("~/Content/themes/dashboard")%>
    <%: Scripts.Render("~/bundles/modernizr")%>
</head>
<body>    
    <div class="login-page bk-img">
        <img src="<%: Url.Content("~/Content/Images/logo-dbffintech.png") %>" style="display: block; margin: 45px auto;"/>            
        <div class="form-content">
            <div class="container">
                <div class="row">
                    <div class="col-md-6 col-md-offset-3">                        
                        <div class="well row pt-2x pb-3x bk-bsb-blue well-login" style="box-shadow:5px 5px 15px 2px rgba(0,0,0,0.2);">
                            <div class="col-md-8 col-md-offset-2">                                      
                                <img src="<%: Url.Content("~/Content/Images/logo.png") %>" style="display: block; margin: 0 auto;width: 100%;"/>                                                          
                                <% if(ViewBag.Message != null){ %>
                                <div class="alert alert-danger" style="margin-top: 15px;">
                                    <%: ViewBag.Message %>
                                </div>
                                <% } %>
                                <% Html.BeginForm("Index", "Login", FormMethod.Post, new { @class = "mt bsb-login" }); %>
                                <label class="text-uppercase text-sm">Username</label>
                                <%: Html.TextBoxFor( m => m.UserName, new { @class = "form-control mb" })  %>


                                <label class="text-uppercase text-sm">Password</label>
                                <%: Html.PasswordFor( m => m.Password, new { @class = "form-control mb" })  %>

                                <div class="center-block">
                                    <button type="submit" class="btn btn-primary btn-primary-login" style="display: inherit; margin: 0 auto;">LOGIN</button>
                                </div>                                                                                    

                                <% Html.EndForm(); %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>

</html>
