-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Permission]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Permission Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn	
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 05/13/2016	Scott Martin	Added unique constraint for Name
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Permission](
	[PermissionID] BIGINT IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](1000) NULL,
	[Code] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_Permission_PermissionID] PRIMARY KEY CLUSTERED 
	(
		[PermissionID] ASC
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
ALTER TABLE Core.Permission WITH CHECK ADD CONSTRAINT [FK_Permission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Permission CHECK CONSTRAINT [FK_Permission_UserModifedBy]
GO
ALTER TABLE Core.Permission WITH CHECK ADD CONSTRAINT [FK_Permission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Permission CHECK CONSTRAINT [FK_Permission_UserCreatedBy]
GO
ALTER TABLE Core.Permission WITH CHECK ADD CONSTRAINT [UC_Permission_Name] UNIQUE ([Name])
GO
ALTER TABLE Core.Permission CHECK CONSTRAINT [UC_Permission_Name]
GO