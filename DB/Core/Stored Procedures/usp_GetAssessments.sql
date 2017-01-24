
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessments]
-- Author:		Sumana Sangapu
-- Date:		09/17/2015
--
-- Purpose:		Get the assessments
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	-	Sumana Sangapu	- Task 1601	- Initial Creation
-- 01/15/2016	-	Rajiv Ranjan	- Task 5307	- Refactor proc, allowed null into @AssessmentID for getting all data
-- 04/08/2016		Scott Martin	Added new tables and fields for expanded functionality
-- 04/26/2016	-	Kyle Campbell	Added new "UseSnapshot" field to get proc
-- 09/06/2016	-	Gurpreet Singh  - Task 14279 - Added IsActive in select query
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessments]
	@AssessmentID BIGINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 
AS
BEGIN
	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'
		
	BEGIN TRY
	SELECT  
		a.AssessmentID,
		a.Name as AssessmentName, 
		a.VersionID, 
		af.FrequencyID,
		af.Frequency, 
		P.ProgramID,
		P.ProgramName,
		i.ImageID,
		i.ImageBLOB AS ImageContent,
		A.Abbreviation,
		A.IsSignatureRequired,
		A.SignatureTypeID,
		A.ExpirationAssessmentID,
		AOD.DetailID AS OrganizationDetailID,
		DM.DocumentTypeID,
		A.UseSnapshot,
		A.IsActive
	FROM	
		[Core].[Assessments] A 
		LEFT OUTER JOIN [Reference].[AssessmentFrequency] AF
			ON AF.FrequencyID = A.FrequencyID
		LEFT OUTER JOIN [Core].[Images] I
			ON I.ImageID = A.ImageID
		LEFT OUTER JOIN Core.AssessmentOrganizationDetails AOD
			ON A.AssessmentID = AOD.AssessmentID
		LEFT OUTER JOIN Core.OrganizationDetails OD
			ON AOD.DetailID = OD.DetailID
		LEFT OUTER JOIN Core.DocumentMapping DM
			ON A.AssessmentID = DM.AssessmentID
		LEFT OUTER JOIN Reference.Program P 
			ON A.ProgramID = P.ProgramID 
	WHERE	
		(ISNULL(@AssessmentID,0) = 0 OR A.AssessmentID = @AssessmentID)

	END TRY
	BEGIN CATCH
			SELECT  @ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
