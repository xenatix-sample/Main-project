-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_GetConfig]
-- Author:		Chad Roberts
-- Date:		1/26/2016
--
-- Purpose:		Get a configuration for a service
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Chad Roberts	Initial creation.
-- 9/5/2016		Rahul Vats		Added Logic for Retrieving the values on the basis of ConfigName
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_GetServiceConfiguration]
	@ConfigID INT = 0,
	@ConfigName NVARCHAR(255) = '',
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'
			--This approach limits the number of files we have to create and keeps the flexibility of specifying either the ID or Name to get the Config.
			if(@ConfigID = 0 and @ConfigName = '')
			Begin
				RAISERROR (
					'Please Pass Either The ConfigID or ConfigName to fetch the Config Values.', -- Message text.
					16, -- Severity.
					1 -- State.
				);
			End
			if(@ConfigID > 0)
			Begin
				SELECT ConfigID,ConfigName,ConfigXML,ConfigTypeID,IsActive,ModifiedBy,ModifiedOn
					FROM Synch.Config
				WHERE IsActive=1 AND ConfigID=@ConfigID
			End
			Else
			Begin
				SELECT ConfigID,ConfigName,ConfigXML,ConfigTypeID,IsActive,ModifiedBy,ModifiedOn
					FROM Synch.Config
				WHERE IsActive=1 AND ConfigName=@ConfigName
			End
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END