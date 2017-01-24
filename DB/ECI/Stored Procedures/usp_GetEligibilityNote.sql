-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetEligibilityNote]
-- Author:		Justin Spalti
-- Date:		11/03/2015
--
-- Purpose:		Get Contact's ECI Eligibility Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/03/2015	Justin Spalti	TFS:2700	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetEligibilityNote]
       @EligibilityID BIGINT,
       @ResultCode int OUTPUT,
       @ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
    SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

    BEGIN TRY
            SELECT e.EligibilityID, e.Notes
            FROM	ECI.Eligibility e
			WHERE e.EligibilityID = @EligibilityID
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END

