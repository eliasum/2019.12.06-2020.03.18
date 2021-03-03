using NHibernate;
using Sage.Entity.Interfaces;
using Sage.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbConsult.Web.BusinessRules
{
    public static class CommonProcedures
    {
        // метод обновления контактов
        public static void UpdateAccountContacts(string id, ISession session)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return;
            }

            // запрос к БД
            var procedure = session.Connection.CreateCommand();                     // создание запроса к базе данных, который необходимо выполнить
            procedure.Connection = session.Connection;                              // соединение
            procedure.CommandText = "FB_ACCOUNT_UPDATECONTACTS";                    // собственно команда 
            procedure.CommandType = System.Data.CommandType.StoredProcedure;        // тип команды - хранимая процедура FB_ACCOUNT_UPDATECONTACTS

            // входной параметр
            var accountid = procedure.CreateParameter();                            // создание входного параметра для хранимой процедуры FB_ACCOUNT_UPDATECONTACTS
            accountid.DbType = System.Data.DbType.String;                           // тип данных поля - string
            accountid.ParameterName = "accountid";                                  // имя параметра - accountid
            accountid.Value = id;                                                   // значение - id
            accountid.Direction = System.Data.ParameterDirection.Input;             // тип параметра - входной

            procedure.Parameters.Add(accountid);                                    // добавить в коллекцию параметров
            procedure.ExecuteScalar();                                              // выполнить запрос

            var accExt = EntityFactory.GetById("AccountExt", id) as IAccountExt;    // получить id сущности AccountExt с помощью EntityFactory и привести к IAccountExt
            session.Refresh(accExt);
        }
    }
}
