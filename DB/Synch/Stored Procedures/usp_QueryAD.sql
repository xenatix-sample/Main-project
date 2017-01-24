-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_QueryAD]
-- Author:		Rahul Vats
-- Date:		08/31/2016
--
-- Purpose:		Query Active Directory from SQL Server
--
-- Notes:		The idea of this stored procedure is to be able to keep in Synch, the changes between AD and Axis Application
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/31/2016	Rahul Vats	Initial Checkin
-- 09/06/2016	Rahul Vats	Review the proc
-- 10/13/2016	Rahul Vats	Added More Meaningful message
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_QueryAD] 
(
	@LDAP_Query varchar(MAX) = '',
	@Verbose bit = 1, 
	@ResultCode INT = 0 OUTPUT,
	@ResultMessage NVARCHAR(MAX) = '' OUTPUT,
	@Error_Message varchar(250) = NULL OUTPUT
) AS 
BEGIN 
	--Verify Proper Usage and Display Help if not used properly
	--LDAP_Query Argument was not passed
	IF @LDAP_Query = '' 
	BEGIN 
		Print '*****************************************************************************************************************************************************'
		Print'===Query Manual===' + CHAR(10)
		Print 'usp_QueryAD is a stored procedure to query active directory without the default 1000 record LDAP query limit' + CHAR(10)
		Print 'usage — Exec usp_QueryAD ''_LDAP_Query_'', Verbose_Output(0 or 1, optional)' + CHAR(10)
		--In Our Case: The LDAP address should come from Synch.config table
		Declare @ActiveDirectoryPath varchar(200) = 'LDAP://example.domain/OU=SomeOU1,OU=SomeOU2,DC=domain,DC=control'
		Print 'example: Exec usp_QueryAD ''SELECT EmployeeID, SamAccountName FROM '''''+ @ActiveDirectoryPath +''''' WHERE objectCategory=''''person'''' and objectclass=''''user'''''', 1' + CHAR(10)
		Print 'usp_QueryAD returns records corresponding to fields specified in LDAP query.' + CHAR(10)
		Print 'Use INSERT INTO statement to capture results in temp table.' + CHAR(10)
		Print'===Query Manual End===' + CHAR(10)
		Print '*****************************************************************************************************************************************************'
		RETURN -100 --The Path was not specified 
	END 
	
	--Declare Variables
	DECLARE @i int; --Misc
	DECLARE @ADOconn INT -- ADO Connection object
	DECLARE @ADOcomm INT -- ADO Command object
	DECLARE @ADOcommprop INT -- ADO Command object properties pointer
	DECLARE @ADOcommpropVal INT -- ADO Command object properties value pointer
	DECLARE @ADOrs INT -- ADO RecordSet object
	DECLARE @OLEreturn INT -- OLE return value
	DECLARE @src varchar(255) -- OLE Error Source
	DECLARE @desc varchar(255) -- OLE Error Description
	DECLARE @PageSize varchar(6) -- variable for paging size Setting
	DECLARE @StatusStr char(255) -- variable for current status message for verbose output
	SET @PageSize = '12000'; -- IF not SET LDAP query will return max of 1000 rows; page size is the amount of data returned per “PAGE'', not the total number of rows
	
	Print '*****************************************************************************************************************************************************'
	Print'===Establishing Connection with AD and Executing Query===' + CHAR(10)
	--Create the ADO Connection Object
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Create ADO Connection…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OACreate 'ADODB.Connection', @ADOconn OUT 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in Creating a Connection' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10) 
		EXEC sp_OAGetErrorInfo 
			@ADOconn,
			@src OUT,
			@desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
	
	--SET the provider property to ADsDSOObject to point to Active Directory
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Set ADO Connection to use Active Directory driver…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OASetProperty @ADOconn, 'Provider', 'ADsDSOObject' 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in setting ADO Connection to use Active Directory driver' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10) 
		EXEC sp_OAGetErrorInfo @ADOconn, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
	
	--Open the ADO Connection
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Open the ADO Connection…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OAMethod @ADOconn, 'Open' 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in opening the ADO Connection' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10) 
		EXEC sp_OAGetErrorInfo @ADOconn, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
		
	--Create the ADO command object
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Create ADO command object…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OACreate 'ADODB.Command', @ADOcomm OUT 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in creating the ADO Command Object' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10) 
		EXEC sp_OAGetErrorInfo @ADOcomm, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10)
		
	--SET the ADO command object to use the Connection object created first
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Set ADO command object to use Active Directory Connection…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OASetProperty @ADOcomm, 'ActiveConnection', 'Provider=''ADsDSOObject''' 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in Setting ADO command object to use Active Directory Connection' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcomm, @src OUT, @desc OUT
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
		
	--Get a pointer to the properties SET of the ADO Command Object
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Retrieve ADO command properties…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OAGetProperty @ADOcomm, 'Properties', @ADOcommprop OUT 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in Retrieving ADO command properties' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcomm, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
		
	--SET the PageSize property
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Set ''PageSize'' property…' 
		Print @StatusStr 
	END
	IF (@PageSize IS NOT NULL)-- If PageSize is SET then SET the value
	BEGIN 
		EXEC @OLEreturn = sp_OAMethod @ADOcommprop, 'Item', @ADOcommpropVal OUT, 'Page Size' 
		IF @OLEreturn <> 0 
		BEGIN 
			-- Return OLE error
			IF @Verbose = 1 
			Print 'Error in setting the page size' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
			EXEC sp_OAGetErrorInfo @ADOcommprop, @src OUT, @desc OUT 
			GOTO OA_ERROR 
		END 
		EXEC @OLEreturn = sp_OASetProperty @ADOcommpropVal, 'Value', @PageSize 
		IF @OLEreturn <> 0 
		BEGIN 
			-- Return OLE error
			IF @Verbose = 1 
			Print 'Error in setting the page size' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
			EXEC sp_OAGetErrorInfo @ADOcommpropVal, @src OUT, @desc OUT 
			GOTO OA_ERROR 
		END 
		EXEC @OLEreturn = sp_OADestroy @ADOcommpropVal; 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
	
	--SET the SearchScope property to ADS_SCOPE_SUBTREE to search the entire subtree
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Set ''SearchScope'' property…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OAMethod @ADOcommprop, 'Item', @ADOcommpropVal OUT, 'SearchScope' 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in setting the Search Scope' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcommprop, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END 
	EXEC @OLEreturn = sp_OASetProperty @ADOcommpropVal, 'Value', '2' --ADS_SCOPE_SUBTREE
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in setting the Search Scope' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcommpropVal, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END 
	EXEC @OLEreturn = sp_OADestroy @ADOcommpropVal; 
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
	
	--SET the Asynchronous property to True
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Set ''Asynchronous'' property…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OAMethod @ADOcommprop, 'Item', @ADOcommpropVal OUT, 'Asynchronous' 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in setting Asynchronous Calls' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcommprop, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END 
	EXEC @OLEreturn = sp_OASetProperty @ADOcommpropVal, 'Value', TRUE 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in setting Asynchronous Calls' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcommpropVal, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END 
	EXEC @OLEreturn = sp_OADestroy @ADOcommpropVal; 
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 

	--SET the LDAP Query
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Input the LDAP Query…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OASetProperty @ADOcomm, 'CommandText', @LDAP_Query 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in setting LDAP Query' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcomm, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
		
	--Run the LDAP query and output the results to the ADO Recordset
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Execute the LDAP query…' 
		Print @StatusStr 
	END 
	EXEC @OLEreturn = sp_OAMethod @ADOcomm, 'Execute', @ADOrs OUT 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in (Executing LDAP Query/Execute clause of SP_OAMethod)' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10)
		EXEC sp_OAGetErrorInfo @ADOcomm, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END 
	--Return the rows found
	IF @Verbose=1 
	BEGIN
		SET @StatusStr = 'Retrieve the LDAP query results…' 
		Print @StatusStr 
	END 
	DECLARE @pwdlastset varchar(8) 
	EXEC @OLEreturn = sp_OAMethod @ADOrs, 'getrows' 
	IF @OLEreturn <> 0 
	BEGIN 
		-- Return OLE error
		IF @Verbose = 1 
			Print 'Error in (Retrieving the results of search/GetString of GetProperty)' + ' Error Code: ' + cast(@OLEreturn AS varchar) + CHAR(10) 
		EXEC sp_OAGetErrorInfo @ADOrs, @src OUT, @desc OUT 
		GOTO OA_ERROR 
	END
	IF @Verbose=1 
		Print Space(len(@StatusStr)) + 'Done...' + CHAR(10) 
	Print'===Closing Connection with AD Query Executed Successfully===' + CHAR(10)
	Print '*****************************************************************************************************************************************************'

	--Execution Done Successfully - Close the Connections
	DONE:
		Print '*****************************************************************************************************************************************************' 
		Print'===Closing The Connections===' + CHAR(10)
		Print'===Closing The Connections===' + CHAR(10)
		EXEC @i = sp_OAMethod @ADOrs, 'Close' 
		IF @Verbose = 1 
			print '@ADOrs close result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OAMethod @ADOconn, 'Close' 
		IF @Verbose = 1 
			print '@ADOconn close result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOcommprop; 
		IF @Verbose = 1 
			print '@ADOcommprop destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOrs; 
		IF @Verbose = 1 
			print '@ADOrs destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOcomm; 
		IF @Verbose = 1 
			print '@ADOcomm destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOconn; 
		IF @Verbose = 1 
			print '@ADOconn destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		Print'===Connections Closed===' + CHAR(10) 
		Print '*****************************************************************************************************************************************************'
		RETURN 0 
			
	OA_ERROR:
		Print '*****************************************************************************************************************************************************'
		Print'===Error Report===' + CHAR(10) 
		Set @Error_Message = 'There was error in connecting to AD: '
		Print @Error_Message
		SET @Error_Message = 'Error = ' + CHAR(10) + Space(4) 
							+ 'Error Code: ' + isnull(cast(@OLEreturn AS varchar),'?') + CHAR(10) + Space(4)
							+ 'Source: ' + isnull(@src,'?') + CHAR(10) + Space(4)
							+ 'Description: ' + isnull(@desc,'?') + CHAR(10); 
		IF @Verbose=1 
			Print @Error_Message; 
		EXEC @i = sp_OAMethod @ADOrs, 'Close' 
		IF @Verbose = 1 
			print '@ADOrs close result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OAMethod @ADOconn, 'Close' 
		IF @Verbose = 1
			print '@ADOconn close result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOcommprop; 
		IF @Verbose = 1 
			print '@ADOcommprop destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOrs; 
		IF @Verbose = 1 
			print '@ADOrs destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOcomm; 
		IF @Verbose = 1 
			print '@ADOcomm destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		EXEC @i = sp_OADestroy @ADOconn; 
		IF @Verbose = 1 
			print '@ADOconn destroy result = ' + cast(@i AS varchar) + CHAR(10) ; 
		Print'===End Error Report===' + CHAR(10) 
		Print '*****************************************************************************************************************************************************'
		Set @ResultCode = @OLEreturn
		Set @ResultMessage = @Error_Message
		RETURN @OLEreturn 
END
GO