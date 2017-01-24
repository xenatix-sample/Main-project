-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ECI].[ScreeningEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Holds the mapping of Entity Signatures for the Screenings
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
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ScreeningEntitySignatures](
	[SESID] [bigint] IDENTITY(1,1) NOT NULL,
	[ScreeningID] [bigint] NOT NULL,
	[EntitySignatureID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ScreeningEntitySignatures_IsActive]  DEFAULT ((1)),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
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

ALTER TABLE [ECI].[ScreeningEntitySignatures] WITH CHECK ADD  CONSTRAINT [FK_ScreeningEntitySignatures_SignatureID] FOREIGN KEY([EntitySignatureID])
REFERENCES [ESignature].[EntitySignatures] ([EntitySignatureID])
GO

ALTER TABLE [ECI].[ScreeningEntitySignatures] CHECK CONSTRAINT [FK_ScreeningEntitySignatures_SignatureID]
GO

ALTER TABLE [ECI].[ScreeningEntitySignatures]  WITH CHECK ADD  CONSTRAINT [FK_EntitySignatures_ScreeningID] FOREIGN KEY([ScreeningID])
REFERENCES [ECI].[Screening]  ([ScreeningID])
GO

ALTER TABLE [ECI].[ScreeningEntitySignatures] CHECK CONSTRAINT [FK_EntitySignatures_ScreeningID]
GO

ALTER TABLE ECI.ScreeningEntitySignatures WITH CHECK ADD CONSTRAINT [FK_ScreeningEntitySignatures_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningEntitySignatures CHECK CONSTRAINT [FK_ScreeningEntitySignatures_UserModifedBy]
GO
ALTER TABLE ECI.ScreeningEntitySignatures WITH CHECK ADD CONSTRAINT [FK_ScreeningEntitySignatures_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningEntitySignatures CHECK CONSTRAINT [FK_ScreeningEntitySignatures_UserCreatedBy]
GO
