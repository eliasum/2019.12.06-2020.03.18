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

        base.OnFormBound();

        //var newcontact = Sage.Platform.EntityFactory.GetById<IContact>("C6UJ9A000CW6");
        //btnAdd.OnClientClick = string.Format("Sage.Utility.loadDetailsDialog({{ entityType: 'Sage.Entity.Interfaces.IAssociation, Sage.Entity.Interfaces', entityId: '{0}', smartPart: 'FbAddEditAccountAssociation',dialogParameters: {{ Key:'EntityType', Value:'{2}' }}, dialogTitle: '{1}', isCentered: true, dialogHeight: 260, dialogWidth: 800 }});return false;", id, (string)(CurrentEntity.GetType().Name == "Account" ? GetLocalResourceObject("FbAddEditAccountAssociation.Header") : GetLocalResourceObject("FbAddEditContactAssociation.Header")), CurrentEntity.GetType().Name);
        
        using (var session = new SessionScopeWrapper())     // новая сессия
         {
            // определение и настройка источника данных для объектов на странице ascs
            grdFbAccountMyProduct.DataSource = 
                session.CreateSQLQuery(
                    "SELECT FB_ACCOUNTMYPRODUCT.FB_ACCOUNTMYPRODUCTID as Id, FB_ACCOUNTMYPRODUCT.AMOUNT, FB_MYPRODUCT.PRODUCTNAME, FB_ACCOUNTMYPRODUCT.INTERESTRATE FROM FB_ACCOUNTMYPRODUCT INNER JOIN FB_MYPRODUCT ON FB_ACCOUNTMYPRODUCT.MYPRODUCTID = FB_MYPRODUCT.fb_MYPRODUCTID WHERE ACCOUNTID = :accountId")  
                                             .SetString("accountId", Convert.ToString(CurrentEntity.Id))
                                             .ListAs(new { Id = default(string), AMOUNT = default(decimal), PRODUCTNAME = default(string), INTERESTRATE = default(decimal), });
            // привязка данных к источнику данных
            grdFbAccountMyProduct.DataBind();
         }
     }

    // обработчик событий при загрузке страницы
    protected override void OnWireEventHandlers()
    {
        base.OnWireEventHandlers();

        // добавить обработчик нажатия на кнопку добавления/изменения продукта btnAdd в событие нажатия на btnAdd
        btnAdd.Click += btnAdd_Click;
    }

    // обработчик нажатия на кнопку добавления/изменения продукта btnAdd
    private void btnAdd_Click(object sender, EventArgs e)
    {
        /* 
            вывод диалогового окна void SetSpecs(int height, int width, string mappedID, string titleOverride), где:
            string mappedID - FbAddEditAccountMyProduct.ascs - открываемый диалог
            string titleOverride - FbAddEditAccountMyProduct.Header.add - заголовок диалога, прописанный в файле ресурсов FbAccountMyProduct.ascx.ru.resx
        */
        DialogService.SetSpecs(260, 800, "FbAddEditAccountMyProduct", (string)(GetLocalResourceObject("FbAddEditAccountMyProduct.Header.add")));
        DialogService.EntityType = typeof(IFbAccountMyProduct); // тип сущности - интерфейс параметров продукта IFbAccountMyProduct
        DialogService.ShowDialog();                             // показать диалоговое окно
    }

    // метод OnInit() вызывается в момент динамического добавления элемента к форме
    protected void grdFbAccountMyProduct_CustomButtonInitialize(object sender, EventArgs e)
    {
        /*
        серверные элементы управления представляют собой классы в среде .NET Framework, представляющие визуальные элементы веб-формы 
        переменная anchor содержит стандартный HTML-дескриптор анкор <a>
        sender - это указатель на объект вызвавший это событие
        this - {ASP.smartpartsfbconsult_account_fbaccountmyproduct_ascx}
        */
        var anchor = sender as HtmlAnchor;  // {System.Web.UI.HtmlControls.HtmlAnchor}

        if (anchor == null) return;

        var visibleIndex = ((GridViewDataItemTemplateContainer)(anchor.NamingContainer)).VisibleIndex;

        if(anchor.ID == "btnEdit")
        {
            var result = ((ASPxGridView)(anchor.NamingContainer.NamingContainer.NamingContainer)).GetRowValues(visibleIndex, grdFbAccountMyProduct.KeyFieldName);
           anchor.Attributes["onclick"] = string.Format("Sage.Utility.loadDetailsDialog({{ entityType: 'Sage.Entity.Interfaces.IFbAccountMyProduct, Sage.Entity.Interfaces', entityId: '{0}', smartPart: 'FbAddEditAccountMyProduct', dialogTitle: '{1}', isCentered: true, dialogHeight: 300, dialogWidth: 480 }});return false;", result, Convert.ToString(GetLocalResourceObject("FbAddEditAccountMyProduct.Header.edit")));
        }
    }

    // обработчик нажатия на кнопку удаления продукта btnDelet
    protected void btnDelete_OnClick(object sender, EventArgs e)
    {
        var values = grdFbAccountMyProduct.GetRowValues(((GridViewDataItemTemplateContainer)((Control)sender).NamingContainer).VisibleIndex, grdFbAccountMyProduct.KeyFieldName);
        if (values == null) return;
        //if (values[1] as bool? ?? false)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_RequiredFieldsValidation", "setTimeout(function() { Sage.UI.Dialogs.alert('" + GetLocalResourceObject("DeleteIsPrime.Message") + "'); }, 1);", true);
        //    return;
        //}
        //if (values[2] as bool? ?? false)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_RequiredFieldsValidation", "setTimeout(function() { Sage.UI.Dialogs.alert('" + GetLocalResourceObject("DeleteIsMailing.Message") + "'); }, 1);", true);
        //    return;
        //}
        EntityFactory.GetById<IFbAccountMyProduct>(values).Delete();
        PanelRefresh.RefreshAll();
    }
}