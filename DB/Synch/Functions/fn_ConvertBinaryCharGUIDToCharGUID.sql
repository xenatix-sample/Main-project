-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[fn_ConvertBinaryCharGUIDToCharGUID]
-- Author:		Rahul Vats
-- Date:		09/02/2016
--
-- Purpose:		ConvertBinaryCharGUIDToCharGUID
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/02/2016	Rahul Vats	Task# 14263 Initial Creation
----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [Synch].[fn_ConvertBinaryCharGUIDToCharGUID] (@BinaryGUID varchar(100))
RETURNS VARCHAR(100)
AS
BEGIN
        
     DECLARE @CharGUID VARCHAR(100)
	 Set @CharGUID = Convert(varchar(100),CONVERT(varbinary(100),@BinaryGUID),2)
     RETURN @CharGUID  
END
GO