-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ProgramClientIdentifier]
-- Author:		Vishal Joshi
-- Date:		01/08/2016
--
-- Purpose:		mapping of ClientTypeID to required ClientIdentifierTypeID
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn

CREATE TABLE [Reference].[ProgramClientIdentifier](
	[ProgramClientIdentifierID] [int] IDENTITY(1,1) NOT NULL,
	[ClientTypeID] [int] NOT NULL,
	[ClientIdentifierTypeID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactClientIdentifier_ContactClientIdentifierID] PRIMARY KEY CLUSTERED 
(
	[ProgramClientIdentifierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ProgramClientIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProgramClientIdentifier_ClientIdentifierType] FOREIGN KEY([ClientIdentifierTypeID])
REFERENCES [Reference].[ClientIdentifierType] ([ClientIdentifierTypeID])
GO

ALTER TABLE [Reference].[ProgramClientIdentifier] CHECK CONSTRAINT [FK_ProgramClientIdentifier_ClientIdentifierType]
GO

ALTER TABLE [Reference].[ProgramClientIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProgramClientIdentifier_ClientType] FOREIGN KEY([ClientTypeID])
REFERENCES [Reference].[ClientType] ([ClientTypeID])
GO

ALTER TABLE [Reference].[ProgramClientIdentifier] CHECK CONSTRAINT [FK_ProgramClientIdentifier_ClientType]
GO

ALTER TABLE Reference.ProgramClientIdentifier WITH CHECK ADD CONSTRAINT [FK_ProgramClientIdentifier_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ProgramClientIdentifier CHECK CONSTRAINT [FK_ProgramClientIdentifier_UserModifedBy]
GO
ALTER TABLE Reference.ProgramClientIdentifier WITH CHECK ADD CONSTRAINT [FK_ProgramClientIdentifier_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ProgramClientIdentifier CHECK CONSTRAINT [FK_ProgramClientIdentifier_UserCreatedBy]
GO

