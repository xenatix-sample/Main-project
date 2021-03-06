-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetEligibility]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		Get Contact's ECI Eligibility Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu	TFS:2700	Initial Creation
-- 12/09/2015   Arun Choudhary  TFS:4244    Added orderby on eleigibility date.

-- exec [ECI].[usp_GetEligibility] 1,'',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetEligibility]
       @ContactID BIGINT,
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
              WHERE		e.ContactID = @ContactID
              AND		e.IsActive = 1
			  ORDER BY  EligibilityDate DESC

       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END
