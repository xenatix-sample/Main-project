-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ResourceOverrides]
-- Author:		John Crossen
-- Date:		09/10/2015
--
-- Purpose:		ResourceOverrides
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2229 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[ResourceOverrides](
	[ResourceOverrideID] [bigint] NOT NULL IDENTITY(1,1),
	[ResourceID] [int] NOT NULL,
	[ResourceTypeID] [smallint] NOT NULL,
	[OverrideDate] [date] NOT NULL,
	[Comments] [NVARCHAR] (4000) NULL,
	[FacilityID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ResourceOverrides] PRIMARY KEY CLUSTERED 
(
	[ResourceOverrideID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[ResourceOverrides]  WITH CHECK ADD  CONSTRAINT [FK_ResourceOverrides_Facility] FOREIGN KEY([FacilityID])
REFERENCES [Reference].[Facility] ([FacilityID])
GO

ALTER TABLE [Scheduling].[ResourceOverrides] CHECK CONSTRAINT [FK_ResourceOverrides_Facility]
GO

ALTER TABLE Scheduling.ResourceOverrides WITH CHECK ADD CONSTRAINT [FK_ResourceOverrides_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.ResourceOverrides CHECK CONSTRAINT [FK_ResourceOverrides_UserModifedBy]
GO
ALTER TABLE Scheduling.ResourceOverrides WITH CHECK ADD CONSTRAINT [FK_ResourceOverrides_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.ResourceOverrides CHECK CONSTRAINT [FK_ResourceOverrides_UserCreatedBy]
GO
