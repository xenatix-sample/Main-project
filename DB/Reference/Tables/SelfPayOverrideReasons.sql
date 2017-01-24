-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.SelfPayOverrideReason
-- Author:		Sumana Sangapu
-- Date:		09/11/2015
--
-- Purpose:		Holds Override Reasons for Self Pay screen 
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

CREATE TABLE [Reference].[SelfPayOverrideReasons](
	[SelfPayOverrideReasonID] [int] IDENTITY(1,1) NOT NULL,
	[SelfPayOverrideReason] [nvarchar](100) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SelfPayOverrideReasons_SelfPayOverrideReasonID] PRIMARY KEY CLUSTERED 
(
	[SelfPayOverrideReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SelfPayOverrideReasons_SelfPayOverridesName] UNIQUE NONCLUSTERED 
(
	[SelfPayOverrideReason] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.SelfPayOverrideReasons WITH CHECK ADD CONSTRAINT [FK_SelfPayOverrideReasons_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SelfPayOverrideReasons CHECK CONSTRAINT [FK_SelfPayOverrideReasons_UserModifedBy]
GO
ALTER TABLE Reference.SelfPayOverrideReasons WITH CHECK ADD CONSTRAINT [FK_SelfPayOverrideReasons_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SelfPayOverrideReasons CHECK CONSTRAINT [FK_SelfPayOverrideReasons_UserCreatedBy]
GO
