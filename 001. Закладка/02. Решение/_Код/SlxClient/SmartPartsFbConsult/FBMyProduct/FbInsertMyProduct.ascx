<!-- Директива @Control - Применяется для задания свойств при создании собственных пользовательских элементов управления. -->
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FbInsertMyProduct.ascx.cs" Inherits="SmartPartsFbConsult_Account_FbInsertMyProduct" Debug="true" %>
<!--
AutoEventWireup - true, если события для страниц ASP.NET автоматически подключаются к 
функциям обработки событий; в противном случае false. Значение по умолчанию — true.
-->

<!-- Директива @Register - Создает псевдонимы для пространств имен и пользовательских элементов управления -->
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="fbc" Namespace="FbConsult.Web.Controls" Assembly="FbConsult.Web.Controls" %>

<style>
    .shadowContainer {
        margin-top: 8px;
        margin-bottom: 8px;
        margin-left: 8px;
    }

        .shadowContainer .twocoltextcontrol {
            width: 80%;
        }

        .shadowContainer .fbconsultRadioButtonList {
            width: 80%;
            display: inline-block;
            vertical-align: middle;
        }

            .shadowContainer .fbconsultRadioButtonList > table {
                width: 50%;
            }

        .shadowContainer .dxeTextBox_InforModerno {
            border: none;
            /*border-bottom: 1px solid #9b9b9b;*/
            box-shadow: none;
        }

            .shadowContainer .dxeTextBox_InforModerno td.dxic {
                padding: 0;
            }

    .slxbutton {
        margin-left: 8px;
    }
</style>

<div class="mainContentScrollBox">
    <div class="mainContentContentBox" style="min-width: 800px; max-width: 66.6666%">
        <div class="shadowContainer">
            <table class="formtable fbconsultLayout" style="border-spacing: 8px">
                <tr>
                    <td>
                        <!-- метка label с ID="lblProductName" привязана к текстбокс с ID="txtProductName" и выводит текст Text, 
                         указанный в файле ресурсов FbInsertMyProduct.ascx.ru.resx в поле resProductName со значением ID субъекта-->
                        <asp:Label ID="lblProductName" AssociatedControlID="txtProductName" runat="server" Text="<%$ resources: resProductname %>" />
                    </td>
                    <td>
                        <div class="twocoltextcontrol">
                            <dx:ASPxTextBox runat="server" ID="txtProductName" Width="100%" ClientInstanceName="FbAddEditAccountMyProduct_resProductName" MaxLength="32" CssClass="textcontrol-editable">
                                <ValidationSettings Display="None" RequiredField-IsRequired="True" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip" ValidateOnLeave="true" />
                                <InvalidStyle ForeColor="#d24c4b" />
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- метка label с ID="lblProductType" привязана к текстбокс с ID="txtProductType" и выводит текст Text, 
                         указанный в файле ресурсов FbInsertMyProduct.ascx.ru.resx в поле resProductType со значением ID субъекта-->
                        <asp:Label ID="lblProductType" AssociatedControlID="txtProductType" runat="server" Text="<%$ resources: resProductType %>" />
                    </td>
                    <td>
                        <div class="twocoltextcontrol">
                            <dx:ASPxTextBox runat="server" ID="txtProductType" Width="100%" ClientInstanceName="FbAddEditAccountMyProduct_ProductType" MaxLength="32" CssClass="textcontrol-editable">
                                <ValidationSettings Display="None" RequiredField-IsRequired="True" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip" ValidateOnLeave="true" />
                                <InvalidStyle ForeColor="#d24c4b" />
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div runat="server" id="divButtonContainer" style="width: 100%; margin-left: 0px; text-align: left">
    <asp:Button runat="server" ID="btnSave" Text="<%$ resources: btnSave.ToolTip %>" Style="margin-right: 4px;" Width="140px" CssClass="slxbutton" />
    <asp:Button runat="server" ID="btnSaveNew" Text="<%$ resources: btnSaveNew.ToolTip %>" Width="140px" CssClass="slxbutton" />
</div>
