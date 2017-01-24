-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ADSecurityGroup]
-- Author:		Kyle Campbell
-- Date:		04/28/2016
--
-- Purpose:		Active Directory Groups
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[ADSecurityGroup] (
    [SecurityRoleID] INT            NOT NULL,
    [ADGroup]        NVARCHAR (150) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ADSecurityGroup] PRIMARY KEY CLUSTERED 
	(
	   [SecurityRoleID] ASC, 
	   [ADGroup] ASC
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
ALTER TABLE Core.ADSecurityGroup WITH CHECK ADD CONSTRAINT [FK_ADSecurityGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ADSecurityGroup CHECK CONSTRAINT [FK_ADSecurityGroup_UserModifedBy]
GO
ALTER TABLE Core.ADSecurityGroup WITH CHECK ADD CONSTRAINT [FK_ADSecurityGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ADSecurityGroup CHECK CONSTRAINT [FK_ADSecurityGroup_UserCreatedBy]
GO
