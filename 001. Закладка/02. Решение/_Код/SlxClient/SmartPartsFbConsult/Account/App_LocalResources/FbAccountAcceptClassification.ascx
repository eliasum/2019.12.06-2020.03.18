<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FbAccountAcceptClassification.ascx.cs" Inherits="SmartPartsFbConsult_Account_FbAccountAcceptClassification" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<style type="text/css">
    .formtable.wideTextControls {
        padding: 0 10px;
    }

    .formtable {
        border-spacing: 5px;
    }

    .wideTextControls .lbl {
        width: 30%;
        margin-right: 20px;
    }

    .wideTextControls .textcontrol {
        width: 74%;
    }

    .wideTextControls .dxeTextBox_InforModerno {
        border: none;
        border-bottom: 1px solid #9b9b9b;
        box-shadow: none;
    }

    .wideTextControls .dxeTextBox_InforModerno td.dxic {
        padding: 0;
    }

    .wideTextControls .dxeReadOnly_InforModerno {
        background-color: transparent;
        border: none;
    }
</style>
<div class="hide-dialog-close-button"></div>
<div class="shadowContainer" style="padding: 0; box-shadow: none;">
    <table class="formtable fbconsultLayout wideTextControls" style="margin-left: 2%; margin-right: 2%;">
        <tr>
            <td>
                <div runat="server" id="divActivityKind">
                    <table class="formtable">
                        <tr>
                            <td>
                                <div class="lbl alignleft">
                                    <asp:Label runat="server" ID="lblCategory" AssociatedControlID="pklCategory" Text="<%$ resources: lblCategory.Text %>" />
                                </div>
                                <div class="textcontrol">
                                    <div class="textcontrol picklist cbo-arrow" style="width: 110%; margin-top: 5px">
                                        <SalesLogix:PickListControl runat="server" ID="pklCategory" PickListName="Категория Организации" CanEditText="False" DefaultPickListItem="Средний" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="lbl alignleft">
                                    <asp:Label runat="server" ID="lblCreditRating" AssociatedControlID="pklCreditRating" Text="<%$ resources: lblCreditRating.Text %>" />
                                </div>
                                <div class="textcontrol">
                                    <div class="textcontrol picklist cbo-arrow" style="width: 110%; margin-top: 5px">
                                        <SalesLogix:PickListControl runat="server" ID="pklCreditRating" PickListName="Основание классификации" CanEditText="False" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="lbl alignleft">
                                    <asp:Label runat="server" ID="lblRegistrationDate" AssociatedControlID="dtpRegistrationDate" Text="<%$ resources: lblRegistrationDate.Text %>" />
                                </div>
                                <div class="textcontrol">
                                    <div style="margin-top: 5px; width: 108%">
                                        <SalesLogix:DateTimePicker runat="server" ID="dtpRegistrationDate" DisplayTime="false" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr style="padding-top: 30px">
                            <td style="padding-top: 30px">
                                <div class="lbl alignleft" style="width: 130px;">
                                    <asp:Label runat="server" ID="lblComment" AssociatedControlID="txtComment" Text="<%$ resources: lblComment.Text %>" />
                                </div>
                                <div class="textcontrol textarea-container resize-none" style="width: 68%">
                                    <div style="margin-left: 15px;">
                                        <dx:ASPxMemo runat="server" ID="txtComment" ClientInstanceName="txtComment_FbAccountSuperEditDialog" Rows="3" MaxLength="1000" Width="70%">
                                            <ValidationSettings RequiredField-IsRequired="True" Display="Dynamic" />
                                            <InvalidStyle ForeColor="#d24c4b" />
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>

    <div style="padding-top: 10px; width: 94%; margin-left: 14px; text-align: right">
        <asp:Button runat="server" ID="btnOK" Text="<%$ resources: btnOK.Text %>" Width="80px" CssClass="slxbutton" />
        <asp:Button runat="server" ID="btnClose" Text="<%$ resources: btnClose.Text %>" Width="80px" CssClass="slxbutton" CausesValidation="false" />
    </div>

</div>
