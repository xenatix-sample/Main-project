-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Component]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Component Details (UI Screens and other misc stuff)
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Component](
	[ComponentID] BIGINT IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](1000) NULL,
	[ComponentTypeID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_Component_ComponentID] PRIMARY KEY CLUSTERED 
	(
		[ComponentID] ASC
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

ALTER TABLE Core.Component WITH CHECK ADD CONSTRAINT [FK_Component_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Component CHECK CONSTRAINT [FK_Component_UserModifedBy]
GO
ALTER TABLE Core.Component WITH CHECK ADD CONSTRAINT [FK_Component_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Component CHECK CONSTRAINT [FK_Component_UserCreatedBy]
GO
ALTER TABLE Core.Component WITH CHECK ADD CONSTRAINT [FK_Component_ComponentTypeID] FOREIGN KEY ([ComponentTypeID]) REFERENCES [Core].[ComponentType] ([ComponentTypeID])
GO
ALTER TABLE Core.Component CHECK CONSTRAINT [FK_Component_ComponentTypeID]
GO