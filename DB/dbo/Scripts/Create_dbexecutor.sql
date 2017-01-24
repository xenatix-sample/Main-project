-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Create DBexecutor Role
-- Author:		John Crossen
-- Date:		07/31/2015
--
-- Purpose:		Create role to execute stored procs.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/31/2015	John Crossen		TFS# 1066 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------



CREATE ROLE [db_executor]
GO
GRANT EXECUTE TO [db_executor]
