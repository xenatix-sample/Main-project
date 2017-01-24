-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetEligibilityDetails]
-- Author:		Justin Spalti
-- Date:		10/30/2015
--
-- Purpose:		Get Contact's ECI Eligibility Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/30/2015	Justin Spalti	TFS:2700	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetEligibilityDetail]
       @EligibilityID BIGINT,
       @ResultCode int OUTPUT,
       @ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
       SELECT
       @ResultCode = 0,
       @ResultMessage = 'Executed Successfully'

       BEGIN TRY
              SELECT	e.ContactID, e.EligibilityID, e.EligibilityDate,
                        e.EligibilityCategoryID, e.EligibilityTypeID, e.EligibilityDurationID, c.DOB as DOB,
						et.EligibilityType, ec.EligibilityCategory,
                        e.IsActive, e.ModifiedBy, e.ModifiedOn
              FROM		ECI.Eligibility e
			  INNER JOIN Registration.Contact c
			  ON		e.ContactID = c.ContactID
			  INNER JOIN ECI.EligibilityType et
				ON et.EligibilityTypeID = e.EligibilityTypeID
			  INNER JOIN ECI.EligibilityCategory ec
				ON ec.EligibilityCategoryID = e.EligibilityCategoryID
              WHERE		e.EligibilityID = @EligibilityID
              AND		e.IsActive = 1

       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END