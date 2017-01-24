-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_SaveRecordHeaderDetails]
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
-- 01/15/2017	Sumana Sangapu	Corrected the audit proc parameters
-- 01/16/2017	Sumana Sangapu	Added County and StateProvince in the Update
-- 01/20/2017	Sumana Sangapu	Added IsActive to Registration.ContactAddress and Registration.AdditionalDemographics
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveRecordHeaderDetails]
	@RecordHeaderID			BIGINT,
	@WorkflowID				BIGINT,
	@RecordPrimaryKeyValue	BIGINT,
	@ContactID				BIGINT,
	@ModifiedOn				DATETIME,
	@ModifiedBy				INT,
	@ResultCode				INT OUTPUT,
	@ResultMessage			NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID	BIGINT,
			@ProcName		VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID				BIGINT

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

			
			EXEC Core.usp_OpenEncryptionKeys @ResultCode OUTPUT, @ResultMessage OUTPUT
		
			IF NOT EXISTS (SELECT 'X' FROM Core.RecordHeaderDetails rhd WHERE RecordHeaderID = @RecordHeaderID AND ContactID = @ContactID) 
						   
			BEGIN
						INSERT INTO Core.RecordHeaderDetails
						(RecordHeaderID, ContactID, MRN, FirstName, Middle, LastName, SuffixID, DOB, 
						 IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn)
						SELECT		@RecordHeaderID, c.ContactID as ContactID, MRN, FirstName, Middle,LastName, SuffixID, DOB,
									'1' as IsActive, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn 
						FROM		Registration.Contact c
						WHERE		c.ContactID = @ContactID

						SET @ID = SCOPE_IDENTITY()

						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'RecordHeaderDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

						EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'RecordHeaderDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
			ELSE 
			BEGIN
						SELECT @ID = RecordHeaderDetailID FROM Core.RecordHeaderDetails rhd WHERE RecordHeaderID = @RecordHeaderID AND ContactID = @ContactID
						
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RecordHeaderDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

						UPDATE	rhd
						SET		MRN			= c.MRN, 
								FirstName	= C.FirstName, 
								LastName	= C.LastName, 
								Middle		= C.Middle, 
								SuffixID	= c.SuffixID, 
								DOB			= c.DOB 
						FROM		Core.RecordHeaderDetails rhd
						INNER JOIN	Registration.Contact c
						ON			rhd.ContactID = c.ContactID
						WHERE		rhd.RecordHeaderID = @RecordHeaderID
						AND			rhd.ContactID = @ContactID


			END 

			-- Update MedicaidID 
			UPDATE	rhd
			SET		MedicaidID =   (	SELECT	TOP 1 ISNULL(CP.PolicyID,'N/A') 
										FROM	Registration.ContactPayor CP
										INNER JOIN Reference.Payor P
										ON		CP.PayorID = P.PayorID
										WHERE	PayorName LIKE '%medicaid%'
										AND		CP.IsActive = 1
										AND		ContactID = @ContactID
										ORDER BY CP.CreatedOn DESC 
									) 
			FROM	Core.RecordHeaderDetails  rhd
			WHERE	rhd.RecordHeaderID = @RecordHeaderID
			AND		rhd.ContactID = @ContactID

			-- If @WorkflowID is 'CrisisLine' or 'LawLiaison' then update the related fields
			IF @WorkflowID IN (4,5)
			BEGIN
						-- Update Demo fields
						UPDATE		rhd
						SET			SSNEncrypted = C.SSNEncrypted, 
									Age		     = DATEDIFF(YY,c.DOB,GETDATE()), 
									EthnicityID  = Ad.EthnicityID, 
									RaceID       = cr.RaceID,
									PhoneTypeID	 = p.PhoneTypeID,
									Number	     = p.Number,
									Extension    = p.Extension, 
									AddressTypeID = a.AddressTypeID,
									Line1        = a.Line1,
									Line2	     = a.Line2, 
									City	     = a.City, 
									Zip		     = a.Zip, 
									ComplexName  = a.ComplexName, 
									GateCode     = a.GateCode,
									StateProvince = a.StateProvince,
									County		  = a.County
						FROM		Core.RecordHeaderDetails rhd
						INNER JOIN	Registration.Contact c
						ON			rhd.ContactID = c.ContactID
						LEFT JOIN	Registration.AdditionalDemographics ad
						ON			c.ContactID = ad.ContactID
						AND			ad.IsActive = 1 
						LEFT JOIN   (SELECT  cr1.RaceID, cr1.ContactID FROM  ( 
																				SELECT  Stuff(( SELECT   ', ' + convert(nvarchar(10),RaceID) 
																								FROM Registration.ContactRace cr 
																								WHERE Contactid = @ContactID FOR XML PATH('')), 1, 2, '') as  RaceID, @ContactID as ContactID )cr1  
									)cr
						ON			c.ContactID = cr.ContactID
						LEFT JOIN	Registration.ContactPhone cp
						ON			c.ContactID = cp.ContactID
						AND			IsPrimary = 1 
						LEFT JOIN	Core.Phone p
						ON			cp.PhoneID = p.PhoneID 
						LEFT JOIN	Registration.ContactAddress ca
						ON			c.ContactID = ca.ContactID
						AND			ca.IsPrimary = 1 
						LEFT JOIN	Core.Addresses a
						ON			ca.AddressID = a.AddressID
						WHERE		rhd.RecordHeaderID = @RecordHeaderID
						AND			rhd.ContactID = @ContactID

						-- Update CallCenterHeaderDetails 
						UPDATE		rhd
						SET			SourceHeaderID = @RecordPrimaryKeyValue,
									OrganizationMappingID = (SELECT ProgramUnitID 
															 FROM	CallCenter.CallCenterHeader cch
			  												 WHERE	cch.CallCenterHeaderID = @RecordPrimaryKeyValue )
						FROM		Core.RecordHeaderDetails rhd
						WHERE		rhd.RecordHeaderID = @RecordHeaderID
			END 

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RecordHeaderDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT  @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END