-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactPhoto]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Contact/Photo mapping data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/29/2015	Scott Martin		Initial Creation
-- 01/13/2016	Scott Martin		Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/24/2016	Scott Martin		Moved Photo to Core Schema
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Registration.ContactPhoto
(
	ContactPhotoID			BIGINT IDENTITY(1,1) NOT NULL,
	ContactID				BIGINT NOT NULL,
	PhotoID					BIGINT NOT NULL,
	IsPrimary				BIT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactPhoto_ContactPhotoID] PRIMARY KEY CLUSTERED 
(
	[ContactPhotoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ContactPhoto]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhoto_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactPhoto] CHECK CONSTRAINT [FK_ContactPhoto_ContactID]
GO

ALTER TABLE [Registration].[ContactPhoto]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhoto_PhotoID] FOREIGN KEY([PhotoID])
REFERENCES [Core].[Photo] ([PhotoID])
GO

ALTER TABLE [Registration].[ContactPhoto] CHECK CONSTRAINT [FK_ContactPhoto_PhotoID]
GO

ALTER TABLE Registration.ContactPhoto WITH CHECK ADD CONSTRAINT [FK_ContactPhoto_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactPhoto CHECK CONSTRAINT [FK_ContactPhoto_UserModifedBy]
GO
ALTER TABLE Registration.ContactPhoto WITH CHECK ADD CONSTRAINT [FK_ContactPhoto_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactPhoto CHECK CONSTRAINT [FK_ContactPhoto_UserCreatedBy]
GO
