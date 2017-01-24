-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_RefreshActiveDirectoryRefData]
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
-- 08/31/2016	Rahul Vats	Initial Checkin.
-- 09/06/2016	Rahul Vats	Review the proc
-- 10/13/2016	Rahul Vats	Added Configurable Verbosity and catch the AD Error Code
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_RefreshActiveDirectoryRefData] 
(
	@BatchID int,
	@Verbose int = 1,
	@ActiveDirectoryPath varchar(max) = '',
	@ActiveDirectoryGroupName varchar (max) = '',
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';
	BEGIN TRY
		--Truncate Tables
		TRUNCATE TABLE [Synch].[ActiveDirectoryRefData]
		TRUNCATE TABLE [Synch].[ADUserStage]

		Declare @LDAPQuery varchar(MAX) = 'SELECT 
			title,
			department,
			physicalDeliveryOfficeName,
			mobile,
			facsimileTelephoneNumber,
			displayName,
			company,
			division,
			homePhone,
			pager,
			userprincipalname,
			distinguishedname,
			userAccountControl,
			co,
			postalCode,
			st,
			l,
			streetAddress,
			mail,
			telephoneNumber,
			initials,
			sn,
			middleName,
			givenName,
			employeeNumber,
			employeeID,
			samaccountname,
			manager,
			objectguid
		FROM ''' + @ActiveDirectoryPath +  ''' WHERE objectclass=''user'' AND memberOf = '''+@ActiveDirectoryGroupName+''''
		--Print @LDAPQuery
		INSERT INTO [Synch].[ActiveDirectoryRefData]
		(
			objectguid,
			manager,
			samaccountname,
			employeeID,
			employeeNumber,
			givenName,
			middleName,
			sn,
			initials,
			telephoneNumber,
			mail,
			streetAddress,
			l,
			st,
			postalCode,
			co,
			userAccountControl,
			distinguishedname,
			userprincipalname,
			pager,
			homePhone,
			division,
			company,
			displayName,
			facsimileTelephoneNumber,
			mobile,
			physicalDeliveryOfficeName,
			department,
			title
		)
		EXEC [Synch].[usp_QueryAD] @LDAPQuery, @Verbose, @ResultCode OUTPUT, @ResultMessage OUTPUT
		--Do Any Formatting or Prettification to the values here before inserting them into ADUserStage
		
		IF @ResultCode = 0
		BEGIN
			EXEC [Synch].[usp_MergeADUserSyncValidations] @ResultCode OUTPUT, @ResultMessage OUTPUT
			--Final Insert into Synch.ADUserStage
			INSERT INTO Synch.ADUserStage (
				BatchID,
				UserGUID,
				Manager,
				UserName,
				FirstName,
				MiddleName,
				LastName,
				Initials,
				Phone,
				Email,
				AddressLine1,
				City,
				StateProvince,
				ZipPostalCode,
				CountryRegion,
				UserIdentifier_1,
				UserIdentifier_2,
				IsActive,
				CreatedBy,
				ModifiedBy,
				CreatedOn,
				EffectiveFromDate,
				EffectiveToDate,
				ModifiedOn,
				SystemCreatedOn,
				SystemModifiedOn
			)
			Select 
				@BatchID,
				Synch.fn_ConvertBinaryCharGUIDToCharGUID(objectguid),
				manager,
				samaccountname,
				givenName,
				middleName,
				sn,
				initials,
				telephoneNumber,
				mail,
				streetAddress,
				l,
				st,
				postalCode,
				co,
				employeeID, --Sumana told me thats what it matches upto
				employeeNumber, --Sumana told me thats what it matches upto
				Case when (userAccountControl & 2) = 0 then 1 else 0 end,
				1,
				1,
				GETDATE(),
				GETDATE(),
				Case when (userAccountControl & 2) = 0 then null else GETDATE() end,
				GETDATE(),
				GETDATE(),
				GETDATE()
			from Synch.ActiveDirectoryRefData
			where (ErrorMessage is null) and (userAccountControl & 2 = 0)

			select
				StaffID = UserIdentifier_1,
				CountStaffID = count(UserIdentifier_1)
				into #tmpStaffID 
			from Synch.ADUserStage
			group by UserIdentifier_1
			having count(UserIdentifier_1) > 1 

			update Synch.ADUserStage set ErrorMessage = IsNull(ErrorMessage,'') + ' Duplicate Staff ID' where UserIdentifier_1 in (Select StaffID from #tmpStaffID)

			select
				MHMRTCID = UserIdentifier_2,
				CountMHMRTCID = count(UserIdentifier_2)
				into #tmpMHMRTCID 
			from Synch.ADUserStage
			group by UserIdentifier_2
			having count(UserIdentifier_2) > 1 

			update Synch.ADUserStage set ErrorMessage = IsNull(ErrorMessage,'') + ' Duplicate MHMRTC ID' where UserIdentifier_2 in (Select MHMRTCID from #tmpMHMRTCID)
		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
