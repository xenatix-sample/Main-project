-----------------------------------------------------------------------------------------------------------------------
-- Table:		[UserPhoto]
-- Author:		Scott Martin
-- Date:		02/24/2016
--
-- Purpose:		User/Photo mapping data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/24/2016	Scott Martin		Initial Creation
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Core.UserPhoto
(
	UserPhotoID	BIGINT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	PhotoID	BIGINT NOT NULL,
	IsPrimary BIT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_UserPhoto_UserPhotoID] PRIMARY KEY CLUSTERED 
(
	[UserPhotoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[UserPhoto]  WITH CHECK ADD  CONSTRAINT [FK_UserPhoto_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[UserPhoto] CHECK CONSTRAINT [FK_UserPhoto_UserID]
GO

ALTER TABLE [Core].[UserPhoto]  WITH CHECK ADD  CONSTRAINT [FK_UserPhoto_PhotoID] FOREIGN KEY([PhotoID])
REFERENCES [Core].[Photo] ([PhotoID])
GO

ALTER TABLE [Core].[UserPhoto] CHECK CONSTRAINT [FK_UserPhoto_PhotoID]
GO

ALTER TABLE Core.UserPhoto WITH CHECK ADD CONSTRAINT [FK_UserPhoto_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserPhoto CHECK CONSTRAINT [FK_UserPhoto_UserModifedBy]
GO
ALTER TABLE Core.UserPhoto WITH CHECK ADD CONSTRAINT [FK_UserPhoto_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserPhoto CHECK CONSTRAINT [FK_UserPhoto_UserCreatedBy]
GO
