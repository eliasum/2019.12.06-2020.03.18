<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FbAccountContacts.ascx.cs" Inherits="SmartPartsFbConsult_Account_FbAccountContacts" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<dx:ASPxGridView ID="grdContacts" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="n-checkbox" KeyFieldName="Id" EnableCallBacks="False">
    <Columns>
        <dx:GridViewDataHyperLinkColumn FieldName="Id" Caption="<%$ resources: grdContacts.FullName %>" Width="180px">
            <PropertiesHyperLinkEdit  NavigateUrlFormatString = "~/Contact.aspx?entityid={0}&modeid=Detail" TextField="FullName" />
        </dx:GridViewDataHyperLinkColumn>                                                                           <%-- 1 --%>   

        <dx:GridViewDataCheckColumn FieldName="IsPrimary" Caption="<%$ resources: grdContacts.IsPrimary %>" Width="60">
            <PropertiesCheckEdit ValueChecked="T" ValueUnchecked="F" ValueType="System.String" />
        </dx:GridViewDataCheckColumn>                                                                               <%-- 2 --%>   

        <dx:GridViewDataTextColumn FieldName="WorkPhone" Caption="<%$ resources: grdContacts.WorkPhone1 %>" />      <%-- 3 --%>   
        <dx:GridViewDataTextColumn FieldName="OtherPhone" Caption="<%$ resources: grdContacts.WorkPhone2 %>" />     <%-- 4 --%>   
        <dx:GridViewDataTextColumn FieldName="Mobile" Caption="<%$ resources: grdContacts.MobilePhone %>" />        <%-- 5 --%>   
        <dx:GridViewDataTextColumn FieldName="Email" Caption="<%$ resources: grdContacts.Email %>" />               <%-- 6 --%>   
        <dx:GridViewDataTextColumn FieldName="Title" Caption="<%$ resources: grdContacts.Title %>" />               <%-- 7 --%>   
        <dx:GridViewDataTextColumn FieldName="Department" Caption="<%$ resources: grdContacts.Department %>" />     <%-- 8 --%>                     
    </Columns>
    <SettingsText EmptyDataRow="<%$ resources: Common.EmptyDataRow.Text %>" />
    <SettingsPager Mode="ShowAllRecords" />
</dx:ASPxGridView>

<SalesLogix:SmartPartToolsContainer runat="server" ID="FbAccountContacts_RTools" ToolbarLocation="right">
    <asp:LinkButton ID="btnAdd" runat="server" CssClass="nomos-toolbar nomos-symbols-26" Style="color: black; font-weight: bold; font-size: 16px; margin-top: 2px;" ToolTip="<%$ resources: btnAdd.Tooltip  %>" />
</SalesLogix:SmartPartToolsContainer>