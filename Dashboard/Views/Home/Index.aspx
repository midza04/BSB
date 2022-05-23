<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    DBFfintech Dashboard for interface to goAML
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    
    <div class="col-md-12">
        <h3 class="page-title">DBFfintech dashboard</h3>   
        <div class="row">                   
            <div class="col-md-3">
                <div class="panel panel-default">
                    <div class="panel-body bk-success-dashboard text-light" id="service-status-wrapper">
                        <div class="stat-panel text-center">
                            <div class="stat-panel-number h1 " id="service-status">--</div>
                            <div class="stat-panel-title text-uppercase">DBFFintech Service</div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">
                        <a class="btn btn-warning btn-sm" href="#" id="restartService">Restart Service</a>                         
                    </div>                    
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel panel-default">
                    <div class="panel-body bk-success-dashboard text-light" id="sql-status-wrapper">
                        <div class="stat-panel text-center">
                            <div class="stat-panel-number h1 " id="sql-status">--</div>
                            <div class="stat-panel-title text-uppercase">SQL Server</div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">                        
                        <a class="btn btn-running btn-sm" href="#">Service Running</a>
                        <%--<a class="btn btn-warning btn-sm" href="#" id="restartSQL">Restart Service</a>
                        <a class="btn btn-danger btn-sm" href="#" id="toggleSQLService" data-servicestate="Started">Stop Service</a>--%>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel panel-default">
                    <div class="panel-body bk-info text-light">
                        <div class="stat-panel text-center">
                            <div class="stat-panel-number h1 ">--</div>
                            <div class="stat-panel-title text-uppercase">Records from CB tomorrow
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">
                        <a class="btn btn-running-old btn-sm" href="#">View Records</a>                        
                        <a class="btn btn-running-old btn-sm" href="#">Generate</a>                        
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel panel-default">
                    <div class="panel-body bk-success-dashboard  text-light">
                        <div class="stat-panel text-center">
                            <div class="stat-panel-number h1 " id="">--</div>
                            <div class="stat-panel-title text-uppercase">Records from CB yesterday</div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">
                        <a class="btn btn-running btn-sm" href="#">View Records</a>                        
                        <a class="btn btn-running btn-sm" href="#">Regenerate</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">Uploaded Records Per Day</div>
                <div class="panel-body">
                    <div class="chart">
                        <canvas id="dashReport"></canvas>
                    </div>
                    <div id="legendDiv"></div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
                <% Html.RenderAction("UploadedFilesPartial", "Uploads"); %>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="scriptContent" runat="server" ContentPlaceHolderID="ScriptsSection">
    <%: Scripts.Render("~/bundles/dashboardMain")%> 
    <script src="signalr/hubs"></script>
</asp:Content>