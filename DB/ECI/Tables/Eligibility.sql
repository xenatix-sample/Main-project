------------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[Eligibility]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		ECI Eligibility  table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu TFS:2700	Initial Creation
-- 11/03/2015   Justin Spalti - Added the Notes column to the table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[Eligibility](
	[EligibilityID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[EligibilityDate] [date] NOT NULL,
	[EligibilityTypeID] [int] NULL,
	[EligibilityDurationID] [int] NULL,
	[EligibilityCategoryID] [int] NULL,
	[Notes] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Eligibility_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_Eligibility_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Eligibility_EligibilityID] PRIMARY KEY CLUSTERED 
(
	[EligibilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[Eligibility]  WITH CHECK ADD  CONSTRAINT [FK_Eligibility_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [ECI].[Eligibility] CHECK CONSTRAINT [FK_Eligibility_ContactID]
GO

ALTER TABLE [ECI].[Eligibility]  WITH CHECK ADD  CONSTRAINT [FK_Eligibility_EligibilityCategoryID] FOREIGN KEY([EligibilityCategoryID])
REFERENCES [ECI].[EligibilityCategory] ([EligibilityCategoryID])
GO

ALTER TABLE [ECI].[Eligibility] CHECK CONSTRAINT [FK_Eligibility_EligibilityCategoryID]
GO

ALTER TABLE [ECI].[Eligibility]  WITH CHECK ADD  CONSTRAINT [FK_Eligibility_EligibilityDurationID] FOREIGN KEY([EligibilityDurationID])
REFERENCES [ECI].[EligibilityDuration] ([EligibilityDurationID])
GO

ALTER TABLE [ECI].[Eligibility] CHECK CONSTRAINT [FK_Eligibility_EligibilityDurationID]
GO

ALTER TABLE [ECI].[Eligibility]  WITH CHECK ADD  CONSTRAINT [FK_Eligibility_EligibilityTypeID] FOREIGN KEY([EligibilityTypeID])
REFERENCES [ECI].[EligibilityType] ([EligibilityTypeID])
GO

ALTER TABLE [ECI].[Eligibility] CHECK CONSTRAINT [FK_Eligibility_EligibilityTypeID]
GO

ALTER TABLE ECI.Eligibility WITH CHECK ADD CONSTRAINT [FK_Eligibility_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Eligibility CHECK CONSTRAINT [FK_Eligibility_UserModifedBy]
GO
ALTER TABLE ECI.Eligibility WITH CHECK ADD CONSTRAINT [FK_Eligibility_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Eligibility CHECK CONSTRAINT [FK_Eligibility_UserCreatedBy]
GO

