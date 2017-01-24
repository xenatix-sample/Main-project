-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[DocumentTypeGroup]
-- Author:		Sumana Sangapu
-- Date:		04/06/2016
--
-- Purpose:		Lookup to groups the Document Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Sumana Sangapu - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[DocumentTypeGroup](
	[DocumentTypeGroupID] [int]  IDENTITY (1,1) NOT NULL,
	[DocumentTypeGroup] [nvarchar] (50) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_DocumentTypeGroup_DocumentTypeGroupID] PRIMARY KEY CLUSTERED 
(
	[DocumentTypeGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[DocumentTypeGroup] ADD CONSTRAINT IX_DocumentTypeGroup UNIQUE(DocumentTypeGroup)
GO
ALTER TABLE [Reference].[DocumentTypeGroup] WITH CHECK ADD CONSTRAINT [FK_DocumentTypeGroup_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[DocumentTypeGroup] CHECK CONSTRAINT [FK_DocumentTypeGroup_UserModifedBy]
GO
ALTER TABLE [Reference].[DocumentTypeGroup] WITH CHECK ADD CONSTRAINT [FK_DocumentTypeGroup_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[DocumentTypeGroup] CHECK CONSTRAINT [FK_DocumentTypeGroup_UserCreatedBy]
GO

