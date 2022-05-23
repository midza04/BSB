<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Dashboard.Models.UploadModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    XML Uploads
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-10">
        <div class="panel panel-default">
            <div class="panel-heading">Uploads to Date</div>
            <div class="panel-body">
                <table id="uploadsTable" class="display table table-striped table-bordered table-hover" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>File Name</th>
                            <th>Records Count</th>
                            <th>File Sent</th>
                            <th>Date Created</th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th>ID</th>
                            <th>File Name</th>
                            <th>Records Count</th>
                            <th>File Sent</th>
                            <th>Date Created</th>
                        </tr>
                    </tfoot>
                    <tbody>
                        <% foreach (var upload in Model) { %>
                        <tr>
                            <td><%: upload.ID %></td>
                            <td><%: upload.FileNameXML %></td>
                            <td><%: upload.RecordCount %></td>
                            <td><%: upload.SendYesNo %></td>
                            <td><%: upload.DateCreated %></td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="panel panel-default">
            <div class="actions-panel panel-heading">
                Actions
            </div>
            <div class="panel-body">                
                <ul class="action-links">
                    <li><a href="#" class="btn btn-running">Regenerate XML</a></li>
                </ul>

            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
