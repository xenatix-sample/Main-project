
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_ReseedMRN
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Reseed MRN as per Client's requirement.
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	Sumana Sangapu	TFS:2662	Initial Creation

-- exec Registration.usp_ReseedMRN 112547,'',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.usp_ReseedMRN
	@ReseedValue Bigint,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS 
BEGIN

	BEGIN TRY
		DBCC CHECKIDENT ('Registration.ContactMRN', RESEED, @ReseedValue);
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH

END