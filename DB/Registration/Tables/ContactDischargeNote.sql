-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ContactDischargeNote]
-- Author:		Scott Martin
-- Date:		03/24/2016
--
-- Purpose:		Store Contact Discharge Notes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/24/2016	Scott Martin		Initial creation.
-- 06/06/2016	Scott Martin		Changed NoteHeaderID to NOT NULL
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactDischargeNote](
	[ContactDischargeNoteID] BIGINT NOT NULL IDENTITY(1,1),
	[NoteHeaderID] BIGINT NOT NULL,
	[ContactAdmissionID] BIGINT NULL,
	[DischargeReasonID] INT NOT NULL,
	[DischargeDate] DATETIME NOT NULL,
	[SignatureStatusID] INT NOT NULL DEFAULT(1),
	[NoteText] NVARCHAR(MAX) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
  
    CONSTRAINT [PK_ContactDischargeNote] PRIMARY KEY CLUSTERED 
(
	[ContactDischargeNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
) ON [PRIMARY]

GO

ALTER TABLE Registration.ContactDischargeNote WITH CHECK ADD CONSTRAINT [FK_ContactDischargeNote_ContactAdmissionID] FOREIGN KEY([ContactAdmissionID]) REFERENCES [Registration].[ContactAdmission] ([ContactAdmissionID])
GO
ALTER TABLE Registration.ContactDischargeNote CHECK CONSTRAINT [FK_ContactDischargeNote_ContactAdmissionID]
GO
ALTER TABLE Registration.ContactDischargeNote WITH CHECK ADD CONSTRAINT [FK_ContactDischargeNote_DischargeReasonID] FOREIGN KEY([DischargeReasonID]) REFERENCES [Reference].[DischargeReason] ([DischargeReasonID])
GO
ALTER TABLE Registration.ContactDischargeNote CHECK CONSTRAINT [FK_ContactDischargeNote_DischargeReasonID]
GO
ALTER TABLE Registration.ContactDischargeNote WITH CHECK ADD CONSTRAINT [FK_ContactDischargeNote_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactDischargeNote CHECK CONSTRAINT [FK_ContactDischargeNote_UserModifedBy]
GO
ALTER TABLE Registration.ContactDischargeNote WITH CHECK ADD CONSTRAINT [FK_ContactDischargeNote_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactDischargeNote CHECK CONSTRAINT [FK_ContactDischargeNote_UserCreatedBy]
GO
ALTER TABLE Registration.ContactDischargeNote WITH CHECK ADD CONSTRAINT [FK_ContactDischargeNote_NoteHeaderID] FOREIGN KEY(NoteHeaderID) REFERENCES Registration.NoteHeader (NoteHeaderID)
GO
ALTER TABLE Registration.ContactDischargeNote CHECK CONSTRAINT [FK_ContactDischargeNote_NoteHeaderID]
GO
ALTER TABLE Registration.ContactDischargeNote WITH CHECK ADD CONSTRAINT [FK_ContactDischargeNote_SignatureStatusID] FOREIGN KEY (SignatureStatusID) REFERENCES Reference.SignatureStatus (SignatureStatusID)
GO