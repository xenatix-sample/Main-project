


-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetProgramUnitsByProgram]
-- Author:		John Crossen
-- Date:		02/01/2016
--
-- Purpose:		Get Program Units by Program
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/01/2016	John Crossen   5200	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Core.usp_GetProgramUnitsByProgram
	@ProgramName NVARCHAR(255),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

	AS
BEGIN
	DECLARE @AuditPost XML,
		@AuditID BIGINT;

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	SELECT D.DetailID, D.Name FROM Core.OrganizationDetails D JOIN
	(
	SELECT DM.DetailID FROM [Core].[OrganizationAttributes] OA
	JOIN [Core].[OrganizationAttributesMapping] OM ON OM.AttributeID = OA.AttributeID
	JOIN [Core].[OrganizationDetails] OD ON OD.DetailID=OM.DetailID
    JOIN [Core].OrganizationDetailsMapping DM ON OD.DetailID=DM.ParentID
	WHERE OA.DataKey='Program' AND OD.Name=@ProgramName) Detail ON Detail.DetailID = D.DetailID
	

		END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
