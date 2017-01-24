-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ESignature].[DocumentEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		10/16/2015
--
-- Purpose:		Holds data for DocumentEntitySignatures
--
-- Notes:		ResponseDetailsID will store the primary key of the table which needs to link to a signarture (notes, service recording, call center, etc)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	Sumana Sangapu	TFS# 2664 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/22/2016	Sumana Sangapu	Replaced DocumentID to ResponseDetailsID from Core.AssessmentResponseDetails
-- 03/31/2016	Scott Martin	Removed foreign key to Core.AssessmentResponseDetails to allow for linking to other Items
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ESignature].[DocumentEntitySignatures](
	[DocumentEntitySignatureID] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentID] [bigint] NOT NULL,
	[EntitySignatureID] [bigint] NULL,
	[DocumentTypeID] [int]  NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_DocumentEntitySignatures] PRIMARY KEY CLUSTERED 
(
	[DocumentEntitySignatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ESignature].[DocumentEntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_DocumentEntitySignatures_EntitySignatureID] FOREIGN KEY([EntitySignatureID])
REFERENCES [ESignature].[EntitySignatures] ([EntitySignatureID])
GO

ALTER TABLE [ESignature].[DocumentEntitySignatures]  CHECK CONSTRAINT [FK_DocumentEntitySignatures_EntitySignatureID]
GO

ALTER TABLE [ESignature].[DocumentEntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_DocumentEntitySignatures_DocumentTypeID] FOREIGN KEY([DocumentTypeID])
REFERENCES [Reference].[DocumentType] ([DocumentTypeID])
GO

ALTER TABLE [ESignature].[DocumentEntitySignatures]  CHECK CONSTRAINT [FK_DocumentEntitySignatures_DocumentTypeID]
GO

ALTER TABLE ESignature.DocumentEntitySignatures WITH CHECK ADD CONSTRAINT [FK_DocumentEntitySignatures_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.DocumentEntitySignatures CHECK CONSTRAINT [FK_DocumentEntitySignatures_UserModifedBy]
GO
ALTER TABLE ESignature.DocumentEntitySignatures WITH CHECK ADD CONSTRAINT [FK_DocumentEntitySignatures_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.DocumentEntitySignatures CHECK CONSTRAINT [FK_DocumentEntitySignatures_UserCreatedBy]
GO

CREATE NONCLUSTERED INDEX [IX_DocumentEntitySignatures_DocumentTypeID_DocumentID_EntitySignatureID] ON [ESignature].[DocumentEntitySignatures]
(
	[DocumentTypeID] ASC,
	[DocumentID] ASC,
	[EntitySignatureID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO