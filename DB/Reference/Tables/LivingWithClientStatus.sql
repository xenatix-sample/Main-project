-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[LivingWithClientStatus]
-- Author:		Sumana Sangapu
-- Date:		09/10/2015
--
-- Purpose:		Reference Table for LivingWithClientStatuss.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/10/2015	Sumana Sangapu	TFS# 2258 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[LivingWithClientStatus](
	[LivingWithClientStatusID] [INT] IDENTITY(1,1) NOT NULL,
	[LivingWithClientStatus] [NVARCHAR](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_LivingWithClientStatus] PRIMARY KEY CLUSTERED 
(
	[LivingWithClientStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_ContactLivingWithClientStatus_LivingWithClientStatus] ON [Reference].[LivingWithClientStatus]
(
	[LivingWithClientStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Reference.LivingWithClientStatus WITH CHECK ADD CONSTRAINT [FK_LivingWithClientStatus_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.LivingWithClientStatus CHECK CONSTRAINT [FK_LivingWithClientStatus_UserModifedBy]
GO
ALTER TABLE Reference.LivingWithClientStatus WITH CHECK ADD CONSTRAINT [FK_LivingWithClientStatus_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.LivingWithClientStatus CHECK CONSTRAINT [FK_LivingWithClientStatus_UserCreatedBy]
GO
