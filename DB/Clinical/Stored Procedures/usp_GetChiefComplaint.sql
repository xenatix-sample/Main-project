

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetChiefComplaint]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Get list of Chief Complaints
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/30/2015	John Crossen	TFS# 2886 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE Clinical.[usp_GetChiefComplaint]
@ChiefComplaintID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

		   SELECT ChiefComplaintID,ChiefComplaint, ContactID, TakenBy, TakenTime, EncounterID,
		   ModifiedBy, ModifiedOn
		   FROM Clinical.ChiefComplaint
		   WHERE IsActive=1 AND ChiefComplaintID=@ChiefComplaintID
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END