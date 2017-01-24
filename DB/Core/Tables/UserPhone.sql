-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserPhone]
-- Author:		Justin Spalti
-- Date:		
--
-- Purpose:		Mapping b/w user and phone
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	Justin Spalti - Initial table creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[UserPhone](
	[UserPhoneID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PhoneID] [bigint] NOT NULL,
	[PhonePermissionID] [int] NULL,
	[IsPrimary] [bit] NOT NULL DEFAULT ((0)),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_ContactPhone_ContactID] PRIMARY KEY CLUSTERED 
	(
		[UserPhoneID] ASC
	)	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Core].[UserPhone]  WITH CHECK ADD  CONSTRAINT [FK_UserPhone_PhoneID] FOREIGN KEY([PhoneID])
REFERENCES [Core].[Phone] ([PhoneID])
GO

ALTER TABLE [Core].[UserPhone] CHECK CONSTRAINT [FK_UserPhone_PhoneID]
GO

ALTER TABLE [Core].[UserPhone]  WITH CHECK ADD  CONSTRAINT [FK_UserPhone_PhonePermissionID] FOREIGN KEY([PhonePermissionID])
REFERENCES [Reference].[PhonePermission] ([PhonePermissionID])
GO

ALTER TABLE [Core].[UserPhone] CHECK CONSTRAINT [FK_UserPhone_PhonePermissionID]
GO

ALTER TABLE [Core].[UserPhone]  WITH CHECK ADD  CONSTRAINT [FK_UserPhone_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[UserPhone] CHECK CONSTRAINT [FK_UserPhone_UserID]
GO

ALTER TABLE Core.UserPhone WITH CHECK ADD CONSTRAINT [FK_UserPhone_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserPhone CHECK CONSTRAINT [FK_UserPhone_UserModifedBy]
GO
ALTER TABLE Core.UserPhone WITH CHECK ADD CONSTRAINT [FK_UserPhone_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserPhone CHECK CONSTRAINT [FK_UserPhone_UserCreatedBy]
GO

