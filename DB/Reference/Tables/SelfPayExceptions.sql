-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.SelfPayExceptions
-- Author:		Sumana Sangapu
-- Date:		09/11/2015
--
-- Purpose:		Holds Exceptions for Self Pay screen 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--				
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	Sumana Sangapu	2245  - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[SelfPayExceptions](
	[SelfPayExceptionID] [int] IDENTITY(1,1) NOT NULL,
	[SelfPayExceptionName] [nvarchar](100) NULL,
	[SelfPayExceptionByRank] [nvarchar] (25) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SelfPayExceptions_SelfPayExceptionID] PRIMARY KEY CLUSTERED 
(
	[SelfPayExceptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SelfPayExceptions_SelfPayExceptionName] UNIQUE NONCLUSTERED 
(
	[SelfPayExceptionName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.SelfPayExceptions WITH CHECK ADD CONSTRAINT [FK_SelfPayExceptions_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SelfPayExceptions CHECK CONSTRAINT [FK_SelfPayExceptions_UserModifedBy]
GO
ALTER TABLE Reference.SelfPayExceptions WITH CHECK ADD CONSTRAINT [FK_SelfPayExceptions_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SelfPayExceptions CHECK CONSTRAINT [FK_SelfPayExceptions_UserCreatedBy]
GO
