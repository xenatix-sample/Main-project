-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[UserIdentifierType]
-- Author:		Sumana Sangapu
-- Date:		04/06/2016
--
-- Purpose:		Lookup for  UserIdentifierType details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[UserIdentifierType](
	[UserIdentifierTypeID] [int] IDENTITY (1,1) NOT NULL,
	[UserIdentifierType] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_UserIdentifierType_UserIdentifierTypeID] PRIMARY KEY CLUSTERED 
(
	[UserIdentifierTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[UserIdentifierType] ADD CONSTRAINT IX_UserIdentifierType UNIQUE(UserIdentifierType)
GO

ALTER TABLE Reference.UserIdentifierType WITH CHECK ADD CONSTRAINT [FK_UserIdentifierType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.UserIdentifierType CHECK CONSTRAINT [FK_UserIdentifierType_UserModifedBy]
GO
ALTER TABLE Reference.UserIdentifierType WITH CHECK ADD CONSTRAINT [FK_UserIdentifierType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.UserIdentifierType CHECK CONSTRAINT [FK_UserIdentifierType_UserCreatedBy]
GO
