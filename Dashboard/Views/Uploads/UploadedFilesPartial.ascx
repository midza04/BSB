<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Dashboard.Models.UploadModel>>" %>
<div class="panel panel-default">
    <div class="panel-heading">Recently generated files</div>
    <div class="panel-body">
        <table class="display table table-striped table-bordered table-hover dashboard-table" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>File Name</th>
                    <th>Records In File</th>
                    <th>File Sent</th>
                    <th>Date Created</th>
                </tr>
            </thead>            
            <tbody>
                <% foreach (var upload in Model)
                   { %>
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
