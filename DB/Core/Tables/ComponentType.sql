-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ComponentType]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Attribute to identify different components
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ComponentType](
	[ComponentTypeID] INT IDENTITY(1,1) NOT NULL,
	[ComponentType] [nvarchar](250) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_ComponentType_ComponentTypeID] PRIMARY KEY CLUSTERED 
	(
		[ComponentTypeID] ASC
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

ALTER TABLE Core.ComponentType WITH CHECK ADD CONSTRAINT [FK_ComponentType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ComponentType CHECK CONSTRAINT [FK_ComponentType_UserModifedBy]
GO
ALTER TABLE Core.ComponentType WITH CHECK ADD CONSTRAINT [FK_ComponentType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ComponentType CHECK CONSTRAINT [FK_ComponentType_UserCreatedBy]
GO