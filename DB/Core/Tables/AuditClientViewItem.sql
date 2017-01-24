-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AuditClientViewItem]
-- Author:		Scott Martin
-- Date:		01/21/2016
--
-- Purpose:		Items for identifying what data a user is viewing in the application
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AuditClientViewItem](
	[AuditClientViewItemID] [int] IDENTITY (1,1) NOT NULL,
	[AuditClientViewItem] NVARCHAR(50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AuditClientViewItem_AuditClientViewItemID] PRIMARY KEY CLUSTERED 
(
	[AuditClientViewItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO