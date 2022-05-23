<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Dashboard.Models.LoggerModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Logger
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <!-- Zero Configuration Table -->
        <div class="panel panel-default">
            <div class="panel-heading">Logger Entries</div>            
            <div class="panel-body">
                <table id="loggerTable" class="display table table-striped table-bordered table-hover" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Step Description</th>
                            <th>Logger Description</th>
                            <th>Date Logged</th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th>ID</th>
                            <th>Step Description</th>
                            <th>Logger Description</th>
                            <th>Date Logged</th>
                        </tr>
                    </tfoot>
                    <tbody>
                        <% foreach(var logger in Model) { %>
                        <tr>
                            <td><%: logger.ID %></td>
                            <td><%: logger.StepDescription %></td>
                            <td><%: logger.LoggerDescription %></td>
                            <td><%: logger.DateLogged %></td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
