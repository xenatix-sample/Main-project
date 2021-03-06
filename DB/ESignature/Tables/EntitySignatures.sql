-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ESignature].[EntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Holds Entity Signatures for the Screenings
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--				
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	Sumana Sangapu	TFS: 2664  - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 04/11/2016	Scott Martin	Made SignatureID nullable
-- 05/23/2016	Gurmant Singh	Add the EntityName Column to store the Name of the entity for signatures
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ESignature].[EntitySignatures](
	[EntitySignatureID] [bigint] IDENTITY(1,1) NOT NULL,
	[EntityID] [bigint] NOT NULL,
	[EntityName] NVARCHAR(300) NULL,
	[SignatureID] BIGINT NULL,
	[EntityTypeID] [int] NULL,
	[SignatureTypeID] INT NULL,
	[CredentialID] BIGINT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_EntitySignatures_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL ,
	[ModifiedOn] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_EntitySignatures] PRIMARY KEY CLUSTERED 
(
	[EntitySignatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ESignature].[EntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_EntitySignatures_EntityTypeID] FOREIGN KEY([EntityTypeID])
REFERENCES [Reference].[EntityType] ([EntityTypeID])
GO

ALTER TABLE [ESignature].[EntitySignatures] CHECK CONSTRAINT [FK_EntitySignatures_EntityTypeID]
GO

ALTER TABLE [ESignature].[EntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_EntitySignatures_SignatureID] FOREIGN KEY([SignatureID])
REFERENCES [ESignature].[Signatures] ([SignatureID])
GO

ALTER TABLE [ESignature].[EntitySignatures] CHECK CONSTRAINT [FK_EntitySignatures_SignatureID]
GO

ALTER TABLE [ESignature].[EntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_EntitySignatures_SignatureTypeID] FOREIGN KEY([SignatureTypeID])
REFERENCES [ESignature].[SignatureTypes] ([SignatureTypeID])
GO

ALTER TABLE [ESignature].[EntitySignatures] CHECK CONSTRAINT [FK_EntitySignatures_SignatureTypeID]
GO

ALTER TABLE [ESignature].[EntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_EntitySignatures_CredentialID] FOREIGN KEY([CredentialID])
REFERENCES [Reference].[Credentials] ([CredentialID])
GO

ALTER TABLE [ESignature].[EntitySignatures] CHECK CONSTRAINT [FK_EntitySignatures_CredentialID]
GO

ALTER TABLE ESignature.EntitySignatures WITH CHECK ADD CONSTRAINT [FK_EntitySignatures_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.EntitySignatures CHECK CONSTRAINT [FK_EntitySignatures_UserModifedBy]
GO
ALTER TABLE ESignature.EntitySignatures WITH CHECK ADD CONSTRAINT [FK_EntitySignatures_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.EntitySignatures CHECK CONSTRAINT [FK_EntitySignatures_UserCreatedBy]
GO

