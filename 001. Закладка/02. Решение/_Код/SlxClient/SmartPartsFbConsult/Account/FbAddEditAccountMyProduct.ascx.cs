using DevExpress.Data;
using DevExpress.Export.Xl;
using FbConsult.Web.Extensions.NHibernate;
using FbConsult.Web.Extensions.SmartParts;
using Sage.Entity.Interfaces;
using Sage.Platform.Application;
using Sage.Platform.Orm;
using Sage.Platform.WebPortal.Binding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

public partial class SmartPartsFbConsult_Account_FbAddEditAccountMyProduct : FbEntityBoundSmartPartInfoProvider<IFbAccountMyProduct>
{
    // обработчик привязки данных текущей сущности к свойствам элементов
    protected override void OnAddEntityBindings()
    {
        base.OnAddEntityBindings();

        BindingSource.Bindings.Add(new WebEntityBinding("FbMyProduct", lueFbMyProduct, "LookupResultValue"));
        BindingSource.Bindings.Add(new WebEntityBinding("Amount", txtAmount, "Value"));
        BindingSource.Bindings.Add(new WebEntityBinding("Interestrate", txtInterestrate, "Value"));
    }

    // событие OnFormBound возникает после перезагрузки страницы и если были какие либо изменения на странице
    protected override void OnFormBound()
    {
        // вызов базового метода OnFormBound()
        base.OnFormBound();

        // текущая сущность
        var currentAccount = GetParentEntity() as IAccount;

        // Контрол LookupControl с ID="lueAccount" только для чтения выводит название текущей сущности LookupEntityName="Account"
        lueAccount.LookupResultValue = currentAccount;        
    }

    // обработчик событий при загрузке страницы
    protected override void OnWireEventHandlers()
    {
        base.OnWireEventHandlers();

        // добавить обработчик нажатия на кнопку добавления продукта в БД
        btnSave.Click += btnSave_Click;
    }

    // обработчик нажатия на кнопку добавления продукта в БД
    private void btnSave_Click(object sender, EventArgs e)
    {
        CurrentEntity.Amount = Convert.ToDecimal(txtAmount.Text);
        CurrentEntity.InterestRate = Convert.ToDecimal(txtInterestrate.Text);
        CurrentEntity.AccountId = Convert.ToString((lueAccount.LookupResultValue as IAccount).Id);
        CurrentEntity.FbMyProduct = lueFbMyProduct.LookupResultValue as IFbMyProduct;
        CurrentEntity.Save();
        DialogService.CloseEventHappened(sender, e);
        PanelRefresh.RefreshAll();
    }

}