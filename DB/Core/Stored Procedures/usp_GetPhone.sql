-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPhone]
-- Author:		Saurabh Sahu
-- Date:		08/10/2015
--
-- Purpose:		Get Phone Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2015 - Saurabh Sahu - Initial draft
----------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [Core].[usp_GetPhone]
	@PhoneID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT 
			p.PhoneID, 
			pt.PhoneTypeID, 
			p.Number, 
			p.Extension, 
			p.IsActive
		FROM 
			[Core].[Phone] p
			LEFT OUTER JOIN Reference.PhoneType pt ON p.PhoneTypeID = pt.PhoneTypeID
		WHERE 
			p.PhoneID = @PhoneID

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END