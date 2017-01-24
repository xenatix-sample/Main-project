-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetHPIDetail]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get HPI Detail Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/8/2015	Scott Martin	Added 2 new columns
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetHPIDetail]
	@HPIID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[HPIDetailID],
		[HPIID],
		[Comment],
		[Location],
		[HPISeverityID],
		[Quality],
		[Duration],
		[Timing],
		[Context],
		[ModifyingFactors],
		[Symptoms],
		[Conditions],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[HPIDetail]
	WHERE
		HPIID = @HPIID
		AND IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


