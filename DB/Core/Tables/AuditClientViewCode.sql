-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AuditClientViewCode]
-- Author:		Scott Martin
-- Date:		01/21/2016
--
-- Purpose:		Codes for identifying what item a user is viewing in the application
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AuditClientViewCode](
	[AuditClientViewCodeID] [int] IDENTITY (1,1) NOT NULL,
	[AuditClientViewCode] NVARCHAR(50) NOT NULL,
	[ParentAuditClientViewCodeID] INT NOT NULL,
	[AuditClientViewItemID] INT NOT NULL,
	[AuditClientViewItemTypeID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AuditClientViewCode_AuditClientViewCodeID] PRIMARY KEY CLUSTERED 
(
	[AuditClientViewCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[AuditClientViewCode]  WITH CHECK ADD  CONSTRAINT [FK_AuditClientViewCode_AuditClientViewItemID] FOREIGN KEY([AuditClientViewItemID])
REFERENCES [Core].[AuditClientViewItem] ([AuditClientViewItemID])
GO

ALTER TABLE [Core].[AuditClientViewCode] CHECK CONSTRAINT [FK_AuditClientViewCode_AuditClientViewItemID]
GO

ALTER TABLE [Core].[AuditClientViewCode]  WITH CHECK ADD  CONSTRAINT [FK_AuditClientViewCode_AuditClientViewItemTypeID] FOREIGN KEY([AuditClientViewItemTypeID])
REFERENCES [Core].[AuditClientViewItemType] ([AuditClientViewItemTypeID])
GO

ALTER TABLE [Core].[AuditClientViewCode] CHECK CONSTRAINT [FK_AuditClientViewCode_AuditClientViewItemTypeID]
GO