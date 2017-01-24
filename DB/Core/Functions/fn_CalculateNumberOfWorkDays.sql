-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[fn_CalculateNumberOFWorkDays]
-- Author:		Sumana Sangapu
-- Date:		08/12/2015
--
-- Purpose:		Calculate the Number of business days between given two dates
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	Sumana Sangapu	Task# 634 Initial Creation
----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION [Core].[fn_CalculateNumberOFWorkDays] (@StartDate date, @EndDate date)
RETURNS int
AS
BEGIN
     
        
     DECLARE @WORKDAYS INT
     SELECT @WORKDAYS = (DATEDIFF(dd, @StartDate, @EndDate) + 1)
	               -(DATEDIFF(wk, @StartDate, @EndDate) * 2)
   		       -(CASE WHEN DATENAME(dw, @StartDate) = CONVERT(nvarchar,'Sunday') THEN 1 ELSE 0 END)
		       -(CASE WHEN DATENAME(dw, @EndDate) = CONVERT(nvarchar,'Saturday') THEN 1 ELSE 0 END)
  
     RETURN @WORKDAYS  
END
GO


