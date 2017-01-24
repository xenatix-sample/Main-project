-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE [ESignature].[ContactSignatures](
	[ContactSignatureID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[SignatureID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ContactSignatures_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactSignatures] PRIMARY KEY CLUSTERED 
(
	[ContactSignatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ESignature].[ContactSignatures]  WITH CHECK ADD  CONSTRAINT [FK_ContactSignatures_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [ESignature].[ContactSignatures] CHECK CONSTRAINT [FK_ContactSignatures_ContactID]
GO

ALTER TABLE [ESignature].[ContactSignatures]  WITH CHECK ADD  CONSTRAINT [FK_ContactSignatures_SignatureID] FOREIGN KEY([SignatureID])
REFERENCES [ESignature].[Signatures] ([SignatureID])
GO

ALTER TABLE [ESignature].[ContactSignatures] CHECK CONSTRAINT [FK_ContactSignatures_SignatureID]
GO

ALTER TABLE ESignature.ContactSignatures WITH CHECK ADD CONSTRAINT [FK_ContactSignatures_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.ContactSignatures CHECK CONSTRAINT [FK_ContactSignatures_UserModifedBy]
GO
ALTER TABLE ESignature.ContactSignatures WITH CHECK ADD CONSTRAINT [FK_ContactSignatures_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.ContactSignatures CHECK CONSTRAINT [FK_ContactSignatures_UserCreatedBy]
GO
