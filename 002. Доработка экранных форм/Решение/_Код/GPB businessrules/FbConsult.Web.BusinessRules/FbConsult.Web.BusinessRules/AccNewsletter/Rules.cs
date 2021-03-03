using Sage.Entity.Interfaces;
using Sage.Platform;
using Sage.Platform.Application;
using Sage.Platform.ChangeManagement;
using Sage.Platform.ComponentModel;
using Sage.Platform.Data;
using Sage.Platform.Orm;
using Sage.Platform.Repository;
using Sage.Platform.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace FbConsult.Web.BusinessRules.AccNewsletter
{
    public static class Rules
    {
        #region Methods 
        public static void CheckAccContactInfoStep(IAccNewsletter accnewsletter)
        {
            if (!String.IsNullOrEmpty(accnewsletter.Email))
            {
                IQueryable qry = EntityFactory.GetRepository<IAccContactInfo>() as IQueryable;
                IExpressionFactory ep = qry.GetExpressionFactory();
                ICriteria crt = qry.CreateCriteria();

                crt.Add(ep.Eq("AccountId", accnewsletter.Accountid));
                crt.Add(ep.Eq("ContactValue", accnewsletter.Email));

                IList<IAccContactInfo> lst = crt.List<IAccContactInfo>();
                if (lst.Count > 0)
                {
                    foreach (IAccContactInfo vEntity in lst)
                    {
                        vEntity.Comments = "Рассылка сведений об операциях";
                        vEntity.IsActual = true;
                        vEntity.Save();
                    }
                }
                else
                {
                    IAccContactInfo com = EntityFactory.Create<IAccContactInfo>();
                    com.AccountId = accnewsletter.Accountid;
                    com.IsActual = true;
                    if (accnewsletter.DeliveryType.Contains("E-mail"))
                    {
                        com.ContactType = "E-mail для рассылки";
                    }
                    else
                    {
                        com.ContactType = "Телефон для рассылки";
                    }
                    com.Comments = "Рассылка сведений об операциях";
                    com.ContactValue = accnewsletter.Email;
                    com.Save();
                }
            }
        }

        public static void UnsetAllMainStep(IAccNewsletter accnewsletter)
        {
            if (accnewsletter != null && accnewsletter.IsActual.HasValue && accnewsletter.IsActual.Value == true)
            {
                IQueryable qry = EntityFactory.GetRepository<IAccNewsletter>() as IQueryable;
                IExpressionFactory ep = qry.GetExpressionFactory();
                ICriteria crt = qry.CreateCriteria();

                crt.Add(ep.Eq("Accountid", accnewsletter.Accountid));
                crt.Add(ep.Eq("IsActual", true));
                crt.Add(ep.Ne("Id", accnewsletter.Id.ToString()));
                crt.Add(ep.Eq("DeliveryType", accnewsletter.DeliveryType));
                IList<IAccNewsletter> lst = crt.List<IAccNewsletter>();

                foreach (IAccNewsletter rec in lst)
                {
                    rec.IsActual = false;
                    rec.Save();
                }
            }
        }
        #endregion

        #region Events
        public static void OnAfterInsertStep(IAccNewsletter accnewsletter)
        {
            accnewsletter.UnsetAllMain();
            //accnewsletter.CheckAccContactInfo();
            using (var session = new SessionScopeWrapper())
            {
                // обновить контакты
                if (accnewsletter.AccContactInfo != null)
                    CommonProcedures.UpdateAccountContacts(accnewsletter.AccContactInfo.AccountId, session);
            }
        }

        public static void OnAfterDelete(IAccNewsletter accnewsletter)
        {
            using (var session = new SessionScopeWrapper())
            {
                // обновить контакты
                if (accnewsletter.AccContactInfo != null)
                    CommonProcedures.UpdateAccountContacts(accnewsletter.AccContactInfo.AccountId, session);
            }
        }

        public static void OnAfterUpdateStep(IAccNewsletter accnewsletter)
        {
            accnewsletter.UnsetAllMain();

            using (var session = new SessionScopeWrapper())
            {
                // обновить контакты
                if (accnewsletter.AccContactInfo != null)
                CommonProcedures.UpdateAccountContacts(accnewsletter.AccContactInfo.AccountId, session); 
            }

            //accnewsletter.CheckAccContactInfo();
        }

        public static void OnBeforeInsertStep(IAccNewsletter accnewsletter, NHibernate.ISession session)
        {
            /*accnewsletter.IsActual = true;
            if (!String.IsNullOrEmpty(accnewsletter.DeliveryType))
            {
                string[] arr = accnewsletter.DeliveryType.Split(';');
                string el = arr[0];
                for (int i = 1; i < arr.Length; i++)
                {
                    el = arr[i];
                    IQueryable qry = EntityFactory.GetRepository<IAccNewsletter>() as IQueryable;
                    IExpressionFactory ep = qry.GetExpressionFactory();
                    ICriteria crt = qry.CreateCriteria();

                    crt.Add(ep.Eq("Accountid", accnewsletter.Accountid));
                    crt.Add(ep.Eq("IsActual", true));
                    crt.Add(ep.Eq("DeliveryType", el.Trim()));
                    IList<IAccNewsletter> lst = crt.List<IAccNewsletter>();

                    if (lst.Count > 0)
                    {
                        foreach (IAccNewsletter rec in lst)
                        {
                            rec.IsActual = false;
                            rec.Save();
                        }
                    }

                    IAccNewsletter com = EntityFactory.Create<IAccNewsletter>();
                    com.Accountid = accnewsletter.Accountid;
                    com.AttachId = accnewsletter.AttachId;
                    if ((el.Trim().ToLower().Contains("mail") || el.Trim().ToLower().Contains("мэйл")))
                    {
                        com.Email = accnewsletter.Email;
                    }
                    else
                    {
                        com.Phone = accnewsletter.Phone;
                        com.IsRussia = accnewsletter.IsRussia;
                        com.IsForeign = accnewsletter.IsForeign;
                    }
                    com.IsActual = true;
                    com.DeliveryType = el.Trim();
                    com.Subscription = accnewsletter.Subscription;
                    com.ModifyDate = accnewsletter.ModifyDate;
                    com.ModifyUser = accnewsletter.ModifyUser;
                    com.Save();
                }
                accnewsletter.DeliveryType = arr[0].Trim();
                if (arr[0].Trim().ToLower().Contains("mail") || arr[0].Trim().ToLower().Contains("мэйл"))
                {
                    accnewsletter.Phone = "";
                    accnewsletter.IsRussia = false;
                    accnewsletter.IsForeign = false;
                }
                else
                {
                    accnewsletter.Email = "";
                }
            }*/
        }

        public static void OnBeforeUpdateStep(IAccNewsletter accnewsletter, NHibernate.ISession session)
        {

            /*if (!String.IsNullOrEmpty(accnewsletter.DeliveryType))
            {
                string[] arr = accnewsletter.DeliveryType.Split(';');
                string el = arr[0];
                for (int i = 1; i < arr.Length; i++)
                {
                    el = arr[i];
                    IQueryable qry = EntityFactory.GetRepository<IAccNewsletter>() as IQueryable;
                    IExpressionFactory ep = qry.GetExpressionFactory();
                    ICriteria crt = qry.CreateCriteria();

                    crt.Add(ep.Eq("Accountid", accnewsletter.Accountid));
                    crt.Add(ep.Eq("IsActual", true));
                    crt.Add(ep.Eq("DeliveryType", el.Trim()));
                    IList<IAccNewsletter> lst = crt.List<IAccNewsletter>();

                    if (lst.Count > 0)
                    {
                        foreach (IAccNewsletter rec in lst)
                        {
                            rec.IsActual = false;
                            rec.Save();
                        }
                    }

                    IAccNewsletter com = EntityFactory.Create<IAccNewsletter>();
                    com.Accountid = accnewsletter.Accountid;
                    com.AttachId = accnewsletter.AttachId;
                    if ((el.Trim().ToLower().Contains("mail") || el.Trim().ToLower().Contains("мэйл")))
                    {
                        com.Email = accnewsletter.Email;
                    }
                    else
                    {
                        com.Phone = accnewsletter.Phone;
                        com.IsRussia = accnewsletter.IsRussia;
                        com.IsForeign = accnewsletter.IsForeign;
                    }
                    com.IsActual = true;
                    com.DeliveryType = el.Trim();
                    com.Subscription = accnewsletter.Subscription;
                    com.ModifyDate = accnewsletter.ModifyDate;
                    com.ModifyUser = accnewsletter.ModifyUser;
                    com.Save();
                }
                //Записываем старые значения

                string Email = accnewsletter.Email;
                string Phone = accnewsletter.Phone;
                bool? IsRussia = accnewsletter.IsRussia;
                bool? IsForeign = accnewsletter.IsForeign;
                string DeliveryType = arr[0];
                string Subscription = accnewsletter.Subscription;
                string AttachId = accnewsletter.AttachId;
                DateTime? ModifyDate = null;
                ModifyDate = accnewsletter.ModifyDate;
                string ModifyUser = accnewsletter.ModifyUser;

                IChangedState state = accnewsletter as IChangedState;

                PropertyChange change = state.GetChangedState().FindPropertyChange("DeliveryType");

                if (change != null)
                {
                    DeliveryType = (string)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("Email");

                if (change != null)
                {
                    Email = (string)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("Phone");

                if (change != null)
                {
                    Phone = (string)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("IsRussia");

                if (change != null)
                {
                    IsRussia = (bool?)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("IsForeign");

                if (change != null)
                {
                    IsForeign = (bool?)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("Subscription");

                if (change != null)
                {
                    Subscription = (string)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("AttachId");

                if (change != null)
                {
                    AttachId = (string)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("ModifyDate");

                if (change != null)
                {
                    ModifyDate = (DateTime?)change.OldValue;
                }

                change = state.GetChangedState().FindPropertyChange("ModifyUser");

                if (change != null)
                {
                    ModifyUser = (string)change.OldValue;
                }

                if (Phone != accnewsletter.Phone && Phone != null && accnewsletter.Phone != null || IsForeign != accnewsletter.IsForeign || IsRussia != accnewsletter.IsRussia || Email != accnewsletter.Email && Email != null && accnewsletter.Email != null || Subscription != accnewsletter.Subscription || AttachId != accnewsletter.AttachId || DeliveryType != accnewsletter.DeliveryType)
                {
                    //Записываем старые значения
                    IAccNewsletter com = EntityFactory.Create<IAccNewsletter>();
                    com.Accountid = accnewsletter.Accountid;
                    com.AttachId = AttachId;

                    if ((DeliveryType.ToLower().Contains("mail") || DeliveryType.ToLower().Contains("мэйл")))
                    {
                        com.Email = Email;
                    }
                    else
                    {
                        com.Phone = Phone;
                        com.IsRussia = IsRussia;
                        com.IsForeign = IsForeign;
                    }
                    com.IsActual = false;
                    com.DeliveryType = DeliveryType;
                    com.Subscription = Subscription;
                    com.ModifyDate = ModifyDate;
                    com.ModifyUser = ModifyUser;
                    com.Save();

                    accnewsletter.DeliveryType = arr[0].Trim();
                    accnewsletter.IsActual = true;
                    if (arr[0].Trim().ToLower().Contains("mail") || arr[0].Trim().ToLower().Contains("мэйл"))
                    {
                        accnewsletter.Phone = "";
                        accnewsletter.IsRussia = false;
                        accnewsletter.IsForeign = false;
                    }
                    else
                    {
                        accnewsletter.Email = "";
                    }
                }
            }*/
        }
        #endregion
    }
}
