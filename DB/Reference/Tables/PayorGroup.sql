-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[PayorGroup]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Holds the PayorGroup details for the Benefits Screens.
--
-- Notes:		
--
-- Depends:		Reference.Payor,
--				Reference.Address,
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TaskID - 583 -	Initial creation.
-- 07/24/2015   John Crossen					Change IsActive to NOT NULL and add default value
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 08/17/2015	Sumana Sangapu			 1100   Moved to Reference schema
-- 09/16/2015   John Crossen             2313   Refactor Contact benefits
-- 09/24/2015   Arun Choudhary           2377   Made [Group Name] as not null.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[PayorGroup](
	[PayorGroupID] INT IDENTITY(1,1) NOT NULL,
	[PayorPlanID] [int] NOT NULL,
	[GroupID] [nvarchar](50)  NULL,
	[GroupName] [nvarchar](250) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_PayorGroup_PayorGroupID] PRIMARY KEY CLUSTERED 
(
	[PayorGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT [FK_PayorGroup_PayorID] FOREIGN KEY([PayorPlanID]) REFERENCES [Reference].[PayorPlan] ([PayorPlanID]), 
) ON [PRIMARY]

GO

ALTER TABLE Reference.PayorGroup WITH CHECK ADD CONSTRAINT [FK_PayorGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorGroup CHECK CONSTRAINT [FK_PayorGroup_UserModifedBy]
GO
ALTER TABLE Reference.PayorGroup WITH CHECK ADD CONSTRAINT [FK_PayorGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorGroup CHECK CONSTRAINT [FK_PayorGroup_UserCreatedBy]
GO

