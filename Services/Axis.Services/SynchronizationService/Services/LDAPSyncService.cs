using Axis.Constant;
using Axis.DataProvider.SynchronizationService;
using Axis.Model.ServiceBatch;
using Axis.Logging;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Xml;

namespace SynchronizationService.Services
{
    public static class LDAPSyncService
    {
        private static long _lastUSN;
        //11408 - Technical - Centralize the Sync DB for AD to the main DB for the axis Solution
        //11538 - Update the Strings in the Services Engine to match the changes in the DB
        //Staging tables for populating data from Active Directory
        private const string UserStageTable = "Synch.ADUserStage";
        private const string RoleStageTable = "Synch.ADRoleStage";
        private const string UserRoleStageTable = "Synch.ADUserRoleStage";
        //I need to review the other changes in order to verify the rewuirement for 11408
        private static string _usnChangedToken = "usnChanged";
        private const long MinADDate = 0;
        private const long MaxADDate = 9223372036854775807;
        private static bool _debugMode = false;
        private static bool _debugLogFileMode = false;
        private static ILogger _logger;

        //public static void Sync(Object source, ElapsedEventArgs e)
        public static void Sync(int configID)
        {
            _debugMode = ConfigurationManager.AppSettings["Debug"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["Debug"]);
            _debugLogFileMode = ConfigurationManager.AppSettings["DebugLogFile"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["DebugLogFile"]);

            // first lets get our current config, in case something has been updated
            var syncServ = new SynchronizationServiceDataProvider(_logger);
            var config = syncServ.GetServiceConfiguration(configID).DataItems[0];


            // we only want to run this service is we have an active config for it
            // todo: the plan is at some point to have a network monitoring tool to set which configs are active, so services on multiple servers know if they should be running or not
            // NOTE: by having the services continue to check their configs, the configs can be changed 
            if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Using the following ConfigXML: " + config.ConfigXML, EventLogEntryType.Information); 
            if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                {
                    sw.WriteLine("Using the following ConfigXML: " + config.ConfigXML);
                }

            if (config != null && config.IsActive != null && (bool)config.IsActive)
            {
                /*
                 * config.ConfigXML reads from the DB the configuration database containing
                 * SyncServer
                 *  - ProcessingServer
                 * Timer 
                 *  - Interval
                 * LDAPServer
                 *  - LDAPPath
                 *  - LDAPUserFilter
                 *  - LDAPRoleFilter
                 *  - Name
                 *  - Port: Defaultvalue="389"
                */
                using (XmlReader reader = XmlReader.Create(new StringReader(config.ConfigXML)))
                {
                    // create a directory identifier and connection, 
                    reader.ReadToFollowing("SyncServer");
                    reader.ReadToDescendant("ProcessingServer");
                    reader.MoveToFirstAttribute();
                    var processingServer = reader.Value;

                    reader.ReadToFollowing("LDAPServer");
                    reader.ReadToDescendant("LDAPPath");
                    reader.MoveToFirstAttribute();
                    var ldapPath = reader.Value;
                    reader.ReadToFollowing("LDAPUserFilter");
                    reader.MoveToFirstAttribute();
                    var ldapUserFilterFromConfig = reader.Value;
                    reader.ReadToFollowing("LDAPRoleFilter");
                    reader.MoveToFirstAttribute();
                    var ldapRoleFilterFromConfig = reader.Value;

                    if (_debugMode) EventLog.WriteEntry("SynchronizationService",
                        "Sync Server host name should be " + System.Net.Dns.GetHostName() + 
                            ", ldap sync server configured is " + processingServer +
                            ", ldap path configured is " + ldapPath +
                            ", ldap user filter configured is " + ldapUserFilterFromConfig +
                            ", ldap role filter from config is " + ldapRoleFilterFromConfig,
                        EventLogEntryType.Information); 

                    if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                        {
                            sw.WriteLine("Sync Server host name should be " + System.Net.Dns.GetHostName() +
                            ", ldap sync server configured is " + processingServer +
                            ", ldap path configured is " + ldapPath +
                            ", ldap user filter configured is " + ldapUserFilterFromConfig +
                            ", ldap role filter from config is " + ldapRoleFilterFromConfig);
                        }
                    //Till here make sure that you have the right configuration on the right machine

                    // we need to look at the previous batch to know which updates to start requesting, as well as to make sure its completed before starting a new batch
                    var lastBatch = syncServ.GetLastBatch(config.ConfigID); // the batches are by config, not config type, as you can have more than 1 config for a given type
                    if (
                            processingServer == System.Net.Dns.GetHostName() &&
                        
                            // we want to make sure we have the ldap info we need to pull what we need from AD
                            (
                                ldapPath != string.Empty && ldapUserFilterFromConfig != string.Empty && ldapRoleFilterFromConfig != string.Empty
                            ) 
                            
                            &&
                        
                            // only proceed w/ a new AD request, once the last request has been completed
                            (
                                lastBatch.DataItems.Count < 1 || 
                                ( lastBatch.DataItems[0] != null && lastBatch.DataItems[0].BatchStatusID >= (int)BatchStatus.Completed )
                            )
                        ) 
                    {
                        var usnChanged = (lastBatch.DataItems.Count > 0) ? lastBatch.DataItems[0].USN : 0;

                        var ldapUserFilter = ldapUserFilterFromConfig.Replace(_usnChangedToken, usnChanged.ToString());
                        if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Searching AD for users using ldap filter string: " + ldapUserFilter, EventLogEntryType.Information);
                        if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                        {
                            sw.WriteLine("Searching AD for users using ldap filter string: " + ldapUserFilter);
                        }
                        var resultsUsers = SearchAD(ldapPath, ldapUserFilter);

                        var ldapRolesFilter = ldapRoleFilterFromConfig.Replace(_usnChangedToken, usnChanged.ToString());
                        if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Searching AD for roles using ldap filter string: " + ldapRolesFilter, EventLogEntryType.Information);
                        if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                        {
                            sw.WriteLine("Searching AD for roles using ldap filter string: " + ldapRolesFilter);
                        }
                        var resultsRoles = SearchAD(ldapPath, ldapRolesFilter);

                        // we only want to create a new batch, if there are actually results to be processed
                        if ((resultsUsers != null && resultsRoles != null) && (resultsUsers.Count > 0 || resultsRoles.Count > 0))
                        {
                            // add this new request to the db
                            var batch = new ServiceBatchModel { BatchStatusID = (int)BatchStatus.Requesting, BatchTypeID = (int)BatchType.ADSynService, ConfigID = config.ConfigID, USN = _lastUSN + 1, IsActive = true };
                            batch.BatchID = syncServ.AddBatch(batch).ID;
                            if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Batch created, ID: " + batch.BatchID, EventLogEntryType.Information);
                            if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                            {
                                sw.WriteLine("Batch created, ID: " + batch.BatchID);
                            }

                            var dtUsers = BuildUsersDataTable(resultsUsers, batch.BatchID, ldapPath);
                            if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Inserting users into UserStage table.", EventLogEntryType.Information);
                            if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                            {
                                sw.WriteLine("Inserting users into UserStage table.");
                            }
                            syncServ.BulkInsert(dtUsers, UserStageTable, _debugMode, _debugLogFileMode);

                            var dtRoles = BuildRolesDataTable(resultsRoles, batch.BatchID);
                            if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Inserting roles into RoleStage table.", EventLogEntryType.Information);
                            if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                            {
                                sw.WriteLine("Inserting roles into RoleStage table.");
                            }
                            syncServ.BulkInsert(dtRoles, RoleStageTable, _debugMode, _debugLogFileMode);

                            // also, lets populate which groups the users belong to
                            var dtUserRoles = BuildUserRolesDataTable(resultsRoles, batch.BatchID, ldapPath);
                            if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Inserting user's roles into RoleStage table.", EventLogEntryType.Information);
                            if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                            {
                                sw.WriteLine("Inserting user's roles into RoleStage table.");
                            }
                            syncServ.BulkInsert(dtUserRoles, UserRoleStageTable, _debugMode, _debugLogFileMode);

                            // if we've added any staging data to be processed, update the batch created for this interval for keeping up w/ the data to be processed
                            // NOTE: adding +1 to the last usn, as we are doing a >= to filter by usn, so we don't want to get the last usn again, the next time we look for updates
                            batch.BatchStatusID = (int)BatchStatus.Completed;
                            batch.BatchTypeID = (int)BatchType.ADSynService;
                            batch.USN = _lastUSN + 1;
                            if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Updating batch to Pending status with USN " + batch.USN + ".", EventLogEntryType.Information);
                            if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                                {
                                    sw.WriteLine("Updating batch to Pending status with USN " + batch.USN + ".");
                                }
                            syncServ.UpdateBatch(batch);
                        }
                    }
                }
            }
        }

