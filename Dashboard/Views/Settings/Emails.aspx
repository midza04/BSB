<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dashboard.Models.EmailParameterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Email Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-lg-10 col-lg-offset-1">
            <div class="panel panel-default">
                <div class="panel-heading">Email Settings</div>
                <div class="panel-body">
                    <% if (ViewBag.Updated == true)
                       { %>
                    <div class="alert alert-dismissible alert-success">
                        <button data-dismiss="alert" class="close" type="button"><i class="fa fa-remove"></i></button>
                        Settings have been updated.               
                    </div>
                    <% } %>
                    <% Html.BeginForm("SaveEmailSettings", "Settings", FormMethod.Post, new { @class = "form-horizontal" }); %>

                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">Support Person No. 1 Name</label>
                        <div class="col-sm-8">
                            <%: Html.TextBoxFor( m => m.EmailSignatureName1, new { @class = "form-control" })  %>
                        </div>
                    </div>

                    <div class="hr-dashed"></div>

                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">Support Person No. 1 Email Address</label>
                        <div class="col-sm-8">
                            <%: Html.TextBoxFor( m => m.EmailSignatureEmail1, new { @class = "form-control" })  %>
                        </div>
                    </div>

                    <div class="hr-dashed"></div>

                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">Support Person No. 1 Phone Number</label>
                        <div class="col-sm-8">
                            <%: Html.TextBoxFor( m => m.EmailSignaturePhoneNumber1, new { @class = "form-control" })  %>
                        </div>
                    </div>

                    <div class="hr-dashed"></div>

                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">Support Person No. 2 Name</label>
                        <div class="col-sm-8">
                            <%: Html.TextBoxFor( m => m.EmailSignatureName2, new { @class = "form-control" })  %>
                        </div>
                    </div>

                    <div class="hr-dashed"></div>

                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">Support Person No. 2 Email Address</label>
                        <div class="col-sm-8">
                            <%: Html.TextBoxFor( m => m.EmailSignatureEmail2, new { @class = "form-control" })  %>
                        </div>
                    </div>

                    <div class="hr-dashed"></div>

                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">Support Person No. 2 Phone Number</label>
                        <div class="col-sm-8">
                            <%: Html.TextBoxFor( m => m.EmailSignaturePhoneNumber2, new { @class = "form-control" })  %>
                        </div>
                    </div>
                    <div class="hr-dashed"></div>
                    <div class="form-group">
                        <label class="col-sm-4 control-label control-label-left">No records email response</label>
                        <div class="col-sm-8">
                            <%: Html.TextAreaFor( m => m.EmailNoRecordsMessage, new { @class = "form-control", rows = 8})  %>
                        </div>
                    </div>


                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-2">
                            <button type="submit" class="btn btn-primary">Save changes</button>
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
