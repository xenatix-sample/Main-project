

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetRecordHeaderDetails]
-- Author:		Sumana 
-- Date:		07/23/2015
--
-- Purpose:		Save Print Record Header snapshot details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2017	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRecordHeaderDetails]
	@WorkflowDataKey nvarchar(250),
	@RecordPrimaryKeyValue  BIGINT,
	@ResultCode		INT OUTPUT,
	@ResultMessage	NVARCHAR(500) OUTPUT
AS
BEGIN

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		DECLARE @RaceID nvarchar(500),
				@Race nvarchar(500),
				@WorkflowID BIGINT

		-- Fetch WorkflowID from WorkflowDataKey
		SELECT		@WorkflowID = w.WorkflowID 
		FROM		Core.Workflow w 
		INNER JOIN	Core.WorkflowComponentMapping wcm
		ON			w.WorkflowID	= wcm.WorkflowID
		INNER JOIN	Core.ModuleComponent mc
		ON			wcm.ModuleComponentID = mc.ModuleComponentID
		WHERE		mc.DataKey = @WorkflowDataKey 

		-----------------------------------------------------------------------------------
											
	    EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT

		SELECT	RecordHeaderDetailID, rh.RecordHeaderID as RecordHeaderID, ContactID, MRN, FirstName, Middle, LastName, s.Suffix, DOB, MedicaidID, 
				CONVERT(NVARCHAR(9), Core.fn_Decrypt(rhd.SSNEncrypted)) as SSN, 
				OrganizationMappingID as ProgramUnitID, SourceHeaderID as IncidentID, Age, 
				Stuff(( SELECT   ', ' + convert(nvarchar(500),Race) 
								FROM Reference.Race r WHERE r.RaceID IN (SELECT Items FROM [Core].[fn_Split](rhd.RaceID,','))
								FOR XML PATH('')), 1, 2, '') as Race, 
				Ethnicity, Number, Extension,  
				Line1 , Line2, City, sp.StateProvinceCode, Zip, County, 
				ComplexName, GateCode, PhoneTypeID, AddressTypeID
		FROM	Core.RecordHeader rh
		INNER JOIN 	Core.RecordHeaderDetails rhd
		ON		rh.RecordHeaderID = rhd.RecordHeaderID
		LEFT JOIN Reference.StateProvince sp
		ON		rhd.StateProvince = sp.StateProvinceID 
		LEFT JOIN Reference.Ethnicity e
		ON		rhd.EthnicityID = e.EthnicityID
		LEFT JOIN Reference.Suffix s
		ON		rhd.SuffixID = s.SuffixID
		WHERE   WorkflowID  = @WorkflowID
		AND		RecordPrimaryKeyValue = @RecordPrimaryKeyValue  

	END TRY

	BEGIN CATCH
		SELECT  @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END