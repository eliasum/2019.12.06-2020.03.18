using FbConsult.Web.BusinessRules.Properties;
using log4net;
using NHibernate;
using Sage.Entity.Interfaces;
using Sage.Platform;
using Sage.Platform.Application;
using Sage.Platform.ChangeManagement;
using Sage.Platform.MapProvider;
using Sage.SalesLogix.Address;
using Sage.SalesLogix.BusinessRules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Saleslogix.Geocode.Interfaces;
using Saleslogix.Geocode.Managers;
using Sage.Platform.Orm;

namespace FbConsult.Web.BusinessRules.Address
{
    public static class Rules
    {
        #region Methods
        public static void GetEntityNameStep(IAddress address, out String result)
        {
            string entityId = address.EntityId;
            string str = string.Empty;
            if (entityId.Substring(0, 1) == "A")
            {
                str = "IACCOUNT";
            }
            if (entityId.Substring(0, 1) == "C")
            {
                str = "ICONTACT";
            }
            result = str;
        }
        #endregion

        #region Events 
        private static void OnBeforeDeleteStep_UpdateAccount(IAddress address)
        {
            IAccount accById = EntityFactory.GetById<IAccount>(address.EntityId);
            if (accById != null)
            {
                accById.UpdateShippMailAddress(address);

                // Если удаляют почтовый адрес, сделать почтовым основной
                if ((accById.ShippingAddress != null && accById.ShippingAddress.Equals(address)) ||
                    (accById.ShippingAddress == null && accById.Address != null))
                {
                    accById.ShippingAddress = accById.Address;
                    accById.Address.IsMailing = true;
                }

            }

            address.EntityId = null;
            address.IsMailing = false;
            address.IsPrimary = false;
        }

        private static void OnBeforeDeleteStep_UpdateContact(IAddress address)
        {
            IContact cntbyId = EntityFactory.GetById<IContact>(address.EntityId);
            if (cntbyId != null)
            {
                // Если удаляют почтовый адрес, сделать почтовым основной
                if (cntbyId.ShippingAddress != null && cntbyId.ShippingAddress.Equals(address) ||
                    (cntbyId.ShippingAddress == null && cntbyId.Address != null))
                {
                    cntbyId.ShippingAddress = cntbyId.Address;
                    cntbyId.Address.IsMailing = true;
                }
            }
        }

        public static void OnBeforeDeleteStep(IAddress address, ISession session)
        {
            if (address == null)
            {
                return;
            }

            switch (address.GetEntityName().ToUpper())
            {
                case "IACCOUNT":
                    OnBeforeDeleteStep_UpdateAccount(address);
                    break;
                case "ICONTACT":
                    OnBeforeDeleteStep_UpdateContact(address);
                    break;
            }

            OnBeforeDelete(address, session);
        }

        public static void OnAfterDeleteStep(IAddress address)
        {
            using (var session = new SessionScopeWrapper(false))
            {
                //обновить контакты
                CommonProcedures.UpdateAccountContacts(address.EntityId, session);
            }
        }

        public static void OnBeforeInsertStep(IAddress address, ISession session)
        {
            if (address == null)
            {
                return;
            }

            EnsureAddressPrimary(address, session);

            OnBeforeInsert(address, session);

            ValidateCountryCode(address);
        }

        public static void OnAfterInsertStep(IAddress address)
        {
            using (var session = new SessionScopeWrapper(false))
            {
                //обновить контакты
                CommonProcedures.UpdateAccountContacts(address.EntityId, session);
            }
        }

        private static void OnBeforeUpdateStep_UpdateAccount(IAddress address)
        {
            IAccount accById = EntityFactory.GetById<IAccount>(address.EntityId);
            if (accById != null)
            {
                accById.UpdateShippMailAddress(address);
            }

            if (accById != null && accById.Address != null)
            {
                // Галку "Основной" снимать нельзя
                if (address.IsPrimary == false && accById.Address.Equals(address))
                {
                    accById.Address.IsPrimary = true;
                }

                // Если сняли галку "Почтовый", то почтовым делается основной адрес
                if (address.IsMailing == false && (accById.ShippingAddress == null ||
                    (accById.ShippingAddress != null && accById.ShippingAddress.Equals(address))))
                {
                    accById.ShippingAddress = accById.Address;
                    accById.Address.IsMailing = true;
                }
            }
        }

