-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ReferralDetail]
-- Author:		John Crossen
-- Date:		1/2/2016
--
-- Purpose:		ReferralConcern
-- Notes:		N/A (or any additional notes)
--
-- Depends:		ECI.[[ReferralDetail]]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/2/2016	John Crossen	 TFS:4909		Create Table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ReferralDetail](
	[ReferralDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralHeaderID] [bigint] NOT NULL,
	[ReferralDate] [date] NOT NULL,
	[AdditionalConcerns] [nvarchar](4000) NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[ReferralOrganizationID] [int] NULL,
	[OtherOrganization] [nvarchar](500) NULL,
	[ReferralCategoryID] [int] NOT NULL,
	[ReferralSourceID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralDetail] PRIMARY KEY CLUSTERED 
(
	[ReferralDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ReferralDetail]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetail_Reference] FOREIGN KEY([ReferenceID])
REFERENCES [ECI].[Reference] ([ReferenceID])
GO

ALTER TABLE [ECI].[ReferralDetail] CHECK CONSTRAINT [FK_ReferralDetail_Reference]
GO

ALTER TABLE [ECI].[ReferralDetail]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetail_ReferralCategory] FOREIGN KEY([ReferralCategoryID])
REFERENCES [Reference].[ReferralCategory] ([ReferralCategoryID])
GO

ALTER TABLE [ECI].[ReferralDetail] CHECK CONSTRAINT [FK_ReferralDetail_ReferralCategory]
GO

ALTER TABLE [ECI].[ReferralDetail]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetail_ReferralCategorySource] FOREIGN KEY([ReferralSourceID])
REFERENCES [Reference].[ReferralCategorySource] ([ReferralCategorySourceID])
GO

ALTER TABLE [ECI].[ReferralDetail] CHECK CONSTRAINT [FK_ReferralDetail_ReferralCategorySource]
GO

ALTER TABLE [ECI].[ReferralDetail]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetail_ReferralHeader] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader] ([ReferralHeaderID])
GO

ALTER TABLE [ECI].[ReferralDetail] CHECK CONSTRAINT [FK_ReferralDetail_ReferralHeader]
GO

ALTER TABLE [ECI].[ReferralDetail]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetail_ReferralOrganization] FOREIGN KEY([ReferralOrganizationID])
REFERENCES [ECI].[ReferralOrganization] ([ReferralOrganizationID])
GO

ALTER TABLE [ECI].[ReferralDetail] CHECK CONSTRAINT [FK_ReferralDetail_ReferralOrganization]
GO

ALTER TABLE ECI.ReferralDetail WITH CHECK ADD CONSTRAINT [FK_ReferralDetail_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralDetail CHECK CONSTRAINT [FK_ReferralDetail_UserModifedBy]
GO
ALTER TABLE ECI.ReferralDetail WITH CHECK ADD CONSTRAINT [FK_ReferralDetail_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralDetail CHECK CONSTRAINT [FK_ReferralDetail_UserCreatedBy]
GO
