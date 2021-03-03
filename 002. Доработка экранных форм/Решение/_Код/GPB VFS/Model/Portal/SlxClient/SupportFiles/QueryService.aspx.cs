using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FbConsult.Web.Extensions.NHibernate;
using Newtonsoft.Json;
using Sage.Entity.Interfaces;
using Sage.Platform.Application;
using Sage.Platform.Orm;
using Sage.Platform.Orm.Attributes;
using Sage.Platform.Security;
using Sage.SalesLogix.Client.GroupBuilder;
using Sage.SalesLogix.PickLists;
using IntCommonLib.Utils;
using IntTaskGenerator;
using IntCommonLib;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using IntTaskProvider;
using IntCommonLib.Log;

public partial class QueryService : System.Web.UI.Page {
    protected void Page_Load (object sender, EventArgs e) {
        string method = Request.Headers["method"];
        switch (method) {
            case "deleteActivities":
                DeleteActivities ();
                return;
            case "deleteOfferActivities":
                DeleteActivities ("Fb_SasMaOfferAccountId");
                return;
            case "picklist":
                GetPicklist ();
                return;
            case "historyService":
                GetHistoryServiceInfo ();
                return;
           /* case "deleteCompletedSteps":
                DeleteCompletedSteps ();
                return;*/
            case "clearCurrentGroupCache":
                ClearCurrentGroupCache ();
                return;
            case "createAuditRecord":
                CreateAuditRecord ();
                return;
            /*case "CompaniesGroupChanged":
                CompaniesGroupChanged ();
                return;*/
            /*case "StatusChanged":
                StatusChanged ();
                return;
            case "DivisionChanged":
                DivisionChanged ();
                return;*/
            case "urlEncode1251":
                URLEncode1251 ();
                return;
           /* case "ClientManagerChanged":
                ClientManagerChanged ();
                return;
            case "SupportManagerChanged":
                SupportManagerChanged ();
                return;
            case "ValidateInnOkpo":
                ValidateInnOkpo ();
                return;
            case "PrognozGreenDataChanged":
                PrognozGreenDataChanged ();
                return;
            case "RegionChanged":
                RegionChanged ();
                return;
            case "StateRegionPrimeChanged":
                StateRegionPrimeChanged ();
                return;
            case "ValidateWorkerRole":
                ValidateWorkerRole ();
                return;*/
            case "MultiSelect":
                MultiSelect ();
                return;
            /*case "FbLogger":
                FbLogger ();
                return;*/
           /* case "AccountAddRevenue":
                AccountAddRevenue ();
                return;*/
                            case "GetGroupInfo": GetGroupInfo(); return;
            case "GetGroupNonce": GetGroupNonce(); return;
            case "RemoveFromFavorites": RemoveFromFavorites(); return; 
            case "BuildGCS": BuildGCS(); return;
			case "AddPicklistItem":
				AddPicklistItem();
				return;
            case "CreateUserOptionDef":
                CreateUserOptionDef();
                return;
            case "RunPackage":
                RunPackage();
                return;
                
            default:
                StartSelectQuery ();
                return;
        }
    }

    private void CreateAuditRecord () {
        Response.Clear ();
        Response.ContentType = "application/json";
        try {
            var entity = DefaultEncoding (Request.Headers["entity"]);
            var entityId = DefaultEncoding (Request.Headers["entityId"]);
            var fieldName = DefaultEncoding (Request.Headers["fieldName"]);
            var displayName = DefaultEncoding (Request.Headers["displayName"]);
            var oldValue = DefaultEncoding (Request.Headers["oldValue"]);
            var newValue = DefaultEncoding (Request.Headers["newValue"]);
            var isId = DefaultEncoding (Request.Headers["isId"]);
            var description = Request.Headers["description"] == null ? null : DefaultEncoding (Request.Headers["description"]);
            var userId = DefaultEncoding (Request.Headers["userId"]);
            using (var session = new SessionScopeWrapper ()) {
                session.CreateSQLQuery (@"INSERT INTO FB_AUDIT (
                                AUDITTYPE, CHANGEDATE, DESCRIPTION, 
                                DISPLAYNAME, ENTITY, ENTITYID, 
                                FIELDNAME, GROUPID, 
                                NEWVALUE, OLDVALUE, OPERATION, 
                                OSUSER, USERID) 
                                VALUES ( 0, :now, :description, :displayName, :entity, :entityId, :fieldName ,:groupId,  :newValue, :oldValue, 'U', 'WebDLL', :userId)")
                    .SetDateTime ("now", DateTime.UtcNow)
                    .SetString ("description", description)
                    .SetString ("displayName", displayName)
                    .SetString ("entity", entity)
                    .SetString ("fieldName", fieldName)
                    .SetString ("entityId", entityId)
                    .SetInt32 ("groupId", -1)
                    .SetString ("newValue", newValue)
                    .SetString ("oldValue", oldValue)
                    .SetString ("userId", userId)
                    .ExecuteUpdate ();
            }
            DefaultResponse (200, "");
        } catch (Exception e) {
            ExceptionResponse (e);
        }
    }

    private string DefaultEncoding (string str) {
        //Decoding from btoa(encodeURIComponent(str))
        return HttpUtility.UrlDecode (Encoding.UTF8.GetString (Convert.FromBase64String (str)));
    }

    private void DefaultResponse (int statusCode, string message) {
        Response.StatusCode = statusCode;
        var result = Encoding.UTF8.GetBytes (message);
        Response.OutputStream.Write (result, 0, result.Length);
        HttpContext.Current.Response.Flush ();
        HttpContext.Current.Response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest ();
    }

