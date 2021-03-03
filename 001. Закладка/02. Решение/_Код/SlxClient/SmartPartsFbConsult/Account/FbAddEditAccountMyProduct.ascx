<%--Директива @ Control применяется для задания свойств при создании собственных пользовательских элементов управления.
    Для нового ПЭУ изменяются свойства CodeFile и Inherits. CodeFile - Название файла с отделенным кодом для данной страницы,
    Inherits - Класс, от которого наследуется класс данной страницы в технологии отделенного кода--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FbAddEditAccountMyProduct.ascx.cs" Inherits=" SmartPartsFbConsult_Account_FbAddEditAccountMyProduct" Debug="true" %>

<%--Директива @Register создает псевдонимы для пространств имен и пользовательских элементов управления --%>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="fbc" Namespace="FbConsult.Web.Controls" Assembly="FbConsult.Web.Controls" %>

<div class="shadowContainer" style="margin-top: 8px">
    <!-- Контрол LookupControl с ID="lueAccount" только для чтения выводит название сущности LookupEntityName="Account" -->
    <SalesLogix:LookupControl runat="server" ID="lueAccount" ReadOnly="true" LookupEntityName="Account" LookupEntityTypeName="Sage.Entity.Interfaces.IAccount, Sage.Entity.Interfaces, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" InitializeLookup="true" AutoPostBack="true">
    </SalesLogix:LookupControl>
    <table class="formtable">
        <tr>
            <td>
                <!-- метка label с ID="lblFbMyProduct" привязана к LookupControl с ID="lueFbMyProduct" и выводит текст Text, 
                указанный в файле ресурсов FbAddEditAccountMyProduct.ascx.ru.resx в поле resProductname со значением ID субъекта-->
                <asp:Label ID="lblFbMyProduct" AssociatedControlID="lueFbMyProduct" runat="server" Text="<%$ resources: resProductname %>" />
            </td>
            <td>
                <!-- Контрол LookupControl с ID="lueFbMyProduct" выводит название сущности LookupEntityName="FbMyProduct"
                Значения свойств контрола LookupControl указаны в файле ресурсов FbAddEditAccountMyProduct.ascx.ru.resx-->
                <SalesLogix:LookupControl runat="server" ID="lueFbMyProduct" ReadOnly="false" LookupEntityName="FbMyProduct" LookupEntityTypeName="Sage.Entity.Interfaces.IFbMyProduct, Sage.Entity.Interfaces, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" InitializeLookup="true" AutoPostBack="true">
                    <LookupProperties>
                        <SalesLogix:LookupProperty PropertyHeader="<%$ resources: lueAccount.LookupProperties.ProductName.PropertyHeader %>" PropertyName="ProductName" />
                        <SalesLogix:LookupProperty PropertyHeader="<%$ resources: lueAccount.LookupProperties.ProductType.PropertyHeader %>" PropertyName="ProductType" />
                    </LookupProperties>
                </SalesLogix:LookupControl>
            </td>
        </tr>
        <tr>
            <td>
                <!-- метка label с ID="lblAmount" привязана к текстбокс с ID="txtAmount" и выводит текст Text, 
                указанный в файле ресурсов FbAddEditAccountMyProduct.ascx.ru.resx в поле resAmount со значением ID субъекта-->
                <asp:Label ID="lblAmount" AssociatedControlID="txtAmount" runat="server" Text="<%$ resources: resAmount %>" />
            </td>
            <td>
                <div class="textcontrol" style="width: 100%">
                    <dx:ASPxTextBox runat="server" ID="txtAmount" Width="100%" ClientInstanceName="FbAddEditAccountMyProduct_txtAmount" MaxLength="32" CssClass="textcontrol-editable">
                        <ValidationSettings Display="None" RequiredField-IsRequired="True" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip" ValidateOnLeave="true" />
                        <InvalidStyle ForeColor="#d24c4b" />
                    </dx:ASPxTextBox>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <!-- метка label с ID="lblInterestrate" привязана к текстбокс с ID="txtInterestrate" и выводит текст Text, 
                указанный в файле ресурсов FbAddEditAccountMyProduct.ascx.ru.resx в поле resInterestrate со значением ID субъекта-->
                <asp:Label ID="lblInterestrate" AssociatedControlID="txtInterestrate" runat="server" Text="<%$ resources: resInterestrate %>" />
            </td>
            <td>
                <div class="textcontrol" style="width: 100%">
                    <dx:ASPxTextBox runat="server" ID="txtInterestrate" Width="100%" ClientInstanceName="FbAddEditAccountMyProduct_txtInterestrate" MaxLength="32" CssClass="textcontrol-editable">
                        <ValidationSettings Display="None" RequiredField-IsRequired="True" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip" ValidateOnLeave="true" />
                        <InvalidStyle ForeColor="#d24c4b" />
                    </dx:ASPxTextBox>
                </div>
            </td>
        </tr>
    </table>
    <asp:Button runat="server" ID="btnSave" Text="<%$ resources: btnSave.ToolTip %>" Style="margin-right: 4px;" Width="140px" CssClass="slxbutton" />
</div>