        private static void OnBeforeUpdateStep_UpdateContact(IAddress address)
        {
            IContact cntbyId = EntityFactory.GetById<IContact>(address.EntityId);
            if (cntbyId != null && cntbyId.Address != null)
            {
                // Галку "Основной" снимать нельзя
                if (address.IsPrimary == false && cntbyId.Address.Equals(address))
                {
                    cntbyId.Address.IsPrimary = true;
                }

                // Если сняли галку "Почтовый", то почтовым делается основной адрес
                if (address.IsMailing == false && (cntbyId.ShippingAddress != null ||
                    (cntbyId.ShippingAddress != null && cntbyId.ShippingAddress.Equals(address))))
                {
                    cntbyId.ShippingAddress = cntbyId.Address;
                    cntbyId.Address.IsMailing = true;
                }
            }
        }

        public static void OnBeforeUpdateStep(IAddress address, ISession session)
        {
            if (address == null)
            {
                return;
            }

            EnsureAddressPrimary(address, session);

            OnBeforeUpdate(address, session);

            ValidateCountryCode(address);
        }

        public static void OnAfterUpdateStep(IAddress address)
        {
            using (var session = new SessionScopeWrapper(false))
            {
                //обновить контакты
                CommonProcedures.UpdateAccountContacts(address.EntityId, session);
            }
        }

        public static bool OnBeforeUpdate(IAddress address, ISession session)
        {
            if (string.IsNullOrEmpty(address.EntityId))
            {
                throw new ValidationException();
            }
            if (string.IsNullOrEmpty(address.Description) && !address.EntityId.StartsWith("U", StringComparison.Ordinal) && !address.EntityId.Trim().Equals("ADMIN"))
            {
                throw new ValidationException();
            }
            Rules.HandleAddressFlags(address, session);
            Rules.UpdateRelatedEntity(address, session);
            IChangedState changedState = address as IChangedState;
            if (changedState != null)
            {
                ChangeSet changedState2 = changedState.GetChangedState();
                if ((changedState2.HasMemberChange("Address1") || changedState2.HasMemberChange("Address2") || changedState2.HasMemberChange("City") || changedState2.HasMemberChange("State") || changedState2.HasMemberChange("PostalCode") || changedState2.HasMemberChange("Country") || changedState2.HasMemberChange("CountryCode")) && !changedState2.HasMemberChange("GeocodeFailed"))
                {
                    if (address.GeocodeFailed != null && address.GeocodeFailed.Value)
                    {
                        address.GeocodeFailed = new bool?(false);
                    }
                    GeocodeAddress(address);
                }
            }
            return true;
        }

