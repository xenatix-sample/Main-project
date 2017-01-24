-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_GetSynchServices]
-- Author:		Chad Roberts
-- Date:		1/26/2016
--
-- Purpose:		Get the services to be run.
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Chad Roberts	Initial creation.
-----------------------------------------------------------------------------------------------------------------------


create PROCEDURE [Synch].[usp_GetServiceConfigurations]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

		   SELECT ConfigID,ConfigName,ConfigXML,ConfigTypeID,IsActive,ModifiedBy,ModifiedOn
		   FROM Synch.Config
		   WHERE IsActive=1
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END