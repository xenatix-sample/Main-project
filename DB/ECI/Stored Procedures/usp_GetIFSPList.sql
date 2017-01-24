  -----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetIFSPList]
-- Author:		Gurpreet Singh
-- Date:		10/19/2015
--
-- Purpose:		To get ECI IFSP List for contact
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/19/2015   Gurpreet Singh		Initial Creation
-- 10/20/2015	Gurpreet Singh		Updated table/column name as Table/Column is renamed
-- 10/20/2015   John Crossen        Add Meeting Delayed
-- 10/26/2015	Gurpreet Singh		Added IsActive check
-- 10/27/2015	Gurpreet Singh		Added Comments field
-- 10/27/2015	Sumana Sangapu		Add AssessmentID & ResponseID
-- 10/30/2017   John Crossen        Add new link for ResponseID
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetIFSPList]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT
			ifsp.[IFSPID]
			,ifsp.[ContactID]
			,ifsp.[IFSPTypeID]
			,ifspType.[IFSPType]
			,ifsp.[IFSPMeetingDate]
			,ifsp.[IFSPFamilySignedDate]
			,ifsp.MeetingDelayed
			,ifsp.[ReasonForDelayID]
			,ifsp.[Comments]
			,ifsp.[AssessmentID]
			,ifsp.[ResponseID]
			,(select top 1 AssessmentSectionID from Core.AssessmentSections where AssessmentID = ifsp.AssessmentID and IsActive = 1 order by SortOrder asc) as SectionID
			,ifsp.[IsActive]
			,ifsp.[ModifiedBy]
			,ifsp.[ModifiedOn]
		FROM [ECI].[IFSP] ifsp
		LEFT JOIN [ECI].[IFSPType] ifspType ON ifsp.[IFSPTypeID] = ifspType.[IFSPTypeID]
		LEFT JOIN [Core].[AssessmentResponses] resp
		ON	 resp.ResponseID	= ifsp.ResponseID
		WHERE ifsp.[ContactID] = @ContactID AND ifsp.[IsActive] = 1
		ORDER BY ifsp.[ModifiedOn] DESC

	END TRY
	BEGIN CATCH
		SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
