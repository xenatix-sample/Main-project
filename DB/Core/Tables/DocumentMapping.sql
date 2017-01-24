-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[DocumentMapping]
-- Author:		Sumana Sangapu
-- Date:		10/29/2015
--
-- Purpose:		Table creates the mapping between Assessment , Documenttype and screeningtype 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/29/2015   Sumana Sangapu  3136 - Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[DocumentMapping](
	[DocumentMappingID] [int] IDENTITY (1,1) NOT NULL,
	[AssessmentID] [bigint] NULL,
	[DocumentTypeID] [int] NULL,
	[ScreeningTypeID] [smallint] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_DocumentMappingID] PRIMARY KEY CLUSTERED 
(
	[DocumentMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],

) ON [PRIMARY]

GO

ALTER TABLE [Core].[DocumentMapping] WITH CHECK ADD  CONSTRAINT [FK_DocumentMapping_Assessments] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Core].[DocumentMapping] CHECK CONSTRAINT [FK_DocumentMapping_Assessments] 
GO

ALTER TABLE [Core].[DocumentMapping]   WITH CHECK ADD  CONSTRAINT [FK_DocumentType_DocumentTypeID] FOREIGN KEY([DocumentTypeID])
REFERENCES [Reference].[DocumentType] ([DocumentTypeID])
GO

ALTER TABLE [Core].[DocumentMapping]  CHECK CONSTRAINT [FK_DocumentType_DocumentTypeID]
GO

ALTER TABLE  [Core].[DocumentMapping]   WITH CHECK ADD  CONSTRAINT [FK_DocumentMapping_ScreeningTypeID] FOREIGN KEY([ScreeningTypeID])
REFERENCES [ECI].[ScreeningType] ([ScreeningTypeID])
GO

ALTER TABLE  [Core].[DocumentMapping] CHECK CONSTRAINT [FK_DocumentMapping_ScreeningTypeID]
GO

ALTER TABLE [Core].[DocumentMapping]  ADD CONSTRAINT IX_DocumentMapping UNIQUE([AssessmentID],[DocumentTypeID],[ScreeningTypeID] )
GO

ALTER TABLE Core.DocumentMapping WITH CHECK ADD CONSTRAINT [FK_DocumentMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.DocumentMapping CHECK CONSTRAINT [FK_DocumentMapping_UserModifedBy]
GO
ALTER TABLE Core.DocumentMapping WITH CHECK ADD CONSTRAINT [FK_DocumentMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.DocumentMapping CHECK CONSTRAINT [FK_DocumentMapping_UserCreatedBy]
GO
