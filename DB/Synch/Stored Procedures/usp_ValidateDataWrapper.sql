
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_ValidateDataWrapper 
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Wrapper procedure to invoke all the Datavaliation procs for lookups for DCV
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Synch.usp_ValidateDataWrapper AS 
BEGIN
			
			/******************Truncate the tables **********************************/

			TRUNCATE TABLE [Synch].[AdditionalDemographics_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[Address_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[ContactAddress_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[ContactClientIdentifier_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[Contact_Phone_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[Contactpayor_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[Contact_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[ContactPresentingProblem_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[ContactRace_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[Phone_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[ContactRelationship_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[FinancialAssessment_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[FinancialAssessmentDetail_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[Users_staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[UserCredential_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[UserRole_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[UserUIds_Staging_ErrorDetails]

			TRUNCATE TABLE Synch.ECIAdditionalDemographics_Staging_Error_Details

			TRUNCATE TABLE [Synch].[ContactDischargeNote_Staging_ErrorDetails]

			TRUNCATE TABLE [Synch].[NoteHeader_Staging_Error_Details]

			TRUNCATE TABLE [Synch].[ContactRelationshipType_Staging_Error_Details]

			/*************************** EXEC PROCS ***************************************************/

			EXEC [Synch].[usp_ValidateDataAdditionalDemographics_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataAddress_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContactAddress_Staging]

			EXEC [Synch].[usp_ValidateDataContactClientIdentifier_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContact_Phone_Staging]

			EXEC [Synch].[usp_ValidateDataContactpayor_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContact_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContactPresentingProblem_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContactRace_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataPhone_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContactRelationship_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataFinancialAssessment_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataFinancialAssessmentDetail_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataUsers_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataUserCredential_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataUserRole_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataUserUIds_Staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataNoteHeader_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataNoteHeader_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataContactDischargeNote_staging_ErrorDetails]

			EXEC [Synch].[usp_ValidateDataECIAdditionalDemo_staging_ErrorDetails]
END