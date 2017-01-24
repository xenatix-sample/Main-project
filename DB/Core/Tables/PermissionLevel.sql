-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[PermissionLevel]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Defines at what level in the organization the user may access data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Added unique constraint for Name
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[PermissionLevel](
	[PermissionLevelID] INT IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[AttributeID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_PermissionLevel_PermissionLevelID] PRIMARY KEY CLUSTERED 
	(
		[PermissionLevelID] ASC
	)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.PermissionLevel WITH CHECK ADD CONSTRAINT [FK_PermissionLevel_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.PermissionLevel CHECK CONSTRAINT [FK_PermissionLevel_UserModifedBy]
GO
ALTER TABLE Core.PermissionLevel WITH CHECK ADD CONSTRAINT [FK_PermissionLevel_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.PermissionLevel CHECK CONSTRAINT [FK_PermissionLevel_UserCreatedBy]
GO
ALTER TABLE Core.PermissionLevel WITH CHECK ADD CONSTRAINT [UC_PermissionLevel_Name] UNIQUE ([Name])
GO
ALTER TABLE Core.PermissionLevel CHECK CONSTRAINT [UC_PermissionLevel_Name]
GO
ALTER TABLE Core.PermissionLevel WITH CHECK ADD CONSTRAINT [FK_PermissionLevel_AttributeID] FOREIGN KEY([AttributeID]) REFERENCES [Core].[OrganizationAttributes] ([AttributeID])
GO
ALTER TABLE Core.PermissionLevel CHECK CONSTRAINT [FK_PermissionLevel_AttributeID]
GO