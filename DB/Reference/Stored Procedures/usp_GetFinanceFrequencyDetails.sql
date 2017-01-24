
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFinanceFrequencyDetails]
-- Author:		Sumana Sangapu
-- Date:		08/07/2015
--
-- Purpose:		Gets the list of Frequency lookup details for Financial Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/07/2015   Sumana Sangapu Tas # 634 Initial Creation
-- 08/18/2015	Suresh Pandey	Add FinanceFrequencyType column in select statement
-- 09/14/2015	Suresh Pandey	Add FrequencyFactor column in select statement
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetFinanceFrequencyDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		FinanceFrequencyID, FinanceFrequency,FrequencyFactor
		FROM		[Reference].[FinanceFrequency] 
		WHERE		IsActive = 1
		ORDER BY	FinanceFrequency  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
