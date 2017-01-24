-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetFinancialSummaries]
-- Author:		Gurpreet Singh
-- Date:		05/16/2016
--
-- Purpose:		Get All Contact Financial Assessment record by Contact ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/16/2016	Gurpreet Singh	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetFinancialSummaries]
	@ContactID BIGINT,
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
		CredentialID
	FROM
		Registration.FinancialSummary FS
	WHERE
		IsActive = 1
		AND ContactID = @ContactID
	ORDER BY
		EffectiveDate DESC;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO