-----------------------------------------------------------------------------------------------------------------------
-- Table:		Core.ContactConsents
-- Author:		Kyle Campbell
-- Date:		03/15/2016
--
-- Purpose:		Tracks Consents and Appropriate Signatures
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/15/2016	Kyle Campbell	TFS #7237	Initial Creation
-- 04/08/2016	Scott Martin	Moved to Registration Schema, changed name to ContactConsent, Added ExpirationReason, Added AssessmentID
-- 04/09/2016	Scott Martin	Removed ExpirationReasonID and ExpiredBy
-- 04/30/2016	Gurpreet Singh	Update datatype from Date to DateTime for EffectiveDate, ExpirationDate, DateSigned
-- 06/01/2016	Scott Martin	Added ExpirationReasonID and ExpiredBy
-- 06/02/2016	Gurpreet Singh	Added ExpiredResponseID
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactConsent]
(
	[ContactConsentID] BIGINT NOT NULL IDENTITY(1,1),
	[ContactID] BIGINT NOT NULL,
	[AssessmentID] BIGINT NOT NULL,
	[AssessmentSectionID] BIGINT NOT NULL,
	[ResponseID] BIGINT NOT NULL,
	[EffectiveDate] DATETIME NOT NULL,
	[ExpirationDate] DATETIME NULL,
	[ExpirationReasonID] INT NULL,
	[ExpiredResponseID] BIGINT NULL,
	[ExpiredBy] INT NULL,
	[DateSigned] DATETIME NULL,
	[SignatureStatusID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT (1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT (1),
	[CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),	
	CONSTRAINT [PK_ContactConsents] PRIMARY KEY CLUSTERED ([ContactConsentID] ASC) 
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_ContactID] FOREIGN KEY (ContactID) REFERENCES Registration.Contact (ContactID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_ResponseID] FOREIGN KEY (ResponseID) REFERENCES Core.AssessmentResponses (ResponseID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_AssessmentID] FOREIGN KEY (AssessmentID) REFERENCES Core.Assessments (AssessmentID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_AssessmentSectionID] FOREIGN KEY (AssessmentSectionID) REFERENCES Core.AssessmentSections (AssessmentSectionID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_SignatureStatusID] FOREIGN KEY (SignatureStatusID) REFERENCES Reference.SignatureStatus (SignatureStatusID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_ExpirationReasonID] FOREIGN KEY (ExpirationReasonID) REFERENCES Reference.AssessmentExpirationReason (AssessmentExpirationReasonID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_ExpiredBy] FOREIGN KEY (ExpiredBy) REFERENCES Core.Users (UserID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_UserModifiedBy] FOREIGN KEY (ModifiedBy) REFERENCES Core.Users (UserID)
GO
ALTER TABLE Registration.ContactConsent WITH CHECK ADD CONSTRAINT [FK_ContactConsent_UserCreatedBy] FOREIGN KEY (CreatedBy) REFERENCES Core.Users (UserID)
GO
ALTER TABLE Registration.ContactConsent  WITH CHECK ADD  CONSTRAINT [FK_ContactConsent_ExpiredResponseID] FOREIGN KEY(ExpiredResponseID) REFERENCES Core.AssessmentResponses (ResponseID)
GO




EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References ContactID from Registration.ContactID', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactConsent,
@level2type = N'COLUMN', @level2name = ContactID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References ResponseID from Core.AssessmentResponses', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactConsent,
@level2type = N'COLUMN', @level2name = ResponseID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Date the consent was signed', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactConsent,
@level2type = N'COLUMN', @level2name = DateSigned;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References SignatureStatusID from Reference.SignatureStatus - Indictates if a consent was signed', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactConsent,
@level2type = N'COLUMN', @level2name = SignatureStatusID;
GO


EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References Assessments from Core.Assessments - Indictates what consent was used', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = ContactConsent,
@level2type = N'COLUMN', @level2name = AssessmentID;
GO
