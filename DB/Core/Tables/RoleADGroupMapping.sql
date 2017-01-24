-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RoleADGroupMapping]
-- Author:		
-- Date:		
--
-- Purpose:		
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[RoleADGroupMapping]
(
	[RoleADGroupMappingID] BIGINT IDENTITY(1,1) NOT NULL,
	[RoleID] BIGINT NOT NULL,
	[ADGroupID] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_RoleADGroupMapping_RoleADGroupMappingID] PRIMARY KEY CLUSTERED 
	(
		[RoleADGroupMappingID] ASC
	)
	WITH 
   (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.RoleADGroupMapping WITH CHECK ADD CONSTRAINT [FK_RoleADGroupMapping_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [Core].[Role] ([RoleID])
GO
ALTER TABLE Core.RoleADGroupMapping CHECK CONSTRAINT [FK_RoleADGroupMapping_RoleID]
GO
ALTER TABLE Core.RoleADGroupMapping WITH CHECK ADD CONSTRAINT [FK_RoleADGroupMapping_ADGroupID] FOREIGN KEY ([ADGroupID]) REFERENCES [Core].[ADGroup] ([ADGroupID])
GO
ALTER TABLE Core.RoleADGroupMapping CHECK CONSTRAINT [FK_RoleADGroupMapping_ADGroupID]
GO
ALTER TABLE Core.RoleADGroupMapping WITH CHECK ADD CONSTRAINT [FK_RoleADGroupMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleADGroupMapping CHECK CONSTRAINT [FK_RoleADGroupMapping_UserModifedBy]
GO
ALTER TABLE Core.RoleADGroupMapping WITH CHECK ADD CONSTRAINT [FK_RoleADGroupMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleADGroupMapping CHECK CONSTRAINT [FK_RoleADGroupMapping_UserCreatedBy]
GO
