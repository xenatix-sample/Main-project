----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetReferralAgency]
-- Author:		Gaurav Gupta
-- Date:		3/3/2016
--
-- Purpose:		Gets the list of  ReferralAgency lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	Gaurav Gupta	TFS#  - Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetReferralAgency]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ReferralAgencyID, ReferralAgency 
		FROM		Reference.ReferralAgency
		WHERE		IsActive = 1
		ORDER BY	ReferralAgency ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

