

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetReferralCategorySource]
-- Author:		Sumana Sangapu
-- Date:		01/04/2016
--
-- Purpose:		Gets the list of  ReferralCategorySource lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetReferralCategorySource]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ReferralCategorySourceID, ReferralCategoryID, ReferralSource  
		FROM		[Reference].[ReferralCategorySource] 
		WHERE		IsActive = 1 
		ORDER BY	[ReferralCategorySourceID]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
