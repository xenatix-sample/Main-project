
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetScreeningName]
-- Author:		Sumana Sangapu
-- Date:		10/07/2015
--
-- Purpose:		Lookup for Screening Name
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015	Sumana Sangapu	TFS# 2620 - Initial creation.
-- 10/12/2015	Sumana Sangapu	TFS# 2620 - Return Screening names as assessment names from Assessments table 
-- 02/23/2016   Satish Singh    TFS#6421  - Null condition to avoid error while assocition with screenign type.
 -----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetScreeningName]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = OBJECT_NAME(@@PROCID) + ' executed successfully'

	BEGIN TRY	

				SELECT  
					a.AssessmentID as ScreeningNameID,
					a.Name as ScreeningName, 
					dm.ScreeningTypeID as ScreeningTypeID
				FROM	
					[Core].[Assessments] a 
					LEFT JOIN [Core].[DocumentMapping] dm
					ON a.AssessmentID = dm.AssessmentID
				WHERE dm.DocumentTypeID = 2 AND dm.ScreeningTypeID IS NOT NULL --Screening 

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END