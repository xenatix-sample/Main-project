-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetFinancialSummary]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Get Contact Financial Assessment record by Contact ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 09/16/2016	Vishal Yadav	selected ModifiedOn
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetFinancialSummaryByContactID]
	@FinancialSummaryID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	SELECT
		FinancialSummaryID,
		ContactID,
		OrganizationID,
		FinancialAssessmentXML,
		DateSigned,
		EffectiveDate,
		AssessmentEndDate,
		ExpirationDate,
		SignatureStatusID,
		UserID,
		UserPhoneID,
		CredentialID,
		ModifiedOn
	FROM
		Registration.FinancialSummary FS
	WHERE
		IsActive = 1
		AND FinancialSummaryID = @FinancialSummaryID
	ORDER BY
		EffectiveDate DESC;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO

