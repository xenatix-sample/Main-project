-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Core].[UsersHierarchyMapping]
-- Author:		Sumana Sangapu
-- Date:		01/21/2016
--
-- Purpose:		Users Hierarchy Mapping 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	Sumana Sangapu  Initial creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[UsersHierarchyMapping](
	[MappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ParentID] [int] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_UsersHierarchyMapping_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_UsersHierarchyMapping_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_UsersHierarchyMapping_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_UsersHierarchyMapping_Creat]  DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL CONSTRAINT [DF_UsersHierarchyMapping_SystemCreatedOn]  DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_UsersHierarchyMapping_SystemModifiedOn]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_UsersHierarchyMapping] PRIMARY KEY CLUSTERED 
(
	[MappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[UsersHierarchyMapping] WITH CHECK ADD  CONSTRAINT [FK_UsersHierarchyMapping_UserId] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UsersHierarchyMapping] CHECK CONSTRAINT [FK_UsersHierarchyMapping_UserId]
GO 

ALTER TABLE Core.UsersHierarchyMapping WITH CHECK ADD CONSTRAINT [FK_UsersHierarchyMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UsersHierarchyMapping CHECK CONSTRAINT [FK_UsersHierarchyMapping_UserModifedBy]
GO
ALTER TABLE Core.UsersHierarchyMapping WITH CHECK ADD CONSTRAINT [FK_UsersHierarchyMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UsersHierarchyMapping CHECK CONSTRAINT [FK_UsersHierarchyMapping_UserCreatedBy]
GO
