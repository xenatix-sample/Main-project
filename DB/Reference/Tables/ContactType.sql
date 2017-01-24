-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ContactType]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Lookup for  ContactType  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TFS# 675 - Initial creation.
-- 07/24/2015   John Crossen    Change IsActive to NOT NULL and add default value
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ContactType](
	[ContactTypeID] [int] IDENTITY (1,1) NOT NULL,
	[ContactType] [nvarchar](200) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactType_ContactTypeID] PRIMARY KEY CLUSTERED 
(
	[ContactTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ContactType] ADD CONSTRAINT IX_ContactType UNIQUE(ContactType)
GO

ALTER TABLE Reference.ContactType WITH CHECK ADD CONSTRAINT [FK_ContactType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ContactType CHECK CONSTRAINT [FK_ContactType_UserModifedBy]
GO
ALTER TABLE Reference.ContactType WITH CHECK ADD CONSTRAINT [FK_ContactType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ContactType CHECK CONSTRAINT [FK_ContactType_UserCreatedBy]
GO
