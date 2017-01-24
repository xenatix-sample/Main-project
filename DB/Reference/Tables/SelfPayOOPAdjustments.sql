-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.SelfPayOOPAdjustments
-- Author:		Sumana Sangapu
-- Date:		09/11/2015
--
-- Purpose:		Holds OOP Adjustments for Self Pay screen 
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

CREATE TABLE [Reference].[SelfPayOOPAdjustments](
	[SelfPayOOPAdjustmentID] [int] IDENTITY(1,1) NOT NULL,
	[SelfPayOOPAdjustmentName] [nvarchar](100) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SelfPayOOPAdjustments_OOPAdjustmentID] PRIMARY KEY CLUSTERED 
(
	[SelfPayOOPAdjustmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SelfPayOOPAdjustments_OOPAdjustmentName] UNIQUE NONCLUSTERED 
(
	[SelfPayOOPAdjustmentName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.SelfPayOOPAdjustments WITH CHECK ADD CONSTRAINT [FK_SelfPayOOPAdjustments_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SelfPayOOPAdjustments CHECK CONSTRAINT [FK_SelfPayOOPAdjustments_UserModifedBy]
GO
ALTER TABLE Reference.SelfPayOOPAdjustments WITH CHECK ADD CONSTRAINT [FK_SelfPayOOPAdjustments_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SelfPayOOPAdjustments CHECK CONSTRAINT [FK_SelfPayOOPAdjustments_UserCreatedBy]
GO
