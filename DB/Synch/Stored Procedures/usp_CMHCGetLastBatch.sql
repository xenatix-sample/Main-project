-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.uspCMHCGetLastBatch
-- Author:		Sumana Sangapu
-- Date:		09/13/2016
--
-- Purpose:		Get the last run datetime from Batch for that BatchType
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/13/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------
Create PROCEDURE Synch.uspCMHCGetLastBatch
@BatchTypeID BIGINT
AS
BEGIN

		SELECT MAX(SystemModifiedOn) FROM Synch.Batch 
		WHERE BatchTypeID=@BatchTypeID

END