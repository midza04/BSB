<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dashboard.Models.NavigationViewModel>" %>

<ul class="ts-sidebar-menu">
    <li class="ts-label">Menu</li>
    <% foreach (var menuItem in Model.MenuItems)
       { %>
    <li <%= menuItem.Selected? "class='open'" : "" %>>
        <a href="<%:Url.Action(menuItem.Action, menuItem.Controller, null) %>"><i class="fa fa-fw"></i><%:menuItem.Text %></a>
    </li>
    <% } %>
    <li>
       <a href="http://192.168.2.62/str-test/"><i class="fa fa-fw"></i>STR</a>

    </li>
</ul>
<div class="nav-bottom-details">
    Username : <%: Model.UserName %> <br />
    IntegratorAML Interface Version 1.1
</div>