    private void ExceptionResponse (Exception exception, string subMessage = "—", int statusCode = 400) {
        Response.StatusCode = statusCode;
        var result = Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (subMessage + " " + exception));
        Response.OutputStream.Write (result, 0, result.Length);
        HttpContext.Current.Response.Flush ();
        HttpContext.Current.Response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest ();
    }

    protected void ClearCurrentGroupCache () {
        Response.Clear ();
        Response.ContentType = "application/json";
        (ApplicationContext.Current.Services.Get<IGroupContextService> () as GroupContextService)?.GetGroupContext ()?.CurrentGroupInfo?.ClearCache ();
        Response.StatusCode = 200;
        HttpContext.Current.Response.Flush ();
        HttpContext.Current.Response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest ();
    }

    private void GetHistoryServiceInfo () {
        Response.Clear ();
        Response.ContentType = "application/json";
        IEntityHistoryService entityHistoryService = ApplicationContext.Current.Services.Get<IEntityHistoryService> ();
        if (entityHistoryService == null) {
            Response.StatusCode = 400;
            var result = Encoding.UTF8.GetBytes ("Cannot init EntityHistoryService");
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
            return;
        }

        {
            Response.StatusCode = 200;
            var result = Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (entityHistoryService));
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
        }

    }

    private void GetPicklist () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string payload = Request.Headers["payload"];
        payload = HttpUtility.UrlDecode (Encoding.UTF8.GetString (Convert.FromBase64String (payload)));
        if (string.IsNullOrEmpty (payload)) {
            Response.StatusCode = 400;
            var result = Encoding.UTF8.GetBytes ("Uncorrect payload");
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
            return;
        }
        try {
            using (var session = new SessionScopeWrapper ()) {
                var picklistProperties = new [] { "PickListId", "Text", "ShortText", "UserId", "DefaultIndex", "Language", "Filter", "DefaultCode" };
                var picklistItemProperties = new [] { "ItemId", "Text", "ShortText" };
                var list = session.CreateSQLQuery ("SELECT ITEMID as PickListId, TEXT, SHORTTEXT, USERID, DEFAULTINDEX, LANGUAGECODE, FILTER, DEFAULTCODE FROM PICKLIST WHERE TEXT = :text AND PickListId = 'PICKLISTLIST' and rownum<2")
                    .SetString ("text", payload)
                    .List<object[]> ();

                if (!list.Any ()) throw new Exception ("Picklist not found");

                var picklist = list.Select (x => picklistProperties.Select ((y, i) => new KeyValuePair<string, int> (y, i))
                    .Aggregate (new ExpandoObject () as IDictionary<string, object>,
                        (a, p) => { a.Add (p.Key, x[p.Value]); return a; })).FirstOrDefault ();

                var picklistItems = session.CreateSQLQuery ("select itemid, text, shorttext  from PICKLIST where PickListId = :id")
                    // ReSharper disable once PossibleNullReferenceException была проверка выше
                    .SetString ("id", Convert.ToString (picklist["PickListId"]))
                    .List<object[]> ();

                picklist["items"] = picklistItems.Select (x => picklistItemProperties.Select ((y, i) => new KeyValuePair<string, int> (y, i))
                    .Aggregate (new ExpandoObject () as IDictionary<string, object>,
                        (a, p) => { a.Add (p.Key, x[p.Value]); return a; }));
                Response.StatusCode = 200;
                var result = Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (picklist));
                Response.OutputStream.Write (result, 0, result.Length);
                HttpContext.Current.Response.Flush ();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest ();
            }
        } catch (Exception exception) {
            Response.StatusCode = 400;
            var result = Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (exception));
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
        }
    }

	private void AddPicklistItem()
	{
		Response.Clear();
		Response.ContentType = "application/json";

		string text = DefaultEncoding(Request.Headers["Text"]);
		string shortText = DefaultEncoding(Request.Headers["ShortText"]);
		string picklistId = DefaultEncoding(Request.Headers["PickListId"]);
		string LanguageCode = DefaultEncoding(Request.Headers["LanguageCode"]);
		if (string.IsNullOrEmpty(text) ||
			string.IsNullOrEmpty(shortText) ||
			string.IsNullOrEmpty(picklistId) ||
			string.IsNullOrEmpty(LanguageCode))
		{
			DefaultResponse(400, "Incorrect Payload");
			return;
		}
		try
		{
			using (var session = new SessionScopeWrapper())
			{
				var items = PickList.GetPickListItems(picklistId);
				PickList.AddNewPickListItem(picklistId, text, shortText, items.Count, ApplicationContext.Current.Services.Get<IUserService>(true).UserId);
				DefaultResponse(200, "");
			}
		}
		catch (ArgumentException)
		{
			DefaultResponse(200, "");
		}
		catch (Exception exception)
		{
			Response.StatusCode = 400;
			var result = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(exception));
			Response.OutputStream.Write(result, 0, result.Length);
			HttpContext.Current.Response.Flush();
			HttpContext.Current.Response.SuppressContent = true;
			HttpContext.Current.ApplicationInstance.CompleteRequest();
		}
	}


	private void DeleteActivities (string idField = "activityid") {
        Response.Clear ();
        Response.ContentType = "application/json";
        string payload = Request.Headers["payload"]?.Replace (" ", "");
        if (string.IsNullOrEmpty (payload) || payload.Length != 12) {
            Response.StatusCode = 400;
            var result = Encoding.UTF8.GetBytes ("Uncorrect payload");
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
            return;
        }

        //var ids = payload.Split(',').Select(x => x.Trim()).Select(x => x.Substring(0, 12)).ToList();
        using (var session = new SessionScopeWrapper ()) {
            session.CreateSQLQuery($"delete from USER_ACTIVITY where ACTIVITYID in (select ACTIVITYID from ACTIVITY where {idField} = :id)")
                .SetString("id", payload)
                .ExecuteUpdate();
            session.CreateSQLQuery($"delete from ACTIVITYATTENDEE where ACTIVITYID in (select ACTIVITYID from ACTIVITY where {idField} = :id)")
                .SetString("id", payload)
                .ExecuteUpdate();
            session.CreateSQLQuery($"delete from RESOURCESCHEDULE where ACTIVITYID in (select ACTIVITYID from ACTIVITY where {idField} = :id)")
                .SetString("id", payload)
                .ExecuteUpdate();
            session.CreateSQLQuery($"delete from USERNOTIFICATION where ACTIVITYID in (select ACTIVITYID from ACTIVITY where {idField} = :id) and Type != 'Deleted'")
                .SetString("id", payload)
                .ExecuteUpdate();
            session.CreateSQLQuery($"delete from FB_OPPORTUNITYACTIVITY where ACTIVITYID in (select ACTIVITYID from ACTIVITY where {idField} = :id)")
                .SetString("id", payload)
                .ExecuteUpdate();
            var resp = session.CreateSQLQuery($"delete from activity where {idField} = :id")
                .SetString("id", payload)
                .ExecuteUpdate();
            Response.StatusCode = 200;
            var result = Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (resp));
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
        }
    }

    /*private void DeleteCompletedSteps () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string payload = Request.Headers["payload"]?.Replace (" ", "");
        if (string.IsNullOrEmpty (payload) || payload.Length != 12) {
            Response.StatusCode = 400;
            var result = Encoding.UTF8.GetBytes ("Uncorrect payload");
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
            return;
        }

        using (var session = new SessionScopeWrapper ()) {
            foreach (IFbOpportunitySalesProcessLink processLink in session.QueryOver<IFbOpportunitySalesProcessLink> ().Where (x => x.OpportunityId == payload).List ()) {
                processLink.Delete ();
            }
            Response.StatusCode = 200;
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
        }
    }*/
    /*private void FbLogger () {
        Response.Clear ();
        Response.ContentType = "text/html";
        try {
            Logger.Log (
                DefaultEncoding (Request.Headers["type"]),
                DefaultEncoding (Request.Headers["entityId"]),
                DefaultEncoding (Request.Headers["entityType"]),
                DefaultEncoding (Request.Headers["description"]),
                DefaultEncoding (Request.Headers["isTab"]) == "true");
            DefaultResponse (200, "OK");
        } catch (Exception exception) {
            ExceptionResponse (exception);
        }
    }*/

    private void StartSelectQuery () {
        Response.Clear ();
        Response.ContentType = "text/html";
        try {
            DefaultResponse (200, JsonConvert.SerializeObject (Select (Request.Headers["query"], Request.Headers["properties"])));
        } catch (Exception exception) {
            ExceptionResponse (exception);
        }
    }

    private void MultiSelect () {
        Response.Clear ();
        Response.ContentType = "text/html";
        var responces = new ExpandoObject () as IDictionary<string, object>;
        foreach (var kvp in JsonConvert.DeserializeObject<List<Dictionary<string, string>>> (Request.Headers["queries"])) {
            try {
                responces[kvp["key"]] = Select (kvp["query"], kvp["properties"]);
            } catch (Exception exception) {
                responces[kvp["key"]] = exception;
            }

        }

        DefaultResponse (200, JsonConvert.SerializeObject (responces));

    }

    private object Select (string queryEncoded, string propertiesEncoded) {
        if (queryEncoded == null) throw new Exception ("No request found");

        var query = HttpUtility.UrlDecode (Encoding.UTF8.GetString (Convert.FromBase64String (queryEncoded)));
        query = query.Replace ("\\", "");
        if (query.ToLowerInvariant ().Contains ("insert ") || query.ToLowerInvariant ().Contains ("update ") || query.ToLowerInvariant ().Contains ("delete "))
            throw new Exception ("Invalid request");

        string[] properties = null;
        if (propertiesEncoded != null) {
            propertiesEncoded = HttpUtility.UrlDecode (Encoding.UTF8.GetString (Convert.FromBase64String (propertiesEncoded)));

            properties = propertiesEncoded.Split (',')
                .Select (x => x.Replace (" ", "")
                    .Replace ("'", "")
                    .Replace ("\"", "")
                    .Replace ("(", "")
                    .Replace (")", "")
                    .Replace ("{", "")
                    .Replace ("}", "")
                ).ToArray ();
        }
        using (var session = new SessionScopeWrapper ()) {
            if (properties == null || !properties.Any ()) return session.CreateSQLQuery (query).List ();

            //Если есть список полей вернем нормальный json вместо массива массивов
            //dynamic eo = properties.Aggregate(new ExpandoObject() as IDictionary<string, object>,
            //    (a, p) => { a.Add(p.Replace(" ", "").Replace("'", "").Replace("\"", "").Replace("(", "").Replace(")", "").Replace("{", "").Replace("}", ""), new object()); return a; });
            var list = properties.Length == 1 ?
                session.CreateSQLQuery (query).List<object> ().Select (x => new [] { x }).ToList () :
                session.CreateSQLQuery (query).List<object[]> ();
            return list.Select (x => properties.Select ((y, i) => new KeyValuePair<string, int> (y, i))
                .Aggregate (new ExpandoObject () as IDictionary<string, object>,
                    (a, p) => { a.Add (p.Key, x[p.Value]); return a; }));
        }
    }

    #region RequestProcessing
    void responseJson(string strJson)
    {
        // Отключение кэширования
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;
        Response.ContentType = "text/html";
        //-----------------------

        Response.Write(strJson);
    }
    #endregion
    void RunPackage()
    {
        Response.Clear();
        Response.ContentType = "application/json";
        var bodyStream = new System.IO.StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();

        var jss = new JavaScriptSerializer();
        var dict = jss.Deserialize<Dictionary<string, string>>(bodyText);

        NameValueCollection nvc = null;
        if (dict != null)
        {
            nvc = new NameValueCollection(dict.Count);
            foreach (var k in dict)
            {
                nvc.Add(k.Key, k.Value);
            }
        }


        string PackageId = nvc["PackageId"];
        string Source = nvc["Source"];
        if (String.IsNullOrEmpty(PackageId))
        {
            Response.StatusCode = 412;
            responseJson("{success:false, msg:'Не удалось запустить пакет на исполнение', errorMessage:'Не передан параметр PackageId'}");
        }
        else
        {
            // Read settings
            string settingsFileName = "IntRouterSettings.xml";
            string settingsFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settingsFileName);

            SettingsReader r = new SettingsReader();
            r.Load(settingsFilePath);

            string encryptedTaskDBConnStr = r.Settings["TaskDBConnStr"] as string;
            string encryptedBufferDBConnStr = r.Settings["BufferDBConnStr"] as string;

            // Get current user id
            IUserService us = ApplicationContext.Current.Services.Get<IUserService>();
            string UserId = us.UserId;
            string s1 = CryptHelper.Decrypt("nAAX+uwRc8HFi0XeKsdsr1TUVDbYj9E8SU3TrcI+8Lp+2qyrmykUl3n5M+Ck0i2Q36d2aCZYrjwWSxgBh6whxgrS5r0MItszDGVt/iNimcdziOjl5gc6ogiA9SMwQlaxeOIyi03E2ZnQf45iRrW5yw==");
            string s12 = CryptHelper.Decrypt("Ln8vg4b27WQrqa7LmGSfOSVPrFndSLfyB2XPN3fGQVkgb0Iz/e644S6SFsH9EGxNQFzK2DhmqkDh7Q+yG6gw0PeeGHW80p/U+rnMQZpka5rqo3rHqjQOIl3XEU9+g9IBq+VLAkOij2LLsPfiWTkjVr+Kd616rZlaO2MZc3Hoyj6ewUrMF4y+ZU+BQo3fuQDW");
            string s124 = CryptHelper.Decrypt("KaK7zo3V5YvO3p2Dx4bhXMxNgeL/wPhhsrJm9GMpCzaA9/AUwg99/cTSZhp8eS/S7y55LDgfLPO1PVui/ARoeZSE9DMogqCwZTQfeSzfdavxE3N12oCdIejjch30q+BHoCC6blOfphfR3yNRRQpCCjRJA9CumMnXzpSZj5qbnZu5k2NEUhN3jOlf+F2N3bqzT8tMpFMB42XWUHwGct7TO4hftsevrZU9Dm53HlhhEx5A5ahXKCK45gJs3EaCb/UeGMWE8ci75xtEY5NsDiDxKtiVpvNp8pRIsjD4ic9NVAAGy8LDh2TW1ZKkqv9E5YakuTMGXNf4fpOsGTgIuPMTCMi+BlzFP69QAcSfyow3BYY=");
            string s123 = CryptHelper.Encrypt("Provider=SLXOLEDB.1;Data Source=MYSLXSERVER;Initial Catalog=MYSLXCONNECTIONALIAS;User Id=Admin;Password=\"q1234567\";Persist Security Info=True; Extended Properties=\"Port = 1706; Log = On\"");
            string s1234 = CryptHelper.Encrypt("OLE DB Services = -1;Provider=OraOLEDB.Oracle.1;Persist Security Info=True;Password=masterkey;User ID=sysdba;Data Source=server_gbp_priv;");
            string s12345 = CryptHelper.Encrypt("Provider=OraOLEDB.Oracle.1;Persist Security Info=True;Password=masterkey;User ID=sysdba;Data Source=server_gbp_priv;");
            string s123456 = CryptHelper.Encrypt("Provider=OraOLEDB.Oracle.1;Password=masterkey;Persist Security Info=True;User ID=sysdba;Data Source=server_gpb_priv;");
            string s1234567 = CryptHelper.Decrypt(encryptedTaskDBConnStr);
            
            // Generate task
            TaskGenerator gen = new TaskGenerator(encryptedTaskDBConnStr, encryptedBufferDBConnStr);
            //gen.UseQuickGeneration = true;
            object resp = null;
            try
            {
                long PackageInstanceId = gen.RunPackage(PackageId, UserId, nvc);
                resp = new { success = true, msg = $"Пакет запущен на исполнение: [{PackageInstanceId.ToString()}]", msgError = "No Error" };
                //resp = $"Пакет запущен на исполнение: [{PackageInstanceId.ToString()}]";
                Response.StatusCode = 202;
            }
            catch(Exception ex)
            {
                resp = new { success = false, msg = $"Не удалось запустить на исполнение пакет по источнику: [{Source}]", msgError = ex.Message };
                //resp = $"Не удалось запустить на исполнение пакет: [{PackageId}]";
                Response.StatusCode = 500;
            }
            finally {
                responseJson(new JavaScriptSerializer().Serialize(resp));
                //try
                //{
                //    Logger logger = new Logger();
                //    logger.LogDelegate = new LogDelegate(null);
                //    logger.Log(
                //        LogLevel.INFO,
                //        resp);
                //}
                //catch (Exception exception)
                //{
                //    ExceptionResponse(exception);
                //}
            }

            
        }

        
    }
    private Dictionary<string, IntParam> PrepareParams(IntParam[] InputParams, IntParam[] DefaultParams, MapItem[] Mappings)
    {
        Dictionary<string, IntParam> dictionary = new Dictionary<string, IntParam>();
        string str = "PARAMS" + new string(MapItem.Separator);
        foreach (IntParam intParam in DefaultParams)
        {
            string key = str + intParam.ParamName;
            object paramValue = null;
            if (!ConvertHelper.TryConvert(intParam.ParamTypeName, intParam.ParamValue, out paramValue))
            {
                throw new Exception("Incorrect default value");
            }
            intParam.ParamValue = paramValue;
            dictionary.Add(key, intParam);
        }
        if (InputParams != null)
        {
            foreach (IntParam intParam2 in InputParams)
            {
                string key = str + intParam2.ParamName;
                object paramValue = null;
                if (!ConvertHelper.TryConvert(intParam2.ParamTypeName, intParam2.ParamValue, out paramValue))
                {
                    throw new Exception("Incorrect input value");
                }
                if (dictionary.ContainsKey(key))
                {
                    dictionary.Remove(key);
                }
                intParam2.ParamValue = paramValue;
                dictionary.Add(key, intParam2);
            }
        }
        MapItem[] array = (from p in Mappings
                           where p.Source.Split(MapItem.Separator, 2)[0] == "CALCS"
                           select p).ToArray<MapItem>();
        foreach (MapItem mapItem in array)
        {
            if (!dictionary.ContainsKey(mapItem.Source))
            {
                IntParam intParam3 = new IntParam();
                string calcName = mapItem.Source.Split(MapItem.Separator, 2)[1];
                object obj = null;
                if (!CalcValueHelper.TryGetCalcValue(calcName, out obj))
                {
                    throw new Exception("Incorrect calculation value");
                }
                intParam3.ParamName = mapItem.Source;
                intParam3.ParamValue = obj;
                if (obj != null)
                {
                    intParam3.ParamTypeName = obj.GetType().FullName;
                }
                dictionary.Add(mapItem.Source, intParam3);
            }
        }
        return dictionary;
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    /*private void CompaniesGroupChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["companiesGroupId"]), DefaultEncoding (Request.Headers["mode"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1]) || string.IsNullOrEmpty (headers[2])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            IFbCompaniesGroup tmpCompaniesGroup = Sage.Platform.EntityFactory.GetById<IFbCompaniesGroup> (headers[1]);
            if ((headers[2] == "change" && (tmpAccount == null || tmpCompaniesGroup == null)) || (headers[2] == "clear" && (tmpCompaniesGroup != null || tmpAccount == null))) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            using (var session = new SessionScopeWrapper ()) {

                if (headers[2] == "change") {
                    var companiesGroupAccount = session.CreateQuery ("from FbCompaniesGroupAccount Where EntityId = :accountId").SetString ("accountId", Convert.ToString (tmpAccount.Id)).List<IFbCompaniesGroupAccount> ().FirstOrDefault ();
                    if (companiesGroupAccount == null) companiesGroupAccount = Sage.Platform.EntityFactory.Create<IFbCompaniesGroupAccount> ();
                    companiesGroupAccount.FbCompaniesGroupId = Convert.ToString (tmpCompaniesGroup.Id);
                    companiesGroupAccount.Category = "Организация";
                    companiesGroupAccount.Status = "Не определено";
                    companiesGroupAccount.ManagerId = tmpAccount.ClientManagerId;
                    companiesGroupAccount.Division = tmpAccount.Division;
                    companiesGroupAccount.EntityId = Convert.ToString (tmpAccount.Id);
                    companiesGroupAccount.EntityName = tmpAccount.AccountName;
                    companiesGroupAccount.PersonCode = (int) tmpAccount.PersonCode;
                    companiesGroupAccount.Save ();
                } else if (headers[2] == "clear") {
                    var companiesGroupAccountToDelete = session.CreateQuery ("from FbCompaniesGroupAccount Where EntityId = :accountId").SetString ("accountId", Convert.ToString (tmpAccount.Id)).List<IFbCompaniesGroupAccount> ().FirstOrDefault ();
                    if (companiesGroupAccountToDelete != null) {
                        var currentUserId = ApplicationContext.Current.Services.Get<Sage.Platform.Security.IUserService> (true).UserId;
                        companiesGroupAccountToDelete.ModifyUser = currentUserId;
                        companiesGroupAccountToDelete.ModifyDate = DateTime.UtcNow;
                        companiesGroupAccountToDelete.Save ();
                        companiesGroupAccountToDelete.Delete ();
                    }
                }
            }
        } catch (Exception exception) {
            ExceptionResponse (exception, "CompaniesGroupChanged");
        }
    }*/

    /*private void StatusChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["newStatus"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            if (tmpAccount == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }

            string oldStatus = tmpAccount.Status;
            tmpAccount.Status = headers[1];
            if (tmpAccount.Status != "Нецелевой" && tmpAccount.Status != "Неперспективный") {
                using (var session = new SessionScopeWrapper ()) {
                    var shouldProcessChecks = session.CreateSQLQuery ("Select PROPERTYDATA From FB_SYSSETTINGS Where MODULENAME = 'AccountStatusSettings' And PROPERTYNAME = 'SelectedStatus' And TO_CHAR(PROPERTYDATA) = :status")
                        .SetString ("status", tmpAccount.Status).List ().Count > 0;
                    if (shouldProcessChecks) {
                        var shouldProcess = true;
                        var conditionSql = session.CreateSQLQuery ("Select PROPERTYDATA From FB_SYSSETTINGS Where MODULENAME = 'AccountStatusSettings' And PROPERTYNAME = 'Condition'").List<string> ().FirstOrDefault ();

                        if (!string.IsNullOrWhiteSpace (conditionSql)) {
                            var parameterIndex = conditionSql.IndexOf (":accountid", StringComparison.OrdinalIgnoreCase);
                            shouldProcess = parameterIndex >= 0 && session.CreateSQLQuery (conditionSql).SetString (conditionSql.Substring (parameterIndex + 1, 9), Convert.ToString (tmpAccount.Id)).List ().Count > 0;
                        }
                        if (shouldProcess) {
                            var shouldDeleteDivision = session.CreateSQLQuery ("Select PROPERTYDATA From FB_SYSSETTINGS Where MODULENAME = 'AccountStatusSettings' And PROPERTYNAME = 'DeleteManager'").List ().Count > 0;
                            var shouldDeleteManagers = session.CreateSQLQuery ("Select PROPERTYDATA From FB_SYSSETTINGS Where MODULENAME = 'AccountStatusSettings' And PROPERTYNAME = 'DeleteSeniorManager'").List ().Count > 0;
                            if (shouldDeleteDivision) FbConsult.Web.BusinessRules.Account.Methods.ChangeDivision (tmpAccount, null, true, true);
                            else if (shouldDeleteManagers) {
                                FbConsult.Web.BusinessRules.Account.Methods.ChangeClientManager (tmpAccount, null, true, true);
                                FbConsult.Web.BusinessRules.Account.Methods.ChangeSupportManager (tmpAccount, null);
                            }
                        }
                    }
                }
                tmpAccount.StatusReason = tmpAccount.StatusNotes = null;
                tmpAccount.Save ();
                using (var session = new SessionScopeWrapper ()) {
                    session.Refresh (tmpAccount);
                }
                DefaultResponse (200, "correct");
                return;
            }

            tmpAccount.Status = Convert.ToString (oldStatus);
            DefaultResponse (200, "dialog");
        } catch (Exception exception) {
            ExceptionResponse (exception, "StatusChanged");
        }
    }*/
    /*private void DivisionChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["divisionId"]), DefaultEncoding (Request.Headers["mode"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1]) || string.IsNullOrEmpty (headers[2])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            IFbDivision tmpDivision = Sage.Platform.EntityFactory.GetById<IFbDivision> (headers[1]);
            if ((headers[2] == "change" && (tmpAccount == null || tmpDivision == null)) || (headers[2] == "clear" && (tmpDivision != null || tmpAccount == null))) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            if (headers[2] == "change") FbConsult.Web.BusinessRules.Account.Methods.ChangeDivision (tmpAccount, tmpDivision.FbDivisionId, true, true);
            else if (headers[2] == "clear") FbConsult.Web.BusinessRules.Account.Methods.ChangeDivision (tmpAccount, null, true, true);

            tmpAccount.Save ();
            using (var session = new SessionScopeWrapper ()) {
                session.Refresh (tmpAccount);
            }
            DefaultResponse (200, "correct");
        } catch (Exception exception) {
            ExceptionResponse (exception, "DivisionChanged");
        }
    }*/
    private void URLEncode1251 () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string payload = DefaultEncoding (Request.Headers["payload"]);
        if (string.IsNullOrEmpty (payload)) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            Response.StatusCode = 200;
            var result = Encoding.UTF8.GetBytes (HttpUtility.UrlEncode (payload, Encoding.GetEncoding (1251)));
            Response.OutputStream.Write (result, 0, result.Length);
            HttpContext.Current.Response.Flush ();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest ();
        } catch (Exception exception) {
            ExceptionResponse (exception, "URLEncode1251");
        }
    }
    /*private void ClientManagerChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["workerRoleId"]), DefaultEncoding (Request.Headers["mode"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1]) || string.IsNullOrEmpty (headers[2])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            IFbWorkerDivisionRole tmpWorker = Sage.Platform.EntityFactory.GetById<IFbWorkerDivisionRole> (headers[1]);
            if ((headers[2] == "change" && (tmpAccount == null || tmpWorker == null || tmpWorker.Worker == null)) || (headers[2] == "clear" && (tmpWorker != null || tmpAccount == null))) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            if (headers[2] == "change") FbConsult.Web.BusinessRules.Account.Methods.ChangeClientManagerAndDivision (tmpAccount, tmpWorker.FbWorkerId, tmpWorker.FbDivisionId);
            else if (headers[2] == "clear") FbConsult.Web.BusinessRules.Account.Methods.ChangeClientManager (tmpAccount, null, false, false);

            tmpAccount.Save ();
            using (var session = new SessionScopeWrapper ()) {
                session.Refresh (tmpAccount);
            }
            DefaultResponse (200, "correct");
        } catch (Exception exception) {
            ExceptionResponse (exception, "ClientManagerChanged");
        }
    }*/
    /*private void SupportManagerChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["workerRoleId"]), DefaultEncoding (Request.Headers["mode"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1]) || string.IsNullOrEmpty (headers[2])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            IFbWorkerDivisionRole tmpWorker = Sage.Platform.EntityFactory.GetById<IFbWorkerDivisionRole> (headers[1]);
            if ((headers[2] == "change" && (tmpAccount == null || tmpWorker == null || tmpWorker.Worker == null)) || (headers[2] == "clear" && (tmpWorker != null || tmpAccount == null))) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            if (headers[2] == "change") FbConsult.Web.BusinessRules.Account.Methods.ChangeSupportManager (tmpAccount, tmpWorker.FbWorkerId);
            else if (headers[2] == "clear") FbConsult.Web.BusinessRules.Account.Methods.ChangeSupportManager (tmpAccount, null);

            tmpAccount.Save ();
            using (var session = new SessionScopeWrapper ()) {
                session.Refresh (tmpAccount);
            }
            DefaultResponse (200, "correct");
        } catch (Exception exception) {
            ExceptionResponse (exception, "SupportManagerChanged");
        }
    }*/
    /*private void ValidateInnOkpo () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["code"]), DefaultEncoding (Request.Headers["mode"]) };
        foreach (var item in headers)
            if (string.IsNullOrEmpty (item)) {
                DefaultResponse (400, "Uncorrect headers");
                return;
            }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            if (tmpAccount == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            if (headers[2] == "inn") {
                using (var session = new SessionScopeWrapper ()) {
                    var accountsCount = Convert.ToInt64 (session.CreateSQLQuery ($"Select Count(1) From UNIQ_ACCOUNT Where INN = '{headers[1]}' And ACCOUNTID != '{headers[0]}'").UniqueResult ());

                    if (accountsCount > 0) {
                        DefaultResponse (406, $"Субъект с ИНН {headers[1]} уже существует. <br /> Вы хотите ввести ИНН?");
                        return;
                    }
                    DefaultResponse (200, "correct");
                    return;
                }
            } else if (headers[2] == "okpo") {
                using (var session = new SessionScopeWrapper ()) {
                    var accountsCount = Convert.ToInt64 (session.CreateSQLQuery ($"Select Count(1) From UNIQ_ACCOUNT Where TOLLFREE2 = '{headers[1]}' And BANK = '{tmpAccount.Bank}' And ACCOUNTID != '{headers[0]}'").UniqueResult ());
                    if (accountsCount < 1) {
                        DefaultResponse (200, "correct");
                        return;
                    }

                    DefaultResponse (406, "Организация с таким ОКПО уже существует.");
                    var accountChangedState = (Sage.Platform.ChangeManagement.IChangedState) tmpAccount;
                    var changedState = accountChangedState.GetChangedState ();
                    if (changedState == null) return;
                    var okpoChange = changedState.FindPropertyChange ("TollFree2");
                    if (okpoChange == null) return;
                    tmpAccount.TollFree2 = Convert.ToString (okpoChange.OldValue);
                }
            }
        } catch (Exception exception) {
            ExceptionResponse (exception, "SupportManagerChanged");
        }
    }*/
    /*private void RegionChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["regionId"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            IFbRegion tmpRegion = Sage.Platform.EntityFactory.GetById<IFbRegion> (headers[1]);
            if (tmpAccount == null || tmpRegion == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            FbConsult.Web.BusinessRules.Account.Methods.ChangeRegion (tmpAccount, Convert.ToString (tmpRegion.Id), true, true);
            tmpAccount.Save ();
            using (var session = new SessionScopeWrapper ()) {
                session.Refresh (tmpAccount);
            }
            DefaultResponse (200, "correct");
        } catch (Exception exception) {
            ExceptionResponse (exception, "RegionChanged");
        }
    }*/
    /*private void StateRegionPrimeChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["entity"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            if (tmpAccount == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            using (var session = new SessionScopeWrapper ()) {
                var settings = session.CreateSQLQuery ("Select PropertyName, PropertyData From Fb_SysSettings Where PropertyName in ('CompareActionType', 'CompareActionMaxWaitTime', 'CompareGroupComparePriority', 'CompareActionRefreshCache', 'WebServiceAddress') and ModuleName = 'PRIMEIntegrationSettings'")
                    .ListAs (new { PropertyName = default (string), PropertyData = default (string) });

                var actionType = settings.Where (x => x.PropertyName == "CompareActionType").Select (x => x.PropertyData).FirstOrDefault ();
                var primeUrl = settings.Where (x => x.PropertyName == "WebServiceAddress").Select (x => x.PropertyData).FirstOrDefault ();
                var maxTime = settings.Where (x => x.PropertyName == "CompareActionMaxWaitTime").Select (x => x.PropertyData).FirstOrDefault ();
                var subjectId = string.Empty;
                var isCorporate = string.IsNullOrWhiteSpace (tmpAccount.Inn) ? (string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? true : tmpAccount.TollFree2.Length == 8) : tmpAccount.Inn.Length == 10;

                if (string.IsNullOrWhiteSpace (tmpAccount.Inn) && string.IsNullOrWhiteSpace (tmpAccount.TollFree2)) {
                    DefaultResponse (209, "Запрос в Прайм невозможен, требуется указать ИНН или ОКПО организации.");
                    return;
                }
                if (string.IsNullOrWhiteSpace (tmpAccount.TollFree2)) {
                    subjectId = GetPrimeAccountIdFromCache (tmpAccount); //Если ОКПО не заполнен, то пытаемя искать в кэше
                    if (string.IsNullOrWhiteSpace (subjectId)) {
                        DefaultResponse (209, "Запрос в ПРАЙМ невозможен, требуется указать ИНН и ОКПО организации.");
                        return;
                    }
                }
                if (string.IsNullOrEmpty (subjectId)) {
                    var parameters = new List<KeyValuePair<string, string>> ();
                    if (!string.IsNullOrWhiteSpace (tmpAccount.Inn)) parameters.Add (new KeyValuePair<string, string> (isCorporate ? "INN" : "CodesINN", tmpAccount.Inn));
                    if (!string.IsNullOrWhiteSpace (tmpAccount.TollFree2)) parameters.Add (new KeyValuePair<string, string> (isCorporate ? "OKPO" : "CodesOKPO", tmpAccount.TollFree2));
                    var fullPrimeUrl = (primeUrl.EndsWith ("/") ? primeUrl : primeUrl + "/") + string.Format ("PrimeServiceRetranslator/GetRecordSet?RequestId={0}&serviceMethod=", CreatePrimeRequestRecord (isCorporate ? "ACCOUNTSHORT" : "ACCOUNTIP", parameters, true, 0, tmpAccount)) + (isCorporate ? string.Format ("GetListCompaniesRegData?WithListINN=true%26OnlyActive=false{0}", string.Join ("", parameters.Select (x => string.Format ("%26{0}={1}", x.Key, x.Value)))) : string.Format ("GetListIPRegDataByCodesOrParameters?{0}", string.Join ("", parameters.Select (x => string.Format ("%26{0}={1}", x.Key, x.Value)))));
                    var result = GetDataFromPrime (fullPrimeUrl);
                    var record = result != null ? result.Descendants ("Record") : null;
                    if (record != null && record.Count () == 1)
                        subjectId = GetChildElementValue<string> (record.FirstOrDefault (), (isCorporate ? "SubjectId" : "IPSubjectId"));
                    else if (record != null && record.Count () == 0) {
                        DefaultResponse (209, "Организация не найдена в ПРАЙМ");
                        return;
                    } else if (record != null && record.Count () > 1) {
                        DefaultResponse (209, "В ПРАЙМ найдено больше одной организации с такими ИНН и ОКПО");
                        return;
                    }
                }
                /*
                if (actionType == "0")
                {
                    //if (string.IsNullOrWhiteSpace(subjectId)) return;
                    //var parameters = new List<KeyValuePair<string, string>>();
                    //parameters.Add(new KeyValuePair<string, string>((isCorporate ? "SubjectId" : "CodesID"), subjectId));
                    //if (isCorporate) parameters.Add(new KeyValuePair<string, string>("WithListINN", "true"));
                    //var requestId = CreatePrimeRequestRecord((isCorporate ? "ACCOUNT" : "ACCOUNTIP"), parameters, false, 1);
                    //ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_CheckUpdate", $"FB.Other.FbAccountCheckPrimeUpdate.CheckUpdate('{Convert.ToString(tmpAccount.Id)}','{requestId}',{maxTime}, true);", true);
                }
                */
                /*if (actionType == "1" || actionType == "0") {
                    if (string.IsNullOrWhiteSpace (subjectId))
                        return;
                    var parameters = new List<KeyValuePair<string, string>> ();
                    parameters.Add (new KeyValuePair<string, string> ((isCorporate ? "CodesID" : "SubjectId"), subjectId));
                    var parameterFinInfoYears = new List<KeyValuePair<string, string>> ();
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("SubjectId", subjectId));
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("Params", "129525"));
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("YearFrom", "2000"));
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("YearTo", Convert.ToString (DateTime.Now.Year - 1)));
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("ParamsForView", "129525"));
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("YearBegin", "2000"));
                    parameterFinInfoYears.Add (new KeyValuePair<string, string> ("YearEnd", Convert.ToString (DateTime.Now.Year - 1)));

                    if (isCorporate) parameters.Add (new KeyValuePair<string, string> ("WithListINN", "true"));
                    if (settings.Where (x => x.PropertyName == "CompareActionRefreshCache").Select (x => x.PropertyData).FirstOrDefault () == "T")
                        CreatePrimeRequestRecord ((isCorporate ? "ACCOUNT" : "ACCOUNTIP"), parameters, false, 1, tmpAccount);
                    var primeAccountUrl = (primeUrl.EndsWith ("/") ? primeUrl : primeUrl + "/") + string.Format ("PrimeServiceRetranslator/GetRecordSet?RequestId={0}&serviceMethod=", CreatePrimeRequestRecord (isCorporate ? "ACCOUNT" : "ACCOUNTIP", new List<KeyValuePair<string, string>> () { new KeyValuePair<string, string> (isCorporate ? "SubjectId" : "CodesID", subjectId) }, true, 0, tmpAccount)) + string.Format (isCorporate ? "GetCompanyRegData?SubjectId={0}%26WithListINN=true" : "GetListIPRegDataByCodesOrParameters?CodesID={0}", subjectId);
                    var result = GetDataFromPrime (primeAccountUrl);

                    var primeFinInfoYearUrl = (primeUrl.EndsWith ("/") ? primeUrl : primeUrl + "/") + string.Format ("PrimeServiceRetranslator/GetRecordSet?RequestId={0}&serviceMethod=", CreatePrimeRequestRecord ("FININFOYEARLIST", parameterFinInfoYears, true, 0, tmpAccount)) + "GetCompaniesWithParamsByParameters?" + string.Join ("", parameterFinInfoYears.Select (x => string.Format ("%26{0}={1}", x.Key, x.Value)));
                    var primeFinInfoYear = GetDataFromPrime (primeFinInfoYearUrl);
                    UpdateRevenue (primeFinInfoYear, tmpAccount);
                    UpdateEmptyFields (result, tmpAccount);
                }
            }
        } catch (Exception exception) {
            ExceptionResponse (exception, "StateRegionPrimeChanged");
        }
    }*/
    #region Auxiliary elements for work StateRegionPrimeChanged
    /*private string GetPrimeAccountIdFromCache (IAccount tmpAccount) {
        var primeAccountId = string.Empty;
        var isCorporate = string.IsNullOrWhiteSpace (tmpAccount.Inn) ? (string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? true : tmpAccount.TollFree2.Length == 8) : tmpAccount.Inn.Length == 10;
        string where = String.Empty;
        if (tmpAccount.Inn.Length == 10)
            where = string.IsNullOrWhiteSpace (tmpAccount.Inn) ? (!string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? "Where INN Is NUll And OKPO='" + tmpAccount.TollFree2 + "'" : "") : (!string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? "Where (INN = '" + tmpAccount.Inn + "' Or HISTORYINNLIST Like'%" + tmpAccount.Inn + "%') And OKPO='" + tmpAccount.TollFree2 + "'" : "Where (INN = '" + tmpAccount.Inn + "' Or HISTORYINNLIST Like'%" + tmpAccount.Inn + "%') And OKPO Is Null");
        else if (tmpAccount.Inn.Length == 12)
            where = string.IsNullOrWhiteSpace (tmpAccount.Inn) ? (!string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? "Where INN Is NUll And OKPO='" + tmpAccount.TollFree2 + "'" : "") : (!string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? "Where (INN = '" + tmpAccount.Inn + "') And OKPO='" + tmpAccount.TollFree2 + "'" : "Where (INN = '" + tmpAccount.Inn + "') And OKPO Is Null");
        if (string.IsNullOrWhiteSpace (tmpAccount.TollFree2)) {
            using (var session = new SessionScopeWrapper ()) {
                if (isCorporate) {
                    var accountShort = session.CreateSQLQuery ("Select PRIME_ACCOUNTID, INN, OKPO, HISTORYINNLIST, ISACTIVE From V_FB_PRIME_ACCOUNTSHORT " + where)
                        .ListAs (new { PrimeAccountId = default (string), Inn = default (string), Okpo = default (string), HistoryInnList = default (string), IsActive = default (string) });
                    if (accountShort.Count > 0)
                        primeAccountId = accountShort.Count () == 1 ? accountShort.FirstOrDefault ().PrimeAccountId : accountShort.Count () > 1 ? accountShort.Count (x => x.IsActive == "T") == 1 ? accountShort.Where (x => x.IsActive == "T").FirstOrDefault ().PrimeAccountId : accountShort.Count (x => x.IsActive == "T") == 0 ? accountShort.OrderBy (x => x.PrimeAccountId).FirstOrDefault ().PrimeAccountId : string.Empty : string.Empty;
                    else {
                        var account = session.CreateSQLQuery ("Select PRIME_ACCOUNTID, INN, OKPO, HISTORYINNLIST, ISACTIVE, REGISTRATIONDATE From V_FB_PRIME_ACCOUNT " + where)
                            .ListAs (new { PrimeAccountId = default (string), Inn = default (string), Okpo = default (string), HistoryInnList = default (string), IsActive = default (string), RegistrationDate = default (DateTime?) });
                        if (account.Count > 0)
                            primeAccountId = account.Count == 1 ? account.FirstOrDefault ().PrimeAccountId : (account.Count > 1 ? (account.Count (x => x.IsActive == "T") == 1 ? account.Where (x => x.IsActive == "T").FirstOrDefault ().PrimeAccountId : (account.Count (x => x.IsActive == "T") == 0 ? account.OrderBy (x => x.PrimeAccountId).FirstOrDefault ().PrimeAccountId : string.Empty)) : string.Empty);
                    }
                } else
                    primeAccountId = session.CreateSQLQuery ("Select PRIME_ACCOUNTID From V_FB_PRIME_ACCOUNTIP " + where).List<string> ().FirstOrDefault ();
            }
        }
        return primeAccountId;
    }*/
    private XDocument GetDataFromPrime (string fullPrimeUrl) {
        try {
            var webRequest = WebRequest.Create (fullPrimeUrl);
            var webResponse = webRequest.GetResponse ();
            if (((HttpWebResponse) webResponse).StatusCode == HttpStatusCode.OK) {
                using (var responseStream = webResponse.GetResponseStream ())
                using (var streamReader = new System.IO.StreamReader (responseStream)) {
                    var result = XDocument.Parse (streamReader.ReadToEnd ());
                    var resultCodeElement = result.Descendants ("ResultCode").FirstOrDefault ();
                    var resultMessagElement = result.Descendants ("ResultMessage").FirstOrDefault ();
                    if (resultCodeElement != null && resultCodeElement.Value == "0")
                        return result;
                    else DefaultResponse (209, string.Format ("Возникла ошибка при обработке ответа от web - сервиса Прайм: <br /> {0} ", HttpUtility.HtmlEncode (resultCodeElement != null ? resultMessagElement.Value : "Неизвестная ошибка"))); //ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_PrimeValidationMessage", "setTimeout(function() { Sage.UI.Dialogs.alert('" + string.Format("Возникла ошибка при обработке ответа от web-сервиса Прайм:<br />{0}", HttpUtility.HtmlEncode(resultCodeElement != null ? resultCodeElement.Value : "Неизвестная ошибка")) + "'); }, 1);", true);
                }
            } else DefaultResponse (209, string.Format ("Возникла ошибка при обращении к web-сервису Прайм: <br /> {0}", HttpUtility.HtmlEncode (((HttpWebResponse) webResponse).StatusDescription))); //ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_PrimeValidationMessage", "setTimeout(function() { Sage.UI.Dialogs.alert('" + string.Format("Возникла ошибка при обращении к web-сервису Прайм:<br />{0}", HttpUtility.HtmlEncode(((HttpWebResponse)webResponse).StatusDescription)) + "'); }, 1);", true);
        } catch (Exception ex) {
            DefaultResponse (209, string.Format ("Во время выполнения запроса произошла ошибка: <br /> {0}", HttpUtility.HtmlEncode (ex.Message)).Replace ("\r", string.Empty).Replace ("\n", string.Empty)); //ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_PrimeValidationMessage", "setTimeout(function(){Sage.UI.Dialogs.alert('" + string.Format("Во время выполнения запроса произошла ошибка:<br />{0}", HttpUtility.HtmlEncode(ex.Message)).Replace("\r", string.Empty).Replace("\n", string.Empty) + "');}, 1);", true);
        }
        return null;
    }
    private static T GetChildElementValue<T> (XElement element, XName elementName) {
        var childElement = element.Element (elementName);
        if (childElement == null) return default (T);
        return GetElementValue<T> (childElement);
    }
    private static T GetElementValue<T> (XElement element) {
        var nullAttribute = element.Attribute ("{http://www.w3.org/2001/XMLSchema-instance}nil");
        var isNull = nullAttribute != null && nullAttribute.Value == "true";
        var type = Nullable.GetUnderlyingType (typeof (T)) ?? typeof (T);
        if (type == typeof (DateTime) && !isNull) return (T) Convert.ChangeType (DateTimeOffset.Parse (element.Value).DateTime, type, CultureInfo.InvariantCulture);
        return isNull ? default (T) : (T) Convert.ChangeType (element.Value, type, CultureInfo.InvariantCulture);
    }
    /*private string CreatePrimeRequestRecord (string dataType, List<KeyValuePair<string, string>> parameters, bool isDerectRequest, int priority, IAccount tmpAccount) {
        var xmlRequest = string.Format ("<?xml version=\"1.0\" encoding=\"windows-1251\"?>\r\n<REQUEST>\r\n    <DATATYPE>{0}</DATATYPE>\r\n    <PARAMETERS>\r\n{1}\r\n    </PARAMETERS>\r\n</REQUEST>", dataType, string.Join ("\r\n", parameters.Select (x => string.Format ("        <PARAMETER Name=\"{0}\" Value=\"{1}\" />", x.Key, x.Value))));
        var request = Sage.Platform.EntityFactory.Create<IFbPrimeRequestView> ();
        request.StatusCode = 1;
        request.Status = "NEW";
        request.DataType = dataType;
        request.XmlRequest = xmlRequest.ToString ();
        request.Priority = priority;
        request.IsDirectRequest = isDerectRequest;
        request.AccountId = Convert.ToString (tmpAccount.Id);
        request.Save ();
        return Convert.ToString (request.Id);
    }*/
    private void UpdateRevenue (XDocument primeRecord, IAccount tmpAccount) {
        var record = primeRecord != null ? primeRecord.Descendants ("Record") : null;
        if (record == null) return;
        var primeRevenueRecords = record.OrderByDescending (x => x.Descendants ("Year")?.FirstOrDefault ().Value).FirstOrDefault ();
        if (primeRevenueRecords == null) return;
        var year = primeRevenueRecords.Descendants ("Year").FirstOrDefault ()?.Value;
        var primeRevenue = primeRevenueRecords.Descendants ("AValue").FirstOrDefault ()?.Value;
        if (primeRevenueRecords == null && string.IsNullOrWhiteSpace (year)) return;
        using (var session = new SessionScopeWrapper ()) {
            var revenueDto = new { RevenueYear = default (int?), Okpo = string.Empty, Revenue = default (double?), AiPokazatelId = string.Empty };
            var revenue = session.CreateSQLQuery ("Select YEAR_FIN as RevenueYear, OKPO as Okpo, YEARCURR as Revenue, AI_POKAZATELID as AiPokazatelId From NM_CRM.AI_FININFO Where AI_POKAZATELID = '0' And OKPO =:Okpo and  YEAR_FIN=:Year")
                .SetString ("Okpo", tmpAccount.TollFree2)
                .SetString ("Year", year)
                .ListAs (revenueDto)
                .FirstOrDefault ();
            if (revenue == null) {
                session.CreateSQLQuery ("Insert into NM_CRM.AI_FININFO (AI_POKAZATELID, YEARCURR, OKPO, YEAR_FIN) Values (:IaPokazatelId, :YearCurr, :Okpo, :Year)")
                    .SetString ("IaPokazatelId", "0")
                    .SetString ("YearCurr", primeRevenue)
                    .SetString ("Okpo", tmpAccount.TollFree2)
                    .SetString ("Year", year)
                    .ExecuteUpdate ();
            }
        }
    }
    /*private void UpdateEmptyFields (XDocument primeRecord, IAccount tmpAccount) {
        using (var session = new SessionScopeWrapper ()) {
            #region update fields from chache
            //var eventTarget = Page.Request.Params.Get("__EVENTTARGET");
            //var eventArgument = Request["__EVENTARGUMENT"];
            //if (!string.IsNullOrWhiteSpace(eventTarget) && eventTarget == "doUpdate" && !string.IsNullOrWhiteSpace(eventArgument))
            //{
            //    var response = session.CreateSQLQuery("Select STATUS_CODE, XML_RESPONSE from V_FB_PRIME_REQUEST where FB_PRIME_REQUESTID =:primeId")
            //                     .SetString("primeId" , eventArgument)
            //                     .ListAs(new { StatusCode = default(int), XMLRespone = default(string) })
            //                     .FirstOrDefault();
            //    if (response == null)
            //    {
            //        response = session.CreateSQLQuery("Select STATUS_CODE, XML_RESPONSE from V_FB_PRIME_REQUEST_ARCHIVE where FB_PRIME_REQUESTID =:primeId")
            //                          .SetString("primeId", eventArgument)
            //                          .ListAs(new { StatusCode = default(int), XMLRespone = default(string) })
            //                          .FirstOrDefault();
            //    }
            //    if (response != null && response.StatusCode == 0)
            //    {
            //        var result = XDocument.Parse(response.XMLRespone);
            //        var resultCode = result.Descendants("RESULT_CODE").FirstOrDefault();
            //        var records = resultCode != null ? (resultCode.Value == "0" ? result.Descendants("RESULTS").ToList() : null) : null;
            //        primeAccountId = records != null ? (records.Count == 1 ? records.Elements("RESULT").Where(x => x.Attribute("Name").Value == "PRIME_ACCOUNTID").Select(x => x.Attribute("Value").Value).FirstOrDefault() : null): null;
            //    }
            //}
            //if (string.IsNullOrWhiteSpace(primeAccountId))
            //{
            //    var currentUser = ApplicationContext.Current.Services.Get<IUserService>(true).UserId;
            //    var response = session.CreateSQLQuery("Select FB_PRIME_REQUESTID, STATUS_CODE, CREATEUSER, XML_REQUEST, XML_RESPONSE from V_FB_PRIME_REQUEST where ACCOUNTID =:accountId and DATATYPE = 'ACCOUNT' order by MODIFYDATE desc")
            //                   .SetString("accountId", Convert.ToString(tmpAccount.Id))
            //                   .ListAs(new {PrimeRequestId = string.Empty, StatusCode = default(int), CreateUser = string.Empty, XMLRequest = string.Empty, XMLRespone =string.Empty })
            //                   .FirstOrDefault();
            //    if (response != null)
            //    {
            //        var shouldUpdate = response.CreateUser == currentUser;
            //    }
            //}
            //dynamic accountCorpFromCache = null;
            //dynamic accountIpFromChache = null;
            //if (!string.IsNullOrWhiteSpace(primeAccountId))
            //{
            //    accountCorpFromCache = session.CreateSQLQuery("select CHIEFEGRULPOST,CHIEFEGRULLASTNAME, CHIEFEGRULFIRSTNAME,CHIEFEGRULMIDDLENAME,CHIEFEGRULINN,OKFS,OKOPF,OKOGU,JURADDRESS,OKPO,INN,OGRN,PHONE,FAX,EMAIL,WEB,KPP,OKTMO,AUTHORIZEDCAPITAL,QUANTITY,FOUNDERSNUMBER,EGRULSTATUS,SHORTNAMEEGRUL,LONGNAMEEGRUL,BRANCHNAME2014,BRANCH2014,BRANCHNAME,BRANCH,LOCATIONID,REGIONNAME,NAMEENG,REGISTRATIONDATE from v_fb_prime_account where PRIME_ACCOUNTID=:primeId")
            //                           .SetString("primeId", primeAccountId)
            //                           .ListAs(new { ChiefEGRULPost = string.Empty, ChiefEGRULLastName = string.Empty, ChiefEGRULFirstName = string.Empty, ChiefEGRULMiddleName = string.Empty, ChiefEGRULInn = string.Empty, Okfs = default(int?), Okopf = default(int?), Okogu = default(int?), JurAddress = string.Empty, Okpo = string.Empty, Inn = string.Empty, Ogrn = string.Empty, Phone = string.Empty, Fax = string.Empty, Email = string.Empty, Web = string.Empty, Kpp = string.Empty, Oktmo = string.Empty, AuthorizedCapital = default(double?), Quantity = default(double?), FoundersNumber = default(int?), EGRULStatus = string.Empty, ShortNameEGRUL = string.Empty, LongNameEGRUL = string.Empty, BranchName2014 = string.Empty, Branch2014 = string.Empty, BranchName = string.Empty, Branch = string.Empty, LocationId = string.Empty, RegionName = string.Empty, NameEng = string.Empty, RegistrationDate = default(DateTime?) })
            //                           .FirstOrDefault();

            //    accountIpFromChache = session.CreateSQLQuery("select FIO, REGIONNAME,LOCATIONID, OKPO, INN,OKOPF,OKOGU, OKFS, OGRN, REGISTRATIONDATE, BRANCH2014,BRANCHNAME2014, BRANCH,BRANCHNAME from v_fb_prime_accountip  where PRIME_ACCOUNTID=:primeId")
            //                          .SetString("primeId", primeAccountId)
            //                          .ListAs(new { Fio = string.Empty, Okpo = string.Empty, RegionName = string.Empty, LocationId = string.Empty, Inn = string.Empty, Okopf = default(int?), Okogu = default(int?), Okfs = default(int?), Ogrn = string.Empty, RegistrationDate = default(DateTime?), Branch2014 = string.Empty, BranchName2014 = string.Empty, Branch = string.Empty, BranchName = string.Empty })
            //                          .FirstOrDefault();
            //}
            #endregion
            if (primeRecord == null) return;

            var record = primeRecord != null ? primeRecord.Descendants ("Record").FirstOrDefault () : null;
            var isImported = tmpAccount.FirstImportDate.HasValue;
            var isCorporate = string.IsNullOrWhiteSpace (tmpAccount.Inn) ? (string.IsNullOrWhiteSpace (tmpAccount.TollFree2) ? true : tmpAccount.TollFree2.Length == 8) : tmpAccount.Inn.Length == 10;
            var branch = GetChildElementValue<string> (record, "Branch2014");
            var branchVersion = string.IsNullOrWhiteSpace (branch) ? 2001 : 2014;
            branch = string.IsNullOrWhiteSpace (branch) ? GetChildElementValue<string> (record, "Branch") : branch;
            if (string.IsNullOrWhiteSpace (tmpAccount.AccountName) && !isImported && tmpAccount.AccountName != GetChildElementValue<string> (record, isCorporate ? "ShortNameEGRUL" : "FIO")) tmpAccount.AccountName = CropValueByField (typeof (IAccount), nameof (tmpAccount.AccountName), GetChildElementValue<string> (record, isCorporate ? "ShortNameEGRUL" : "FIO"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Aka) && !isImported && tmpAccount.Aka != GetChildElementValue<string> (record, isCorporate ? "LongNameEGRUL" : "FIO")) tmpAccount.Aka = CropValueByField (typeof (IAccount), nameof (tmpAccount.Aka), GetChildElementValue<string> (record, isCorporate ? "LongNameEGRUL" : "FIO"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Okfs) && !isImported) {
                var okfsCode = GetChildElementValue<string> (record, "OKFS");
                var okfs = string.IsNullOrWhiteSpace (okfsCode) ? null : session.CreateSQLQuery ("Select SHORTNAME From FB_OKFS Where CODE = :okfsCode").SetString ("okfsCode", okfsCode).List<string> ().FirstOrDefault ();
                if (tmpAccount.Okfs != okfs) tmpAccount.Okfs = CropValueByField (typeof (IAccount), nameof (tmpAccount.Okfs), okfs);
            }
            if (string.IsNullOrWhiteSpace (tmpAccount.TollFree2) && !isImported && tmpAccount.TollFree2 != GetChildElementValue<string> (record, isCorporate ? "OKPO" : "ipOKPO")) tmpAccount.TollFree2 = CropValueByField (typeof (IAccount), nameof (tmpAccount.TollFree2), GetChildElementValue<string> (record, isCorporate ? "OKPO" : "ipOKPO"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Inn) && !isImported && tmpAccount.Inn != GetChildElementValue<string> (record, "INN")) tmpAccount.Inn = CropValueByField (typeof (IAccount), nameof (tmpAccount.Inn), GetChildElementValue<string> (record, "INN"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Territory) && !isImported) {
                var okopfCode = GetChildElementValue<string> (record, "OKOPF");
                var okopf = string.IsNullOrWhiteSpace (okopfCode) ? null : session.CreateSQLQuery ("Select SHORTNAME From FB_OKOPF Where CODE = :okopfCode").SetString ("okopfCode", okopfCode).List<string> ().FirstOrDefault ();
                if (tmpAccount.Territory != okopf) tmpAccount.Territory = CropValueByField (typeof (IAccount), nameof (tmpAccount.Territory), okopf);
            }
            if (string.IsNullOrWhiteSpace (tmpAccount.State)) {
                var egrulStatus = GetChildElementValue<string> (record, "EgrulStatus");
                tmpAccount.State = CropValueByField (typeof (IAccount), nameof (tmpAccount.State), string.IsNullOrWhiteSpace (egrulStatus) ? "Не определено" : egrulStatus);
            }
            if (string.IsNullOrWhiteSpace (tmpAccount.Okogu) && tmpAccount.Okogu != GetChildElementValue<string> (record, "OKOGU")) tmpAccount.Okogu = CropValueByField (typeof (IAccount), nameof (tmpAccount.Okogu), GetChildElementValue<string> (record, "OKOGU"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Okato) && tmpAccount.Okato != GetChildElementValue<string> (record, "LocationId")) tmpAccount.Okato = CropValueByField (typeof (IAccount), nameof (tmpAccount.Okato), GetChildElementValue<string> (record, "LocationId"));
            if (string.IsNullOrWhiteSpace (tmpAccount.RegionPrime) && tmpAccount.RegionPrime != GetChildElementValue<string> (record, "RegionName")) tmpAccount.RegionPrime = CropValueByField (typeof (IAccount), nameof (tmpAccount.RegionPrime), GetChildElementValue<string> (record, "RegionName"));
            if (string.IsNullOrWhiteSpace (tmpAccount.MainPhone) && tmpAccount.MainPhone != GetChildElementValue<string> (record, "Phone")) tmpAccount.MainPhone = CropValueByField (typeof (IAccount), nameof (tmpAccount.MainPhone), GetChildElementValue<string> (record, "Phone"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Fax) && tmpAccount.Fax != GetChildElementValue<string> (record, "Fax")) tmpAccount.Fax = CropValueByField (typeof (IAccount), nameof (tmpAccount.Fax), GetChildElementValue<string> (record, "Fax"));
            if (string.IsNullOrWhiteSpace (tmpAccount.WebAddress) && tmpAccount.WebAddress != GetChildElementValue<string> (record, "Web")) tmpAccount.WebAddress = CropValueByField (typeof (IAccount), nameof (tmpAccount.WebAddress), GetChildElementValue<string> (record, "Web"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Email) && tmpAccount.Email != GetChildElementValue<string> (record, "EMail")) tmpAccount.Email = CropValueByField (typeof (IAccount), nameof (tmpAccount.Email), GetChildElementValue<string> (record, "EMail"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Oktmo) && tmpAccount.Oktmo != GetChildElementValue<string> (record, "OKTMO")) tmpAccount.Oktmo = CropValueByField (typeof (IAccount), nameof (tmpAccount.Oktmo), GetChildElementValue<string> (record, "OKTMO"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Kpp) && tmpAccount.Kpp != GetChildElementValue<string> (record, "KPP")) tmpAccount.Kpp = CropValueByField (typeof (IAccount), nameof (tmpAccount.Kpp), GetChildElementValue<string> (record, "KPP"));
            if (string.IsNullOrWhiteSpace (tmpAccount.Ogrn) && tmpAccount.Ogrn != GetChildElementValue<string> (record, "OGRN")) tmpAccount.Ogrn = CropValueByField (typeof (IAccount), nameof (tmpAccount.Ogrn), GetChildElementValue<string> (record, "OGRN"));
            if (string.IsNullOrWhiteSpace (tmpAccount.EngName) && tmpAccount.EngName != GetChildElementValue<string> (record, "ShortNameEgrulTranslit")) tmpAccount.EngName = CropValueByField (typeof (IAccount), nameof (tmpAccount.EngName), GetChildElementValue<string> (record, "ShortNameEgrulTranslit"));
            if (string.IsNullOrWhiteSpace (tmpAccount.EngNameSource) && !string.IsNullOrWhiteSpace (tmpAccount.EngName)) tmpAccount.EngNameSource = "Транслит";
            if (!tmpAccount.Employees.HasValue && tmpAccount.Employees != GetChildElementValue<int?> (record, "Quantity")) tmpAccount.Employees = GetChildElementValue<int?> (record, "Quantity");
            if (!tmpAccount.AuthorizedCapital.HasValue && tmpAccount.AuthorizedCapital != GetChildElementValue<decimal?> (record, "AuthorizedCapital")) {
                tmpAccount.AuthorizedCapital = GetChildElementValue<decimal?> (record, "AuthorizedCapital");
                tmpAccount.IsFoundersCountManual = false;
            }
            if (!tmpAccount.FoundersCount.HasValue && tmpAccount.FoundersCount != GetChildElementValue<int?> (record, "FoundersNumber")) {
                tmpAccount.FoundersCount = GetChildElementValue<int?> (record, "FoundersNumber");
                tmpAccount.IsFoundersCountManual = false;
            }
            if (!tmpAccount.RegistrationDate.HasValue && tmpAccount.RegistrationDate != GetChildElementValue<DateTime?> (record, isCorporate ? "RegistrationDate_2002" : "RegistrationDate")) tmpAccount.RegistrationDate = GetChildElementValue<DateTime?> (record, isCorporate ? "RegistrationDate_2002" : "RegistrationDate");
            if (isCorporate && !tmpAccount.RegistrationDate.HasValue) tmpAccount.RegistrationDate = GetChildElementValue<DateTime?> (record, "RegistrationDate");
            if (tmpAccount.Okveds.Count == 0) {
                tmpAccount.Okveds.Clear ();
                if (!string.IsNullOrWhiteSpace (branch)) {
                    var okved = Sage.Platform.EntityFactory.Create<IFbAccountOkved> ();
                    okved.Account = tmpAccount;
                    okved.Okved = CropValueByField (typeof (IFbAccountOkved), nameof (okved.Okved), branch);
                    okved.SourceOkved = "прайм";
                    okved.Version = branchVersion;
                    okved.IsPrimarySource = true;
                    okved.IsPrimary = true;
                    okved.OkvedEntity = Sage.Platform.EntityFactory.GetRepository<IFbOkved> ().FindFirstByProperty ("OkvedVersionId", string.Format ("{0}_{1}", branchVersion, branch));
                    tmpAccount.Okveds.Add (okved);
                }
            }
            if (tmpAccount != null) tmpAccount.Save ();

            session.CreateSQLQuery ("Call NB_UPDATE_ACC_CATEGORY (:v_ID)").SetString ("v_ID", Convert.ToString (tmpAccount.Id)).UniqueResult ();
            session.Refresh (tmpAccount);
            DefaultResponse (200, "Данные обновлены");
        }

    }*/
    private string CropValueByField (Type entityType, string propertyName, string value) {
        if (string.IsNullOrEmpty (value)) return value;

        var lengthAttribute = (ExtendedTypeAttribute) entityType.GetProperty (propertyName).GetCustomAttributes (typeof (ExtendedTypeAttribute), false).FirstOrDefault ();
        if (lengthAttribute != null && lengthAttribute.Length.HasValue && value.Length > lengthAttribute.Length.Value) return value.Substring (0, lengthAttribute.Length.Value);

        return value;
    }
    #endregion
    /*private void ValidateWorkerRole () {
        Response.Clear ();
        Response.ContentType = "application/json";
        try {
            string entityId = DefaultEncoding (Request.Headers["entityId"]);

            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (entityId);
            if (tmpAccount == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }

            bool WorkerHaveRoleInClientManagerDivision = false;
            bool WorkerHaveRoleInSupportManagerDivision = false;

            var rolesClientManager = string.Empty;
            switch (tmpAccount.Category) {
                case "Крупный":
                case "Крупнейший":
                    rolesClientManager = "(ISACCOUNTMANAGER = 'T' Or ISTRANSACTMANAGER = 'T' Or ISACCOUNTMANAGER_KB = 'T')";
                    break;
                case "Средний":
                case "Средний_":
                    rolesClientManager = "(ISACCOUNTMANAGER = 'T' Or ISTRANSACTMANAGER = 'T' Or ISACCOUNTMANAGER_SB = 'T')";
                    break;
                case "Малый":
                    rolesClientManager = "(ISACCOUNTMANAGER = 'T' Or ISTRANSACTMANAGER = 'T' Or ISACCOUNTMANAGER_MB = 'T')";
                    break;
                case "Некорпоративный":
                    rolesClientManager = "(ISACCOUNTMANAGER = 'T' Or ISTRANSACTMANAGER = 'T' Or ISACCOUNTMANAGER_IB = 'T')";
                    break;
                case "Private Banking":
                case "Другое":
                    rolesClientManager = "(ISACCOUNTMANAGER = 'T')";
                    break;
                default:
                    rolesClientManager = "(ISACCOUNTMANAGER = 'T' Or ISTRANSACTMANAGER = 'T' Or ISACCOUNTMANAGER_KB = 'T' Or ISACCOUNTMANAGER_SB = 'T' Or ISACCOUNTMANAGER_MB = 'T' Or ISACCOUNTMANAGER_IB = 'T')";
                    break;
            }
            var rolesSupportManager = string.Empty;
            switch (tmpAccount.Category) {
                case "Крупный":
                case "Крупнейший":
                    rolesSupportManager = "(ISSUPPORTMANAGER = 'T' Or ISSUPPORTMANAGER_KB = 'T')";
                    break;
                case "Средний":
                case "Средний_":
                    rolesSupportManager = "(ISSUPPORTMANAGER = 'T' Or ISSUPPORTMANAGER_SB = 'T')";
                    break;
                case "Малый":
                    rolesSupportManager = "(ISSUPPORTMANAGER = 'T' Or ISSUPPORTMANAGER_MB = 'T')";
                    break;
                case "Некорпоративный":
                    rolesSupportManager = "(ISSUPPORTMANAGER = 'T' Or ISSUPPORTMANAGER_IB = 'T')";
                    break;
                case "Private Banking":
                case "Другое":
                    WorkerHaveRoleInSupportManagerDivision = false;
                    break;
                default:
                    rolesSupportManager = "(ISSUPPORTMANAGER = 'T' Or ISSUPPORTMANAGER_KB = 'T' Or ISSUPPORTMANAGER_SB = 'T' Or ISSUPPORTMANAGER_MB = 'T' Or ISSUPPORTMANAGER_IB = 'T')";
                    break;
            }

            using (var session = new SessionScopeWrapper ()) {
                //ClientManager
                var currentUserIdClientManager = ApplicationContext.Current.Services.Get<IUserService> (true).UserId;
                var workerIdClientManager = session.CreateSQLQuery ("Select FB_WORKERID From V_FB_WORKER Where USERID = :userId")
                    .SetString ("userId", currentUserIdClientManager).List<string> ().FirstOrDefault ();

                WorkerHaveRoleInClientManagerDivision = !string.IsNullOrWhiteSpace (workerIdClientManager) && session.CreateSQLQuery (string.Format ("Select FB_DIV_WORKER_ROLEID From FB_DIV_WORKER_ROLE Where FB_DIVISIONID In (Select FB_DIVISIONID From FB_DIV_WORKER_ROLE Where FB_WORKERID = :managerId And {0}) And FB_WORKERID = :workerId", rolesClientManager))
                    .SetString ("managerId", tmpAccount.ClientManagerId).SetString ("workerId", workerIdClientManager).List ().Count > 0;
                //SupportManager
                var currentUserIdSupportManager = ApplicationContext.Current.Services.Get<IUserService> (true).UserId;
                var workerIdSupportManager = session.CreateSQLQuery ("Select FB_WORKERID From V_FB_WORKER Where USERID = :userId")
                    .SetString ("userId", currentUserIdSupportManager).List<string> ().FirstOrDefault ();
                WorkerHaveRoleInSupportManagerDivision = !string.IsNullOrWhiteSpace (workerIdSupportManager) && session.CreateSQLQuery (string.Format ("Select FB_DIV_WORKER_ROLEID From FB_DIV_WORKER_ROLE Where FB_DIVISIONID In (Select FB_DIVISIONID From FB_DIV_WORKER_ROLE Where FB_WORKERID = :managerId And {0}) And FB_WORKERID = :workerId", rolesSupportManager))
                    .SetString ("managerId", tmpAccount.SupportManagerId).SetString ("workerId", workerIdSupportManager).List ().Count > 0;
            }

            Dictionary<string, bool> responce = new Dictionary<string, bool> ();
            responce.Add ("RoleInClientManager", WorkerHaveRoleInClientManagerDivision);
            responce.Add ("RoleSupportManager", WorkerHaveRoleInSupportManagerDivision);
            DefaultResponse (200, JsonConvert.SerializeObject (responce));
        } catch (Exception exception) {
            ExceptionResponse (exception, "ValidateWorkerRole");
        }
    }*/
    /*private void AccountAddRevenue () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["revenue"]), DefaultEncoding (Request.Headers["year"]), DefaultEncoding (Request.Headers["mode"]) };

        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1]) || string.IsNullOrEmpty (headers[2])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);

            if (tmpAccount == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }

            using (var session = new SessionScopeWrapper ()) {
                var revenue = session.CreateQuery ($"from FbRevenue Where EntityId = '{Convert.ToString(tmpAccount.Id)}' And RevenueYear = '{Convert.ToString(headers[2])}'")
                    .List<IFbRevenue> ().FirstOrDefault ();

                if (headers[3] == "change") {
                    if (revenue != null) {
                        revenue.IsManual = true;
                        revenue.Revenue = Convert.ToDecimal (headers[1]);
                        revenue.Save ();
                    } else {
                        IFbRevenue newRevenue = Sage.Platform.EntityFactory.Create<IFbRevenue> ();
                        newRevenue.EntityId = Convert.ToString (tmpAccount.Id);
                        newRevenue.IsManual = true;
                        newRevenue.Revenue = Convert.ToDecimal (headers[1]);
                        newRevenue.RevenueYear = Convert.ToInt32 (headers[2]);
                        newRevenue.Save ();
                    }
                } else if (headers[3] == "clear" && revenue != null) revenue.Delete ();

                session.Refresh (tmpAccount);
                DefaultResponse (200, "Данные обновлены");
            }
        } catch (Exception exception) {
            ExceptionResponse (exception, "AccountAddRevenue");
        }
    }*/

    /*private void PrognozGreenDataChanged () {
        Response.Clear ();
        Response.ContentType = "application/json";
        string[] headers = { DefaultEncoding (Request.Headers["entityId"]), DefaultEncoding (Request.Headers["mode"]), DefaultEncoding (Request.Headers["entity"]) };
        if (string.IsNullOrEmpty (headers[0]) || string.IsNullOrEmpty (headers[1]) || string.IsNullOrEmpty (headers[2])) {
            DefaultResponse (400, "Uncorrect headers");
            return;
        }
        try {
            IAccount tmpAccount = Sage.Platform.EntityFactory.GetById<IAccount> (headers[0]);
            if (tmpAccount == null) {
                DefaultResponse (400, "Uncorrect entity");
                return;
            }
            if (headers[1] == "clear") {
                if (headers[2] == "prognoz" || headers[2] == "greenData") {
                    if (headers[2] == "prognoz") tmpAccount.PrognozAccountId = string.Empty;
                    else if (headers[2] == "greenData") tmpAccount.GreendataAccountId = string.Empty;
                    tmpAccount.Save ();
                    using (var session = new SessionScopeWrapper ()) {
                        session.Refresh (tmpAccount);
                    }
                    DefaultResponse (200, $"correct clear {headers[2]}");
                }
                DefaultResponse (400, $"Uncorrect clear  {headers[2]}");
            } else if (headers[1] == "change") {
                var validateResult = prognozExportValidate (Convert.ToString (tmpAccount.Id));
                if (validateResult == "Ready") {
                    var requestId = string.Empty;
                    var MaxWaitTime = string.Empty;
                    var Interval = string.Empty;
                    var customSettings = Sage.Platform.EntityFactory.GetRepository<ICustomSetting> ().FindByProperty ("Category", "ExportToPrognoz").ToList ();
                    foreach (var customSetting in customSettings) {
                        if (customSetting.Description == "Timeout") MaxWaitTime = customSetting.DataValue;
                        if (customSetting.Description == "Interval") Interval = customSetting.DataValue;
                    }
                    if (headers[2] == "prognoz") requestId = CreatePrognozRequest ("PROGNOZ", tmpAccount);
                    else if (headers[2] == "greenData") requestId = CreatePrognozRequest ("GREENDATA", tmpAccount);
                    Dictionary<string, string> responce = new Dictionary<string, string> ();
                    responce.Add ("requestId", requestId);
                    responce.Add ("interval", Interval);
                    responce.Add ("maxWaitTime", MaxWaitTime);
                    DefaultResponse (200, JsonConvert.SerializeObject (responce));
                } else DefaultResponse (400, validateResult);
            }
        } catch (Exception exception) {
            ExceptionResponse (exception, "PrognozGreenDataChanged");
        }
    }*/

    private void GetGroupInfo()
    {
        Response.Clear();
        Response.ContentType = "application/json";
        string header = DefaultEncoding(Request.Headers["gid"]);

        try
        {
            DefaultResponse(200, JsonConvert.SerializeObject(GroupInfo.GetGroupInfo(header)));
        }
        catch (Exception exception)
        {
            ExceptionResponse(exception, "GetGroup");
        }
    }

    private void GetGroupNonce()
    {
        Response.Clear();
        Response.ContentType = "application/json";

        try
        {
            DefaultResponse(200, JsonConvert.SerializeObject(GroupInfo.GetGroupNonce()));
        }
        catch (Exception exception)
        {
            ExceptionResponse(exception, "GetGroupNonce");
        }
    }

    private void RemoveFromFavorites()
    {
        Response.Clear();
        Response.ContentType = "application/json";
        string header = DefaultEncoding(Request.Headers["gid"]);

        try
        {
            FavoriteGroupsManager.RemoveFromFavorites(header);
            DefaultResponse(200, "true");
        }
        catch (Exception exception)
        {
            ExceptionResponse(exception, "RemoveFromFavorites");
        }
    }

    private void BuildGCS ()
    {
        Response.Clear();
        Response.ContentType = "application/json";
        var bodyStream = new System.IO.StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();

        var arrIdentifier = bodyText.Split(',');
        var countarrIdentifier = arrIdentifier.Length;
        List<string> queryIdentifier = new List<string>();
        var tempQuery = "";
        
        if (arrIdentifier.Length != 0)
            tempQuery += arrIdentifier[0] + ",";

        for (int j = 0; j < (arrIdentifier.Length/500)+1; j++)
        {
            if (j * 500 < arrIdentifier.Length)
            {
                for (int i = j * 500 + 1; i < arrIdentifier.Length; i++)
                {
                    tempQuery += arrIdentifier[i] + ",";
                    if (!(i % 500 != 0 || i == 0))
                        break;
                }
                tempQuery = tempQuery.Substring(0, tempQuery.Length - 1);
                queryIdentifier.Add(tempQuery);
                tempQuery = "";
            }
        }
        var typeIdentifier = DefaultEncoding(Request.Headers["typeIdentifier"]);

        try
        {
            IGroupContextService service = ApplicationContext.Current.Services.Get<IGroupContextService>() as GroupContextService;

            service.CurrentTable = "ACCOUNT";
            var currentGroupInfo = service.GetGroupContext().CurrentGroupInfo;
            currentGroupInfo.LookupTempGroup.ClearConditions();

            for (int i = 0; i < queryIdentifier.Count; i++)
            {
                if (i != (queryIdentifier.Count - 1))
                {
                    currentGroupInfo.LookupTempGroup.AddLookupCondition(string.Format("ACCOUNT:{0}", typeIdentifier), " IN ", string.Format("({0})", queryIdentifier[i]), "OR");
                }
                else
                {
                    currentGroupInfo.LookupTempGroup.AddLookupCondition(string.Format("ACCOUNT:{0}", typeIdentifier), " IN ", string.Format("({0})", queryIdentifier[i]));
                }
            }
            currentGroupInfo.LookupTempGroup.GroupXML = GroupInfo.RebuildGroupXML(currentGroupInfo.LookupTempGroup.GroupXML);
            service.CurrentGroupID = "LOOKUPRESULTS";
            currentGroupInfo.CurrentGroup.Flush();
            
            DefaultResponse(200, "Account.aspx?gid=LOOKUPRESULTS&modeid=list");
        }
        catch (Exception exception)
        {
            ExceptionResponse(exception, "RemoveFromFavorites");
        }
    }

    // метод обработки AJAX запроса и формирования сессии записи в БД принятых данных
    private void CreateUserOptionDef()
    {
        Response.Clear();                           // очистить запрос

        Response.ContentType = "application/json";  // формат запроса

        // массив заголовка запроса
        string[] headers = { DefaultEncoding(Request.Headers["Category"]), DefaultEncoding(Request.Headers["Name"]), DefaultEncoding(Request.Headers["DefValue"]) };

        string desc = headers[2];
        
        if (headers[1] == "ActivityStatusClientMainViewCount")
        {
            using (var session = new SessionScopeWrapper())
            {
                var data = new { NAME = string.Empty };
                var query = session.CreateSQLQuery("SELECT NAME FROM USEROPTIONDEF WHERE NAME='ActivityStatusClientMainViewCount'").ListAs(data);

                if (query.Count == 0)
                {
                    // insert
                    session.CreateSQLQuery(@"INSERT INTO USEROPTIONDEF (NAME, CATEGORY, DEFVALUE) VALUES ('ActivityStatusClientMainViewCount', 'AlarmOption', :description)")
                        .SetString("description", desc)
                        .ExecuteUpdate();
                }
                else
                {
                    // update
                    session.CreateSQLQuery("UPDATE USEROPTIONDEF SET DEFVALUE = :description WHERE Name = 'ActivityStatusClientMainViewCount'")
                        .SetString("description", desc)
                        .ExecuteUpdate();
                }
            }
        }
        else
        if (headers[1] == "ActivityTypeClientMainViewCount")
        {
            using (var session = new SessionScopeWrapper())
            {
                var data = new { NAME = string.Empty };
                var query = session.CreateSQLQuery("SELECT NAME FROM USEROPTIONDEF WHERE NAME='ActivityTypeClientMainViewCount'").ListAs(data);

                if (query.Count == 0)
                {
                    // insert
                    session.CreateSQLQuery(@"INSERT INTO USEROPTIONDEF (NAME, CATEGORY, DEFVALUE) VALUES ('ActivityTypeClientMainViewCount', 'AlarmOption', :description)")
                        .SetString("description", desc)
                        .ExecuteUpdate();
                }
                else
                {
                    // update
                    session.CreateSQLQuery("UPDATE USEROPTIONDEF SET DEFVALUE = :description WHERE Name = 'ActivityTypeClientMainViewCount'")
                        .SetString("description", desc)
                        .ExecuteUpdate();
                }
            }
        }
    }
    #region Auxiliary elements for work PrognozGreenDataChanged
    private string prognozExportValidate (string id) {
        var result = "Обнаружены ошибки:" + "<br />";
        bool hasErrors = false;
        using (var session = new SessionScopeWrapper ()) {
            var checkList = session.CreateSQLQuery ("SELECT CHECK_NAME AS CheckName, CHECK_SQL AS CheckSQL, CHECK_MESSAGE AS CheckMessage FROM FB_PROGNOZ_ACC_CHECK").ListAs<Check> ();
            foreach (var check in checkList) {
                var replaced = check.CheckSQL.Replace ("{ACCOUNTID}", ":accountId");
                try {
                    var sqlList = session.CreateSQLQuery (replaced).SetString ("accountId", id).List ();
                    if (sqlList.Count == 0) {
                        hasErrors = true;
                        result = result + check.CheckMessage + "; <br />";
                    }
                } catch (Exception) {
                    hasErrors = true;
                    result = result + "Не удалось выполнить проверку: " + check.CheckName + Environment.NewLine + "Обратитесь к системному администратору.";
                }
            }
        }
        return (!hasErrors) ? result = "Ready" : result;
    }
    /*private string CreatePrognozRequest (string claim, IAccount tmpAccount) {
        IFbPrognozAccount request = null;
        var requests = Sage.Platform.EntityFactory.GetRepository<IFbPrognozAccount> ().FindByProperty ("AccountId", Convert.ToString (tmpAccount.Id)).ToList ();
        foreach (var temp in requests) {
            if (temp.ClaimSystem == claim)
                request = temp;
        }
        if (request == null) {
            request = Sage.Platform.EntityFactory.Create<IFbPrognozAccount> ();
            request.AccountId = Convert.ToString (tmpAccount.Id);
        } else {
            if (claim == "PROGNOZ")
                if (!string.IsNullOrWhiteSpace (tmpAccount.PrognozAccountId))
                    return "";
            if (claim == "GREENDATA")
                if (!string.IsNullOrWhiteSpace (tmpAccount.GreendataAccountId))
                    return "";
        }
        request.StatusCode = 2;
        request.TryToSendCount = 0;
        request.ClaimSystem = claim;
        if (claim == "PROGNOZ")
            request.Status = "SEND_TO_PROGNOZ_READY";
        if (claim == "GREENDATA")
            request.Status = "SEND_TO_GREENDATA_READY";
        request.Save ();
        var xmlData = FormXmlData (Convert.ToString (request.Id), tmpAccount);
        request.XmlContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n " + xmlData.ToString ();
        request.Save ();
        return Convert.ToString (request.Id);
    }*/
    private XDocument FormXmlData (string requestId, IAccount tmpAccount) {
        var xmlData = new XDocument ();
        XElement temp;
        var request = new XElement ("REQUEST");
        var requestDate = new XElement ("REQUEST_DATE", DateTime.Now.ToString ("yyyy-MM-dd H:mm:ss"));
        var listOfClients = new XElement ("LISTOF_CLIENTS");
        using (var session = new SessionScopeWrapper ()) {
            var paramlist = session.CreateSQLQuery ("SELECT PARAM_NAME AS ParamName, PROGNOZ_ATRIBUTE AS PrognozAtribute, SLX_TABLE AS SlxTable, SLX_FIELD AS SlxField FROM FB_PROGNOZ_ACC_PARAM").ListAs<Parameter> ();
            foreach (var param in paramlist) {
                if (param.SlxTable == "FB_PROGNOZ_ACC") {
                    temp = new XElement (param.PrognozAtribute, new XCData (requestId));
                    temp.Add (new XAttribute ("isNull", "false"));
                    listOfClients.Add (temp);
                    continue;
                }
                var reglist = session.CreateSQLQuery (string.Format ("SELECT {0} FROM {1} WHERE ACCOUNTID=:accountId", param.SlxField, param.SlxTable))
                    .SetString ("accountId", Convert.ToString (tmpAccount.Id)).List ();
                if (reglist.Count == 0) {
                    temp = new XElement (param.PrognozAtribute, new XCData (""));
                    temp.Add (new XAttribute ("isNull", "true"));
                } else {
                    var value = reglist[0];
                    if (value != null && value.GetType () == typeof (DateTime)) value = ((DateTime) value).ToString ("yyyy-MM-dd H:mm:ss");
                    temp = new XElement (param.PrognozAtribute, new XCData (Convert.ToString (value)));
                    if (string.IsNullOrEmpty (Convert.ToString (value))) temp.Add (new XAttribute ("isNull", "true"));
                    else temp.Add (new XAttribute ("isNull", "false"));
                }
                listOfClients.Add (temp);
            }
        }
        request.Add (requestDate);
        request.Add (listOfClients);
        xmlData.Add (request);
        return xmlData;
    }
    class Url {
        public string UrlConstant { get; set; }
        public string UrlVariable { get; set; }
        public string UrlConstant2 { get; set; }
    }
    class Check {
        public string CheckName { get; set; }
        public string CheckSQL { get; set; }
        public string CheckMessage { get; set; }
    }
    class Parameter {
        public string ParamName { get; set; }
        public string PrognozAtribute { get; set; }
        public string SlxTable { get; set; }
        public string SlxField { get; set; }
    }
    #endregion

}