using FbConsult.Web.Extensions.SmartParts;
using Sage.Entity.Interfaces;
using Sage.Platform.Orm;
using Sage.Platform.WebPortal.Binding;
using System;
using System.Linq;
using System.Web.UI;

public partial class SmartPartsFbConsult_Account_FbAccountAcceptClassification : FbEntityBoundSmartPartInfoProvider<IAccount>
{
    private IAccount _parentAccount;
    public IAccount ParentAccount
    {
        get
        {
            if (_parentAccount != null) return _parentAccount;
            return _parentAccount = GetParentEntity() as IAccount;
        }
    }

    protected override void OnActivating()
    {
        base.OnActivating();
        CurrentEntity.Category = CurrentEntity.ProposedCategory;
        CurrentEntity.CreditRating = CurrentEntity.ProposedCreditRating;
    }

    protected override void OnAddEntityBindings()
    {
        base.OnAddEntityBindings();
        BindingSource.Bindings.Add(new WebEntityBinding("CreditRating", pklCreditRating, "PickListValue"));
        BindingSource.Bindings.Add(new WebEntityBinding("Category", pklCategory, "PickListValue"));
        BindingSource.Bindings.Add(new WebEntityBinding("RegistrationDate", dtpRegistrationDate, "DateTimeValue"));
        BindingSource.Bindings.Add(new WebEntityBinding("AuditDescription", txtComment, "Text"));
    }

    protected override void OnWireEventHandlers()
    {
        base.OnWireEventHandlers();

        ClientBindingMgr.RegisterSaveButton(btnOK);
        ClientBindingMgr.RegisterDialogCancelButton(btnClose);
        btnClose.Click += DialogService.CloseEventHappened;
        btnOK.Click += UpdateDataEvent;
    }

    private void UpdateDataEvent(object sender, EventArgs e)
    {
        var groupId = string.Empty;
        var message = string.Empty;

        using (var session = new SessionScopeWrapper())
        {
            groupId = session.CreateSQLQuery("select FB_COMGACC.FB_COMPANIESGROUPID from FB_COMGACC where FB_COMGACC.ENTITYID = :accountId")
                .SetString("accountId", Convert.ToString(ParentAccount.Id))
                .List<string>()
                .FirstOrDefault();
        }

        if (groupId == null) message += (string.IsNullOrEmpty(message) ? string.Empty : "<br/>") + " - " + Convert.ToString(GetLocalResourceObject("lblCompanyGroupName.Text")).Replace(":", string.Empty);

        if (!string.IsNullOrEmpty(message))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_RequiredFiledsValidation", "setTimeout(function() { Sage.UI.Dialogs.alert('" + GetLocalResourceObject("RequiredFiledsValidation.Message") + "<br/>" + message + "'); }, 1);", true);
            return;
        }

        CurrentEntity.Category = "3" + pklCategory.PickListValue;
        CurrentEntity.RegistrationDate = dtpRegistrationDate.DateTimeValue.HasValue ? (DateTime?)dtpRegistrationDate.DateTimeValue.Value.ToLocalTime() : null;
        CurrentEntity.Save();

        using (var session = new SessionScopeWrapper())
        {
            session.Refresh(CurrentEntity);
        }

        PanelRefresh.RefreshAll();
        DialogService.CloseEventHappened(sender, e);
    }
}