        private static SearchResultCollection SearchAD(string ldapPath, string ldapFilter)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ldapPath);
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                mySearcher.Filter = ldapFilter;
                mySearcher.Tombstone = true;
                SearchResultCollection results = mySearcher.FindAll();
                if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Active Directory search successful, result count is " + results.Count + ".", EventLogEntryType.Information);
                if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                {
                    sw.WriteLine("Active Directory search successful, result count is " + results.Count + ".");
                }
                return results;
            }
            catch (Exception e)
            {
                if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Error requesting date from Active Directory server: " + e.Message, EventLogEntryType.Error);
                if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                {
                    sw.WriteLine("Error requesting date from Active Directory server: " + e.Message);
                }
                return null;
            }
        }

        private static DataTable BuildUsersDataTable(SearchResultCollection results, long batchID, string ldapPath)
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("UserStageID"));
            dt.Columns.Add(new DataColumn("BatchID"));
            dt.Columns.Add(new DataColumn("UserGUID", typeof(byte[])));
            dt.Columns.Add(new DataColumn("ManagerGUID", typeof(byte[])));
            dt.Columns.Add(new DataColumn("UserName"));
            dt.Columns.Add(new DataColumn("FirstName"));
            dt.Columns.Add(new DataColumn("LastName"));
            dt.Columns.Add(new DataColumn("Phone"));
            dt.Columns.Add(new DataColumn("Email"));
            dt.Columns.Add(new DataColumn("AddressLine1"));
            dt.Columns.Add(new DataColumn("AddressLine2"));
            dt.Columns.Add(new DataColumn("City"));
            dt.Columns.Add(new DataColumn("StateProvince"));
            dt.Columns.Add(new DataColumn("ZipPostalCode"));
            dt.Columns.Add(new DataColumn("CountryRegion"));
            dt.Columns.Add(new DataColumn("IsTemporaryPassword", typeof(bool)));
            dt.Columns.Add(new DataColumn("EffectiveToDate"));
            dt.Columns.Add(new DataColumn("LoginAttempts"));
            dt.Columns.Add(new DataColumn("LoginCount"));
            dt.Columns.Add(new DataColumn("LastLogin"));
            dt.Columns.Add(new DataColumn("IsActive", typeof(bool)));
            dt.Columns.Add(new DataColumn("ModifiedBy"));
            dt.Columns.Add(new DataColumn("ModifiedOn"));
            dt.Columns.Add(new DataColumn("CreatedBy"));
            dt.Columns.Add(new DataColumn("CreatedOn"));
            dt.Columns.Add(new DataColumn("SystemCreatedOn"));
            dt.Columns.Add(new DataColumn("SystemModifiedOn"));
            
            foreach (SearchResult result in results)
            {
                var managerGUID = GetManagerGUID(result, ldapPath);

                var row = dt.NewRow();
                row["BatchID"] = batchID;
                row["UserGUID"] = result.Properties["objectguid"][0] as byte[];
                row["ManagerGUID"] = managerGUID;
                row["UserName"] = result.Properties["samaccountname"][0];
                row["FirstName"] = result.Properties["givenname"].Count > 0 ? result.Properties["givenname"][0] : string.Empty;
                row["LastName"] = result.Properties["sn"].Count > 0 ? result.Properties["sn"][0] : string.Empty;
                row["Phone"] = result.Properties["telephoneNumber"].Count > 0 ? result.Properties["telephoneNumber"][0] : string.Empty;
                row["Email"] = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0] : string.Empty;
                row["AddressLine1"] = result.Properties["streetaddress"].Count > 0 ? result.Properties["streetaddress"][0] : string.Empty;
                row["AddressLine2"] = result.Properties["postofficebox"].Count > 0 ? result.Properties["postofficebox"][0] : string.Empty;
                row["City"] = result.Properties["l"].Count > 0 ? result.Properties["l"][0] : string.Empty;
                row["StateProvince"] = result.Properties["st"].Count > 0 ? result.Properties["st"][0] : string.Empty;
                row["ZipPostalCode"] = result.Properties["postalcode"].Count > 0 ? result.Properties["postalcode"][0] : string.Empty;
                row["CountryRegion"] = result.Properties["co"].Count > 0 ? result.Properties["co"][0] : string.Empty;
                row["EffectiveToDate"] = (result.Properties["accountexpires"].Count > 0) ? ((long)(result.Properties["accountexpires"][0] ?? MinADDate) > MinADDate && (long)(result.Properties["accountexpires"][0] ?? MinADDate) < MaxADDate) ?
                    (DateTime?)DateTime.FromFileTime((long)(result.Properties["accountexpires"][0] ?? MinADDate)) : null : null;
                row["LoginAttempts"] = (result.Properties["badpwdcount"].Count > 0) ? (int) result.Properties["badpwdcount"][0] : 0;
                row["LoginCount"] = (result.Properties["logoncount"].Count > 0) ? (int)result.Properties["logoncount"][0] : 0;
                row["LastLogin"] = (result.Properties["lastlogon"].Count > 0) ? ((long)(result.Properties["lastlogon"][0] ?? MinADDate) > MinADDate && (long)(result.Properties["lastlogon"][0] ?? MinADDate) < MaxADDate) ?
                    (DateTime?)DateTime.FromFileTime((long)(result.Properties["lastlogon"][0] ?? MinADDate)) : null : null;
                row["IsActive"] = !Convert.ToBoolean((int)result.Properties["useraccountcontrol"][0] & 0x0002) && (result.Properties["IsDeleted"].Count <= 0 || !Convert.ToBoolean(result.Properties["IsDeleted"][0]));
                row["ModifiedBy"] = 0;
                row["ModifiedOn"] = DateTime.UtcNow;
                row["CreatedBy"] = 0;
                row["CreatedOn"] = DateTime.UtcNow;
                row["SystemCreatedOn"] = DateTime.UtcNow;
                row["SystemModifiedOn"] = DateTime.UtcNow;
                dt.Rows.Add(row);

                _lastUSN = (_lastUSN <= (long)result.Properties["usnchanged"][0]) ? (long)result.Properties["usnchanged"][0] : _lastUSN;
            }

            return dt;
        }

        private static byte[] GetManagerGUID(SearchResult result, string ldapPath)
        {
            byte[] managerGUID = null;

            if (result.Properties["manager"].Count == 1)
            {
                var managerDistinguishedName = result.Properties["manager"][0];
                var managerUsers = SearchAD(ldapPath,
                    "(&(objectCategory=user)(distinguishedname=" + managerDistinguishedName + "))");
                var managerUser = managerUsers.Count == 1 ? managerUsers[0] : null;
                if (managerUser != null) managerGUID = managerUser.Properties["objectguid"][0] as byte[];
            }

            return managerGUID;
        }

        private static DataTable BuildRolesDataTable(SearchResultCollection results, long batchID)
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("RoleID"));
            dt.Columns.Add(new DataColumn("BatchID"));
            dt.Columns.Add(new DataColumn("RoleGUID", typeof(byte[])));
            dt.Columns.Add(new DataColumn("Name"));
            dt.Columns.Add(new DataColumn("Description"));
            dt.Columns.Add(new DataColumn("IsActive", typeof(bool)));
            dt.Columns.Add(new DataColumn("ModifiedBy"));
            dt.Columns.Add(new DataColumn("ModifiedOn"));
            dt.Columns.Add(new DataColumn("CreatedBy"));
            dt.Columns.Add(new DataColumn("CreatedOn"));
            dt.Columns.Add(new DataColumn("SystemCreatedOn"));
            dt.Columns.Add(new DataColumn("SystemModifiedOn"));

            foreach (SearchResult result in results)
            {
                var row = dt.NewRow();
                row["BatchID"] = batchID;
                row["RoleGUID"] = result.Properties["objectguid"][0] as byte[];
                row["Name"] = result.Properties["name"][0];
                row["Description"] = result.Properties["description"].Count > 0 ? result.Properties["description"][0] : string.Empty;
                row["IsActive"] = result.Properties["IsDeleted"].Count <= 0 || !Convert.ToBoolean(result.Properties["IsDeleted"][0]);
                row["ModifiedBy"] = 0;
                row["ModifiedOn"] = DateTime.UtcNow;
                row["CreatedBy"] = 0;
                row["CreatedOn"] = DateTime.UtcNow;
                row["SystemCreatedOn"] = DateTime.UtcNow;
                row["SystemModifiedOn"] = DateTime.UtcNow;
                dt.Rows.Add(row);

                _lastUSN = (_lastUSN <= (long)result.Properties["usnchanged"][0]) ? (long)result.Properties["usnchanged"][0] : _lastUSN;
            }

            return dt;
        }

        private static DataTable BuildUserRolesDataTable(SearchResultCollection resultsRoles, long batchID, string ldapPath)
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("UserRoleStageID"));
            dt.Columns.Add(new DataColumn("BatchID"));
            dt.Columns.Add(new DataColumn("UserGUID", typeof(byte[])));
            dt.Columns.Add(new DataColumn("RoleGUID", typeof(byte[])));
            dt.Columns.Add(new DataColumn("IsActive", typeof(bool)));
            dt.Columns.Add(new DataColumn("ModifiedBy"));
            dt.Columns.Add(new DataColumn("ModifiedOn"));
            dt.Columns.Add(new DataColumn("CreatedBy"));
            dt.Columns.Add(new DataColumn("CreatedOn"));
            dt.Columns.Add(new DataColumn("SystemCreatedOn"));
            dt.Columns.Add(new DataColumn("SystemModifiedOn"));

            foreach (SearchResult resultRole in resultsRoles)
            {
                var members = resultRole.Properties["member"];
                foreach (string member in members)
                {
                    var memberUsers = SearchAD(ldapPath, "(&(objectCategory=user)(distinguishedname=" + member + "))");
                    var memberUser = memberUsers.Count == 1 ? memberUsers[0] : null;

                    // only set the association between a user and a role, if we find a unique user record as the member of a role 
                    // which we should b/c we are requesting by distinguished name
                    if (memberUser != null)
                    {
                        var row = dt.NewRow();
                        row["BatchID"] = batchID;
                        row["UserGUID"] = memberUser.Properties["objectguid"][0] as byte[];
                        row["RoleGUID"] = resultRole.Properties["objectguid"][0] as byte[];
                        row["IsActive"] = !Convert.ToBoolean((int)memberUser.Properties["useraccountcontrol"][0] & 0x0002) && (memberUser.Properties["IsDeleted"].Count <= 0 || !Convert.ToBoolean(memberUser.Properties["IsDeleted"][0]));
                        row["ModifiedBy"] = 0;
                        row["ModifiedOn"] = DateTime.UtcNow;
                        row["CreatedBy"] = 0;
                        row["CreatedOn"] = DateTime.UtcNow;
                        row["SystemCreatedOn"] = DateTime.UtcNow;
                        row["SystemModifiedOn"] = DateTime.UtcNow;
                        dt.Rows.Add(row);
                    }
                }
                

            }

            return dt;
        }
    }
}