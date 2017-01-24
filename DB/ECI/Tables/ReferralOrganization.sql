-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ReferralOrganization]
-- Author:		John Crossen
-- Date:		1/2/2016
--
-- Purpose:		ReferralOrganization
-- Notes:		N/A (or any additional notes)
--
-- Depends:		ECI.[[ReferralOrganization]]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/2/2016	John Crossen	 TFS:4909		Create Table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ReferralOrganization](
	[ReferralOrganizationID] [int] IDENTITY(1,1) NOT NULL,
	[ReferralOrganization] [nvarchar](400) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralOrganization] PRIMARY KEY CLUSTERED 
(
	[ReferralOrganizationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ECI.ReferralOrganization WITH CHECK ADD CONSTRAINT [FK_ReferralOrganization_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralOrganization CHECK CONSTRAINT [FK_ReferralOrganization_UserModifedBy]
GO
ALTER TABLE ECI.ReferralOrganization WITH CHECK ADD CONSTRAINT [FK_ReferralOrganization_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ReferralOrganization CHECK CONSTRAINT [FK_ReferralOrganization_UserCreatedBy]
GO


