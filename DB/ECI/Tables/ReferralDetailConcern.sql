-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ReferralDetailConcern]
-- Author:		John Crossen
-- Date:		1/2/2016
--
-- Purpose:		ReferralConcern
-- Notes:		N/A (or any additional notes)
--
-- Depends:		ECI.[[ReferralDetailConcern]]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/2/2016	John Crossen	 TFS:4909		Create Table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ReferralDetailConcern](
	[ReferralDetailConcernID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralDetailID] [bigint] NOT NULL,
	[ReferralConcernID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralDetailConcern] PRIMARY KEY CLUSTERED 
(
	[ReferralDetailConcernID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ReferralDetailConcern]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetailConcern_ReferralConcern] FOREIGN KEY([ReferralConcernID])
REFERENCES [ECI].[ReferralConcern] ([ReferralConcernID])
GO

ALTER TABLE [ECI].[ReferralDetailConcern] CHECK CONSTRAINT [FK_ReferralDetailConcern_ReferralConcern]
GO

ALTER TABLE [ECI].[ReferralDetailConcern]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDetailConcern_ReferralDetail] FOREIGN KEY([ReferralDetailID])
REFERENCES [ECI].[ReferralDetail] ([ReferralDetailID])
GO

ALTER TABLE [ECI].[ReferralDetailConcern] CHECK CONSTRAINT [FK_ReferralDetailConcern_ReferralDetail]
GO

ALTER TABLE ECI.ReferralDetailConcern WITH CHECK ADD CONSTRAINT [FK_ReferralDetailConcern_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralDetailConcern CHECK CONSTRAINT [FK_ReferralDetailConcern_UserModifedBy]
GO
ALTER TABLE ECI.ReferralDetailConcern WITH CHECK ADD CONSTRAINT [FK_ReferralDetailConcern_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralDetailConcern CHECK CONSTRAINT [FK_ReferralDetailConcern_UserCreatedBy]
GO
