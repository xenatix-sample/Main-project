-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ReferralConcern]
-- Author:		John Crossen
-- Date:		1/2/2016
--
-- Purpose:		ReferralConcern
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		ECI.ReferralConcern
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/2/2016	John Crossen	 TFS:4909		Create Table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE ECI.[ReferralConcern](
	[ReferralConcernID] [INT] IDENTITY(1,1) NOT NULL,
	[ReferralConcern] [NVARCHAR](50) NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [INT] NOT NULL CONSTRAINT [DF_ECIReferralConcern_ModifiedBy]  DEFAULT ((1)),
	[ModifiedOn] [DATETIME] NOT NULL CONSTRAINT [DF_ECIReferralConcern_ModifiedOn]  DEFAULT (GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIReferralConcerns] PRIMARY KEY CLUSTERED 
(
	[ReferralConcernID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ECI.ReferralConcern WITH CHECK ADD CONSTRAINT [FK_ReferralConcern_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralConcern CHECK CONSTRAINT [FK_ReferralConcern_UserModifedBy]
GO
ALTER TABLE ECI.ReferralConcern WITH CHECK ADD CONSTRAINT [FK_ReferralConcern_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralConcern CHECK CONSTRAINT [FK_ReferralConcern_UserCreatedBy]
GO
