-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetReviewOfSystem]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Get a single review of system
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/19/2015	Rajiv Ranjan	Added ReviewdByName.
-- 11/21/2015	Rajiv Ranjan	Added IsReviewChanged
-- 11/22/2015	Rajiv Ranjan	Added LastAssessmentOn
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_GetReviewOfSystem]
       @RoSID BIGINT,
       @ResultCode INT OUTPUT,
       @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
       BEGIN TRY
			SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

			DECLARE @LastAssessmentOn DATETIME

			SELECT 
				@LastAssessmentOn = MAX(ard.ModifiedOn)
			FROM 
				[Clinical].[ReviewOfSystems]  ros				
				INNER JOIN Core.AssessmentResponses ar ON ros.AssessmentID = ar.AssessmentID AND ros.ResponseID = ar.ResponseID
				INNER JOIN Core.AssessmentResponseDetails ard on ar.ResponseID = ard.ResponseID
			WHERE 
				ros.IsActive=1 
				AND ar.IsActive=1
				AND ard.IsActive=1
				AND ros.RoSID = @RoSID

			SELECT 
				ros.RoSID, 
				ros.ContactID, 
				ros.DateEntered, 
				ros.ReviewdBy,
				u.FirstName + ' ' + u.LastName as ReviewdByName, 
				ros.AssessmentID, 
				ros.ResponseID,
				ros.IsReviewChanged, 
				ros.ModifiedBy, 
				ros.ModifiedOn,
				@LastAssessmentOn AS LastAssessmentOn
			FROM 
				[Clinical].[ReviewOfSystems] ros
				LEFT JOIN [Core].[Users] u ON ros.ReviewdBy = u.UserID
			WHERE 
				ros.IsActive=1 
				AND ros.RoSID=@RoSID
         
       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                        @ResultMessage = ERROR_MESSAGE()
       END CATCH
END

GO