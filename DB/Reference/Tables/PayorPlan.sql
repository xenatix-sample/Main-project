-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[PayorPlan]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Holds the PayorPlan details for the Benefits Screens.
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
-- 09/24/2015   Arun Choudhary           2377   Made [PlanID] as nullable.
-- 01/13/2016	Scott Martin					Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 12/13/2016	Sumana Sangapu					Added Effective and Expiration Dates
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[PayorPlan](
	[PayorPlanID] [INT] IDENTITY(1,1) NOT NULL,
	[PayorID] INT NOT NULL,
	[PlanName] [NVARCHAR](250) NOT NULL,
	[PlanID] [NVARCHAR](50),
	[EffectiveDate] DATE  NULL,
	[ExpirationDate] DATE NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_PayorPlan] PRIMARY KEY CLUSTERED 
(
	[PayorPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[PayorPlan]  WITH CHECK ADD  CONSTRAINT [FK_PayorPlan_Payor] FOREIGN KEY([PayorID])
REFERENCES [Reference].[Payor] ([PayorID])
GO

ALTER TABLE [Reference].[PayorPlan] CHECK CONSTRAINT [FK_PayorPlan_Payor]
GO

ALTER TABLE Reference.PayorPlan WITH CHECK ADD CONSTRAINT [FK_PayorPlan_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorPlan CHECK CONSTRAINT [FK_PayorPlan_UserModifedBy]
GO
ALTER TABLE Reference.PayorPlan WITH CHECK ADD CONSTRAINT [FK_PayorPlan_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorPlan CHECK CONSTRAINT [FK_PayorPlan_UserCreatedBy]
GO
