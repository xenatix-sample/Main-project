
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[CoSignatures]
-- Author:		Sumana Sangapu
-- Date:		04/06/2016
--
-- Purpose:		Stores the Cosignatures for the User
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[CoSignatures] (
	[CoSignatureID]	BIGINT IDENTITY(1,1) NOT NULL,
    [UserID]   INT   NOT NULL,
	[CoSigneeID] INT NOT NULL,
	[DocumentTypeGroupID] INT NOT NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_CoSignature_CoSignatureID] PRIMARY KEY CLUSTERED ([CoSignatureID] ASC),
    CONSTRAINT [FK_CoSignature_UserID] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_CoSignature_CoSigneeID] FOREIGN KEY ([CoSigneeID]) REFERENCES [Core].[Users] ([UserID]),
    CONSTRAINT [FK_CoSignature_DocumentTypeGroupID]	FOREIGN KEY ([DocumentTypeGroupID]) REFERENCES [Reference].[DocumentTypeGroup] ([DocumentTypeGroupID])
);
GO

ALTER TABLE Core.CoSignatures WITH CHECK ADD CONSTRAINT [FK_CoSignatures_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.CoSignatures CHECK CONSTRAINT [FK_CoSignatures_UserModifedBy]
GO
ALTER TABLE Core.CoSignatures WITH CHECK ADD CONSTRAINT [FK_CoSignatures_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.CoSignatures CHECK CONSTRAINT [FK_CoSignatures_UserCreatedBy]
GO