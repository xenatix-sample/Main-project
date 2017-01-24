-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserAddress]
-- Author:		Justin Spalti
-- Date:		
--
-- Purpose:		Mapping b/w user and address
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
CREATE TABLE [Core].[UserAddress](
	[UserAddressID] [bigint] IDENTITY(1,1) NOT NULL,
	[AddressID] [bigint] NOT NULL,
	[UserID] [int] NOT NULL,
	[MailPermissionID] [int] NULL,
	[IsPrimary] [bit] NOT NULL DEFAULT ((0)),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_UserAddress_UserAddressID] PRIMARY KEY CLUSTERED 
(
	[UserAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [IX_AddressID]    Script Date: 9/30/2015 6:16:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_AddressID] ON [Core].[UserAddress]
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_UserID]    Script Date: 9/30/2015 6:16:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserID] ON [Core].[UserAddress]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [Core].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK_UserAddress_AddressID] FOREIGN KEY([AddressID])
REFERENCES [Core].[Addresses] ([AddressID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserAddress] CHECK CONSTRAINT [FK_UserAddress_AddressID]
GO

ALTER TABLE [Core].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK_UserAddress_MailPermissionID] FOREIGN KEY([MailPermissionID])
REFERENCES [Reference].[MailPermission] ([MailPermissionID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserAddress] CHECK CONSTRAINT [FK_UserAddress_MailPermissionID]
GO

ALTER TABLE [Core].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK_UserAddress_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserAddress] CHECK CONSTRAINT [FK_UserAddress_UserID]
GO

ALTER TABLE Core.UserAddress WITH CHECK ADD CONSTRAINT [FK_UserAddress_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserAddress CHECK CONSTRAINT [FK_UserAddress_UserModifedBy]
GO
ALTER TABLE Core.UserAddress WITH CHECK ADD CONSTRAINT [FK_UserAddress_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserAddress CHECK CONSTRAINT [FK_UserAddress_UserCreatedBy]
GO

