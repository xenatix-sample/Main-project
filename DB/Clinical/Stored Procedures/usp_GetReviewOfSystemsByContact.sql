-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetReviewOfSystemsByContact]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Get a list of review of systems by contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin	Initial creation.
-- 11/19/2015	Rajiv Ranjan	Added ReviewdByName.
-- 11/21/2015	Rajiv Ranjan	Added IsReviewChanged
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetReviewOfSystemsByContact]
       @ContactID BIGINT,
       @ResultCode INT OUTPUT,
       @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
       BEGIN TRY
			SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

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
				ros.ModifiedOn
			FROM 
				[Clinical].[ReviewOfSystems]  ros
				LEFT JOIN [Core].[Users] u ON ros.ReviewdBy = u.UserID
			WHERE 
				ros.IsActive=1 
				AND ContactID=@ContactID
         
       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END
GO