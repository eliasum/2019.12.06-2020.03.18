using DevExpress.Web;
using FbConsult.Web.Extensions.NHibernate;
using FbConsult.Web.Extensions.SmartParts;
using Sage.Entity.Interfaces;
using Sage.Platform;
using Sage.Platform.Orm;
using Sage.Platform.Utility;
using Sage.Platform.WebPortal.Binding;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class SmartPartsFbConsult_Account_FbInsertMyProduct : FbEntityBoundSmartPartInfoProvider<IFbMyProduct>
{
    // обработчик привязки данных текущей сущности к свойствам элементов
    protected override void OnAddEntityBindings()
    {
        base.OnAddEntityBindings();

        BindingSource.Bindings.Add(new WebEntityBinding("ProductName", txtProductName, "Text"));
        BindingSource.Bindings.Add(new WebEntityBinding("ProductType", txtProductType, "Text"));
    }

    // событие OnFormBound возникает после перезагрузки страницы и если были какие либо изменения на странице
    protected override void OnFormBound()
    {
        // вызов базового метода OnFormBound()
        base.OnFormBound();
    }

    // обработчик событий при загрузке страницы
    protected override void OnWireEventHandlers()
    {
        base.OnWireEventHandlers();

        // добавить обработчик нажатия на кнопку добавления продукта в БД
        btnSave.Click += btnSave_Click;

        // добавить обработчик нажатия на кнопку добавления продукта в БД и продолжения добавления
        btnSaveNew.Click += btnSaveNew_Click;
    }

    // обработчик нажатия на кнопку добавления продукта в БД
    private void btnSave_Click(object sender, EventArgs e)
    {
        SaveMyProduct(false);
    }

    // обработчик нажатия на кнопку добавления продукта в БД и продолжения добавления
    private void btnSaveNew_Click(object sender, EventArgs e)
    {
        SaveMyProduct(true);
    }

    private void SaveMyProduct(bool saveNew)
    {
        // проверка существования сущности
        if (CurrentEntity == null) return;

        // проверка заполнения необходимых полей
        var message = string.Empty;
        var messageInvalid = string.Empty;
        if (string.IsNullOrWhiteSpace(txtProductName.Text)) message += (string.IsNullOrEmpty(message) ? string.Empty : "<br />") + " - " + Convert.ToString(GetLocalResourceObject("resProductName")).Replace(":", string.Empty);
        if (string.IsNullOrWhiteSpace(txtProductType.Text)) message += (string.IsNullOrEmpty(message) ? string.Empty : "<br />") + " - " + Convert.ToString(GetLocalResourceObject("resProductType")).Replace(":", string.Empty);
        if (message.IsNotNullOrEmpty() || messageInvalid.IsNotNullOrEmpty())
        {
            if (message.IsNotNullOrEmpty()) message = $"{GetLocalResourceObject("RequiredFieldsValidation.Message")}<br />{message}<br />";
            if (messageInvalid.IsNotNullOrEmpty()) message += $"<br />{GetLocalResourceObject("RequiredFieldsValidation.Invalid.Message")}<br />{messageInvalid}";
            message = "setTimeout(function() { Sage.UI.Dialogs.alert('" + message + "'); }, 1);";
            ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_RequiredFieldsValidation", message, true);
            return;
        }

        // сохраниение сущности
        CurrentEntity.Save();

        // перенаправление после сохранения в зависимости от нажатой кнопки 
        Response.Redirect(saveNew ? "FbInsertMyProduct.aspx?modeid=Insert" : string.Format("FbMyProduct.aspx?entityId={0}", CurrentEntity.Id));
    }
}


