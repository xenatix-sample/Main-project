-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Role]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Role Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 03/01/2016	Sumana Sangapu	Added UserGUID and ADFlag columns
-- 05/13/2016	Scott Martin	Added unique constraint for Name, added EffectiveDate and ExpirationDate
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[Role](
	[RoleID] BIGINT IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](1000) NULL,
	[EffectiveDate] DATE NOT NULL DEFAULT(GETUTCDATE()),
	[ExpirationDate] DATE NULL,
	[RoleGUID] nvarchar(500) NULL,
	[ADFlag] bit NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Role_RoleID] PRIMARY KEY CLUSTERED 
	(
		[RoleID] ASC
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

ALTER TABLE Core.Role WITH CHECK ADD CONSTRAINT [FK_Role_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Role CHECK CONSTRAINT [FK_Role_UserModifedBy]
GO
ALTER TABLE Core.Role WITH CHECK ADD CONSTRAINT [FK_Role_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Role CHECK CONSTRAINT [FK_Role_UserCreatedBy]
GO
ALTER TABLE Core.Role WITH CHECK ADD CONSTRAINT [UC_Role_Name] UNIQUE ([Name])
GO
ALTER TABLE Core.Role CHECK CONSTRAINT [UC_Role_Name]
GO