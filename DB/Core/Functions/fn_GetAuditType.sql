-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[GetModuleID]
-- Author:		John Crossen
-- Date:		09/21/2015
--
-- Purpose:		Get ModuleID based on ModuleName
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/21/2015	John Crossen	Task# 2401 Initial Creation
-- 09/07/2016	Rahul Vats		Review the Function
----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [Core].[fn_GetAuditType]
	(@AuditType NVARCHAR(50))
	RETURNS INT
AS 
BEGIN
	DECLARE @AuditTypeID as INT
	SELECT @AuditTypeID=AuditTypeID FROM Reference.AuditType WHERE TypeDescription=@AuditType
	RETURN @AuditTypeID 
END
