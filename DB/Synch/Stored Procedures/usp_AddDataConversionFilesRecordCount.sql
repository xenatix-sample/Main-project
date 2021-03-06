
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_AddDataConversionFilesRecordCount]
-- Author:		Sumana Sangapu
-- Date:		05/29/2016
--
-- Purpose:		Source and Destination Record count for the conversion files
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

 
 CREATE PROCEDURE [Synch].[usp_AddDataConversionFilesRecordCount]
 AS 
 BEGIN 
					--------------------------------------------------------
					TRUNCATE TABLE [Synch].[DataConversionFilesRecordCount]
					--------------------------------------------------------

					INSERT INTO [Synch].[DataConversionFilesRecordCount]
					(TableName, SourceCount, DestinationCount, CreatedOn )
					SELECT 'AdditionalDemographics',COUNT(*), (SELECT COUNT(*) FROM [Registration].[AdditionalDemographics]), GETUTCDATE() FROM [Synch].[AdditionalDemographics_Staging]
					UNION
					SELECT 'ContactAdmission',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactAdmission]), GETUTCDATE() FROM [Synch].[ContactAdmission_Staging]
					UNION
					SELECT 'Addresses',COUNT(*), (SELECT COUNT(*) FROM [Core].[Addresses]), GETUTCDATE() FROM [Synch].[Address_Staging]
					UNION
					SELECT 'ContactAddress',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactAddress] ), GETUTCDATE() FROM [Synch].[ContactAddress_Staging]
					UNION
					SELECT 'ContactClientIdentifier',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactClientIdentifier]), GETUTCDATE() FROM [Synch].[ContactClientIdentifier_Staging]
					UNION
					SELECT 'ContactPhone',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactPhone]), GETUTCDATE() FROM [Synch].[Contact_Phone_Staging]
					UNION
					SELECT 'ContactAlias',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactAlias]), GETUTCDATE() FROM [Synch].[ContactAlias_Staging]
					UNION
					SELECT 'Contactpayor',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactPayor]), GETUTCDATE() FROM [Synch].[Contactpayor_staging]
					UNION
					SELECT 'Contact',COUNT(*), (SELECT COUNT(*) FROM [Registration].[Contact]), GETUTCDATE() FROM [Synch].[Contact_Staging]
					UNION
					SELECT 'ContactPresentingProblem',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactPresentingProblem]), GETUTCDATE() FROM [Synch].[ContactPresentingProblem_staging]
					UNION
					SELECT 'ContactRace',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactRace]), GETUTCDATE() FROM [Synch].[ContactRace_staging]
					UNION
					SELECT 'Phone',COUNT(*), (SELECT COUNT(*) FROM [Core].[Phone]), GETUTCDATE() FROM [Synch].[Phone_staging]
					UNION
					SELECT 'StaffEmail',COUNT(*), (SELECT COUNT(*) FROM [Core].[Email]), GETUTCDATE() FROM [Synch].[StaffEmail_Staging]
					UNION
					SELECT 'ContactRelationship',COUNT(*), (SELECT COUNT(*) FROM [Registration].[ContactRelationship]), GETUTCDATE() FROM [Synch].[ContactRelationship_staging]
					UNION
					SELECT 'FinancialAssessment',COUNT(*), (SELECT COUNT(*) FROM [Registration].[FinancialAssessment]), GETUTCDATE() FROM [Synch].[FinancialAssessment_Staging]
					UNION
					SELECT 'FinancialAssessmentDetail',COUNT(*), (SELECT COUNT(*) FROM [Registration].[FinancialAssessmentDetails]), GETUTCDATE() FROM [Synch].[FinancialAssessmentDetail_staging]
					UNION
					SELECT 'SelfPay',COUNT(*), (SELECT COUNT(*) FROM [Registration].[SelfPay]), GETUTCDATE() FROM [Synch].[SelfPay_staging]
					UNION
					SELECT 'Users',COUNT(*), (SELECT COUNT(*) FROM [Core].[Users] ), GETUTCDATE() FROM [Synch].[Users_staging]
					UNION
					SELECT 'UserCredential',COUNT(*), (SELECT COUNT(*) FROM [Core].[UserCredential]), GETUTCDATE() FROM [Synch].[UserCredential_Staging]
					UNION
					SELECT 'UserRole',COUNT(*),  (SELECT COUNT(*) FROM [Core].[UserRole]), GETUTCDATE() FROM [Synch].[UserRole_Staging]
					UNION
					SELECT 'UserEmail',COUNT(*), (SELECT COUNT(*) FROM [Core].[UserEmail]), GETUTCDATE() FROM [Synch].[useremail_Staging]
					UNION
					SELECT 'UserPhone',COUNT(*), (SELECT COUNT(*) FROM [Core].[UserPhone]), GETUTCDATE() FROM [Synch].[UserPhone_Staging]
					UNION
					SELECT 'UserUIds',COUNT(*), (SELECT COUNT(*) FROM [Core].[UserIdentifierDetails]), GETUTCDATE() FROM [Synch].[UserUIds_Staging]
					UNION
					SELECT 'UserOrgMapping',COUNT(*), (SELECT COUNT(*) FROM [Core].[UserOrganizationDetailsMapping]), GETUTCDATE() FROM [Synch].[UserOrgMapping_Staging]
					UNION
					SELECT 'ECIAdditionalDemo',COUNT(*), (SELECT COUNT(*) FROM [ECI].[AdditionalDemographics]), GETUTCDATE() FROM [Synch].[ECIAdditionalDemographics_Staging]
					UNION
					SELECT 'ContactRelationshipType',COUNT(*), (SELECT COUNT(*) FROM Registration.ContactRelationshipType), GETUTCDATE() FROM Synch.ContactRelationshipType_Staging
					UNION
					SELECT 'NoteHeader',COUNT(*), (SELECT COUNT(*) FROM Registration.NoteHeader), GETUTCDATE() FROM Synch.NoteHeader_staging
					UNION
					SELECT 'ContactDischargeNote',COUNT(*), (SELECT COUNT(*) FROM Registration.NoteHeader), GETUTCDATE() FROM Synch.ContactDischargeNote_staging


END