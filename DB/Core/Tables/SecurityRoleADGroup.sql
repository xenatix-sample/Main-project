-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[SecurityRoleADGroup]
-- Author:		
-- Date:		
--
-- Purpose:		SecurityRoleADGroup
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[SecurityRoleADGroup] (
    [SecurityRoleID] INT            NOT NULL,
    [ADGroup]        NVARCHAR (150) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_SecurityRoleADGroup] PRIMARY KEY CLUSTERED 
	(
	[SecurityRoleID] ASC, 
	[ADGroup] ASC
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

ALTER TABLE Core.SecurityRoleADGroup WITH CHECK ADD CONSTRAINT [FK_SecurityRoleADGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SecurityRoleADGroup CHECK CONSTRAINT [FK_SecurityRoleADGroup_UserModifedBy]
GO
ALTER TABLE Core.SecurityRoleADGroup WITH CHECK ADD CONSTRAINT [FK_SecurityRoleADGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SecurityRoleADGroup CHECK CONSTRAINT [FK_SecurityRoleADGroup_UserCreatedBy]
GO


