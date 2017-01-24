-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ReportGroup]
-- Author:		Scott Martin
-- Date:		04/27/2016
--
-- Purpose:		Lookup for Report groups
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/27/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ReportGroup](
	[ReportGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ReportGroup] [varchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_ReportGroup] PRIMARY KEY CLUSTERED 
	(
		[ReportGroupID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Reference.ReportGroup WITH CHECK ADD CONSTRAINT [FK_ReportGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReportGroup CHECK CONSTRAINT [FK_ReportGroup_UserModifedBy]
GO
ALTER TABLE Reference.ReportGroup WITH CHECK ADD CONSTRAINT [FK_ReportGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReportGroup CHECK CONSTRAINT [FK_ReportGroup_UserCreatedBy]
GO