        internal static bool GeocodeAddress(IAddress address)
        {
            try
            {
                if (address.GeocodeFailed != null && address.GeocodeFailed.Value)
                {
                    return false;
                }
                IGeocodeProvider provider = Saleslogix.Geocode.Managers.ProviderManager.GetProvider(null, null);
                if (provider != null)
                {
                    return provider.GeocodeAddress(address);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static bool OnBeforeInsert(IAddress address, ISession session)
        {
            if (string.IsNullOrEmpty(address.EntityId))
            {
                throw new ValidationException();
            }
            if (string.IsNullOrEmpty(address.Description) && !address.EntityId.StartsWith("U", StringComparison.Ordinal) && !address.EntityId.Trim().Equals("ADMIN"))
            {
                throw new ValidationException();
            }
            Rules.HandleAddressFlags(address, session);
            Rules.UpdateRelatedEntity(address, session);
            return true;
        }

        private static void UpdateRelatedEntity(IAddress address, ISession session)
        {
            if (address != null && !string.IsNullOrEmpty(address.EntityId))
            {
                if (address.EntityId.StartsWith("A", StringComparison.Ordinal))
                {
                    // обновить контакты
                    CommonProcedures.UpdateAccountContacts(address.EntityId, session);
                    return;
                }
                if (address.EntityId.StartsWith("C", StringComparison.Ordinal))
                {
                    // обновить контакты
                    CommonProcedures.UpdateAccountContacts(address.EntityId, session);
                }
            }
        }

        private static void HandleAddressFlags(IAddress address, ISession session)
        {
            if (address.IsMailing == null)
            {
                address.IsMailing = new bool?(false);
            }
            if (address.IsPrimary == null)
            {
                address.IsPrimary = new bool?(false);
            }
            if (address.PrimaryAddress == null)
            {
                address.PrimaryAddress = new bool?(false);
            }
            IList<IAddress> list = null;
            if (address.IsMailing == true)
            {
                if (string.IsNullOrEmpty(address.AddressType))
                {
                    address.AddressType = BusinessRuleEnums.PostalAddressType.Shipping.ToString();
                    if (address.IsPrimary == true)
                    {
                        address.AddressType = "Billing & Shipping";
                    }
                }
                list = Rules.GetEntityAddresses(session, address.EntityId);
                foreach (IAddress address2 in list)
                {
                    if (address2.IsMailing == true && address2.Id.ToString() != address.Id.ToString())
                    {
                        address2.IsMailing = new bool?(false);
                    }
                }
            }
            if (address.IsPrimary == true)
            {
                if (string.IsNullOrEmpty(address.AddressType))
                {
                    address.AddressType = BusinessRuleEnums.PostalAddressType.Billing.ToString();
                    if (address.IsMailing == true)
                    {
                        address.AddressType = "Billing & Shipping";
                    }
                }
                if (list == null)
                {
                    list = Rules.GetEntityAddresses(session, address.EntityId);
                }
                foreach (IAddress address3 in list)
                {
                    if (address3.IsPrimary == true && address3.Id.ToString() != address.Id.ToString())
                    {
                        address3.IsPrimary = new bool?(false);
                    }
                }
            }
            if (address.PrimaryAddress == true)
            {
                if (list == null)
                {
                    list = Rules.GetEntityAddresses(session, address.EntityId);
                }
                foreach (IAddress address4 in list)
                {
                    if (address4.PrimaryAddress == true && address4.Id.ToString() != address.Id.ToString())
                    {
                        address4.PrimaryAddress = new bool?(false);
                    }
                }
            }
        }

        public static void OnBeforeDelete(IAddress address, ISession session)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Update SalesOrderAddress Set Address = :nullAddress ").Append("Where Address.Id = :addressId");
                IQuery query = session.CreateQuery(stringBuilder.ToString());
                query.SetParameter("nullAddress", null);
                query.SetParameter("addressId", address.Id);
                query.ExecuteUpdate();
            }
            catch (Exception ex)
            {
                Rules.Log.Error(ex.Message, ex);
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static object ProviderManager { get; private set; }

        private static IList<IAddress> GetEntityAddresses(ISession session, string entityId)
        {
            return (from address in session.QueryOver<IAddress>()
                    where address.EntityId == entityId
                    select address).List();
        }

        public static void ValidateCountryCode(IAddress address)
        {
            address.CountryCode = Helpers.GetCountryCodeFromCountryName(address, "Country", "CountryCode");
        }

        private static void EnsureAddressPrimary(IAddress address, ISession session)
        {
            if (address.IsPrimary == true)
            {
                if (address.EntityType == "ACCOUNTASSISTANT")
                {
                    IACCOUNTASSISTANT assistant = EntityFactory.GetById("ACCOUNTASSISTANT", address.EntityId) as IACCOUNTASSISTANT;
                    if (assistant != null)
                    {
                        var addressesQuery = session.CreateSQLQuery("select ADDRESSID from ADDRESS where ENTITYID = :assistantId")
                            .SetString("assistantId", address.EntityId)
                            .List<string>();
                        foreach (var addressId in addressesQuery)
                        {
                            var assistantAddress = EntityFactory.GetById("ADDRESS", addressId) as IAddress;
                            if (assistantAddress.EntityId != address.EntityId)
                            {
                                assistantAddress.IsPrimary = false;
                                assistantAddress.Save();
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
