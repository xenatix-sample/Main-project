-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetResourceOverrides]
-- Author:		Rajiv Ranjan
-- Date:		10/29/2015
--
-- Purpose:		Gets the list of Resource Overrides
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/29/2015	Rajiv Ranjan	 Initial creation.
-- 01/15/2016    Satish Singh    @ResourceTypeID, @ResourceID  optional for where clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetResourceOverrides]
@ResourceID INT=NULL,
@ResourceTypeID SMALLINT=NULL,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

	SELECT
		RO.ResourceOverrideID,
		RO.ResourceID,
		RO.ResourceTypeID,
		RO.OverrideDate,
		RO.Comments,
		RO.FacilityID
	FROM 
		Scheduling.ResourceOverrides RO
	WHERE	
		(ISNULL(@ResourceID,0)=0 OR ResourceID=@ResourceID )
		AND (ISNULL(@ResourceTypeID,0)=0 OR ResourceTypeID = @ResourceTypeID)
		AND RO.IsActive=1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END