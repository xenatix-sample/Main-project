-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetLastActiveReviewOfSystems]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Get last active review of systems by contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/22/2015	Rajiv Ranjan	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetLastActiveReviewOfSystems]
       @ContactID BIGINT,
       @ResultCode INT OUTPUT,
       @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
       BEGIN TRY
			SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

			SELECT 
				TOP 1
				ros.RoSID, 
				ros.ContactID, 
				ros.DateEntered, 
				ros.ReviewdBy, 
				ros.AssessmentID, 
				ros.ResponseID,
				ros.IsReviewChanged, 
				ros.ModifiedBy, 
				ros.ModifiedOn
			FROM 
				[Clinical].[ReviewOfSystems]  ros				
				INNER JOIN Core.AssessmentResponses ar ON ros.AssessmentID = ar.AssessmentID AND ros.ResponseID = ar.ResponseID
				INNER JOIN Core.AssessmentResponseDetails ard on ar.ResponseID = ard.ResponseID
			WHERE 
				ros.IsActive=1 
				AND ar.IsActive=1
				AND ard.IsActive=1
				AND ros.ContactID = @ContactID
			ORDER BY
				ros.ResponseID DESC
         
       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END
GO