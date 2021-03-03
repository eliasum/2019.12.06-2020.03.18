using NHibernate;
using Sage.Entity.Interfaces;
using Sage.Platform;
using System;

namespace FbConsult.Web.BusinessRules.AccContactInfo
{
	public static class Rules
	{
		public static void OnBeforeInsertStep(IAccContactInfo contact, ISession session)
		{
            if (contact == null && contact.IsMain == false && contact.Account == null && contact.ContactType == null)
            {
                return;
            }

			EnsureContactPrimary(contact, session);

            // обновить контакты
            CommonProcedures.UpdateAccountContacts(contact.AccountId, session);
        }

		public static void OnBeforeUpdateStep(IAccContactInfo contact, ISession session)
		{
            if (contact == null && contact.IsMain == false && contact.Account == null && contact.ContactType == null)
            {
                return;
            }

            EnsureContactPrimary(contact, session);

            // обновить контакты
            CommonProcedures.UpdateAccountContacts(contact.AccountId, session);
        }

        public static void OnBeforeDeleteStep(IAccContactInfo contact, ISession session)
        {
            if (contact == null)
            {
                return;
            }

            // обновить контакты
            CommonProcedures.UpdateAccountContacts(contact.AccountId, session);
        }

        private static void EnsureContactPrimary(IAccContactInfo contact, ISession session)
		{
			if (contact.ContactType.Contains("Тел"))
			{
				var queryList = session.CreateSQLQuery($"select ACC_CONTACT_INFOID from ACC_CONTACT_INFO where ACCOUNTID = '{ contact.AccountId }'").AddScalar("ACC_CONTACT_INFOID", NHibernateUtil.String).List<String>();
				if (queryList == null || queryList.Count < 1) return;
				foreach (var contactId in queryList)
				{
					var foundContact = EntityFactory.GetById<IAccContactInfo>(contactId);
					if (foundContact != null && foundContact.Id != null && foundContact.Id != contact.Id && foundContact.ContactType != null && foundContact.ContactType.Contains("Тел"))
						foundContact.IsMain = false;
				}
			}
			if (contact.ContactType.ToLower().Contains("e-mail") || contact.ContactType.ToLower().Contains("email"))
			{
				var queryList = session.CreateSQLQuery($"select ACC_CONTACT_INFOID from ACC_CONTACT_INFO where ACCOUNTID = '{ contact.AccountId }'").AddScalar("ACC_CONTACT_INFOID", NHibernateUtil.String).List<String>();
				if (queryList == null || queryList.Count < 1) return;
				foreach (var contactId in queryList)
				{
					var foundContact = EntityFactory.GetById<IAccContactInfo>(contactId);
					if (foundContact != null && foundContact.Id != null && foundContact.Id != contact.Id && foundContact.ContactType != null && (foundContact.ContactType.ToLower().Contains("e-mail") || foundContact.ContactType.ToLower().Contains("email")))
						foundContact.IsMain = false;
				}
			}
		}
	}
}
