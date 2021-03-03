<!-- Директива @Control - Применяется для задания свойств при создании собственных пользовательских элементов управления. -->
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FbAccountMyProduct.ascx.cs" Inherits="SmartPartsFbConsult_Account_FbAccountMyProduct" Debug="true" %>
<!--
AutoEventWireup - true, если события для страниц ASP.NET автоматически подключаются к 
функциям обработки событий; в противном случае false. Значение по умолчанию — true.
-->

<!-- Директива @Register - Создает псевдонимы для пространств имен и пользовательских элементов управления -->
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<!--
TagPrefix="dx" - Получает или задает префикс тега, связанного с исходным файлом или пространством имен и сборкой.
TagPrefix Свойство определяет псевдоним, используемый для пространства имен, в котором находится элемент управления.
Он применяется к пользовательским элементам управления. Это значение должно предшествовать имени тега в коде ASP.NET. 
-->
<dx:ASPxGridView ID="grdFbAccountMyProduct" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="n-checkbox" KeyFieldName="Id" EnableCallBacks="False">
    <Columns>
        <%-- Создать с помощью элемента ASPxGridView таблицу с 4-мя столбцами: --%>
        <%-- В первом столбце выводится кнопка редактирования id="btnEdit" и кнопка удаления ID="btnDelete", по нажатии на которую 
        выполняется обработчик btnDelete_OnClick, инициализация кнопок в методе grdFbAccountMyProduct_CustomButtonInitialize()
        OnInit() вызывается в момент динамического добавления элемента к форме--%>
        <dx:GridViewDataTextColumn Name="Command" Caption=" " Width="70px" Settings-AllowHeaderFilter="False">
            <EditFormSettings Visible="False" />
            <DataItemTemplate>
                <a runat="server" id="btnEdit" oninit="grdFbAccountMyProduct_CustomButtonInitialize" style="font-size: 17px; display: inline-block;" 
                    class="nomos-icon nomos-icon-active nomos-documents-7" title="<%$ resources: btnEdit.ToolTip %>"></a>
                <asp:LinkButton runat="server" ID="btnDelete" OnInit="grdFbAccountMyProduct_CustomButtonInitialize" Style="font-size: 17px; display:
                    inline-block;" class="nomos-icon nomos-icon-active nomos-symbols-22" title="<%$ resources: btnDelete.ToolTip %>" OnClick="btnDelete_OnClick" />
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

        <%-- В следующих трех столбцах выводятся данные, которые взяты из источника данных 
        grdFbAccountMyProduct: PRODUCTNAME, AMOUNT, INTERESTRATE. Названия столбцов таблицы из файла FbAccountMyProduct.ascx.ru.resx--%>
        <dx:GridViewDataTextColumn FieldName="PRODUCTNAME" Caption="<%$ resources: grdFbAccountMyProduct.PRODUCTNAME %>" />
        <dx:GridViewDataTextColumn FieldName="AMOUNT" Caption="<%$ resources: grdFbAccountMyProduct.AMOUNT %>" />
        <dx:GridViewDataTextColumn FieldName="INTERESTRATE" Caption="<%$ resources: grdFbAccountMyProduct.INTERESTRATE %>" />
    </Columns>
    <SettingsText EmptyDataRow="<%$ resources: Common.EmptyDataRow.Text %>" />
    <SettingsPager Mode="ShowAllRecords" />
</dx:ASPxGridView>

<SalesLogix:SmartPartToolsContainer runat="server" ID="FbAccountMyProduct_RTools" ToolbarLocation="right">
    <asp:LinkButton ID="btnAdd" runat="server" CssClass="nomos-toolbar nomos-symbols-26" Style="color: black; font-weight: bold; font-size: 16px; margin-top: 2px;" ToolTip="<%$ resources: btnAdd.Tooltip  %>" />
</SalesLogix:SmartPartToolsContainer>
