-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserFacility]
-- Author:		John Crossen
-- Date:		10/14/2015
--
-- Purpose:		Holds the UserFacility details  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/14/2015   John Crossen  Task# 2708 - Assessment DB Design
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[UserFacility](
	[UserFacilityID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[UserID] [INT] NOT NULL,
	[FacilityID] [INT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_UserFacility] PRIMARY KEY CLUSTERED 
(
	[UserFacilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[UserFacility]  WITH CHECK ADD  CONSTRAINT [FK_UserFacility_Facility] FOREIGN KEY([FacilityID])
REFERENCES [Reference].[Facility] ([FacilityID])
GO

ALTER TABLE [Core].[UserFacility] CHECK CONSTRAINT [FK_UserFacility_Facility]
GO

ALTER TABLE [Core].[UserFacility]  WITH CHECK ADD  CONSTRAINT [FK_UserFacility_Users] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[UserFacility] CHECK CONSTRAINT [FK_UserFacility_Users]
GO

ALTER TABLE Core.UserFacility WITH CHECK ADD CONSTRAINT [FK_UserFacility_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserFacility CHECK CONSTRAINT [FK_UserFacility_UserModifedBy]
GO
ALTER TABLE Core.UserFacility WITH CHECK ADD CONSTRAINT [FK_UserFacility_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserFacility CHECK CONSTRAINT [FK_UserFacility_UserCreatedBy]
GO

