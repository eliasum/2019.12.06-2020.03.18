using DevExpress.Web;
using FbConsult.Web.Extensions.NHibernate;
using FbConsult.Web.Extensions.SmartParts;
using Sage.Entity.Interfaces;
using Sage.Platform;
using Sage.Platform.Orm;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class SmartPartsFbConsult_Account_FbAccountMyProduct : FbEntityBoundSmartPartInfoProvider<IAccount>
{
    // событие OnFormBound используется для регистрации клиентского сценария на странице
    protected override void OnFormBound()
     {
        // проверка выполнения каких либо действий на странице
        if (!IsCurrentTabVisible) return;


     }
}