-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_LogException]
-- Author:		Arun Choudhary
-- Date:		07/23/2015
--
-- Purpose:		This will log any unhandled exceptions to the exception table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Justin Spalti - Added the procedure header, dbo to the table, added the ModifiedBy parameter, and included the value in crud operations
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/04/2015 - John Crossen -- Data type mismatches resolved
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_LogException]
	@Message NVARCHAR(1000),
	@Source NVARCHAR(255),
	@Comments NVARCHAR(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		INSERT INTO Core.Exception([Message], [Source], Comments, ModifiedOn, ModifiedBy, CreatedBy, CreatedOn)
		VALUES(@Message, @Source, @Comments, @ModifiedOn, @ModifiedBy, @ModifiedBy, @ModifiedOn)
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
