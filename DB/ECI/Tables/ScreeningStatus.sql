-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ScreeningStatus]
-- Author:		Sumana Sangapu
-- Date:		10/12/2015
--
-- Purpose:		ECI Screening Status Table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/12/2015	Sumana Sangapu     TFS:2620		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ScreeningStatus](
	[ScreeningStatusID] [smallint] IDENTITY(1,1) NOT NULL,
	[ScreeningStatus] [NVARCHAR](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ScreeningStatus] PRIMARY KEY CLUSTERED 
(
	[ScreeningStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ScreeningStatus] UNIQUE NONCLUSTERED  
(
	[ScreeningStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ECI.ScreeningStatus WITH CHECK ADD CONSTRAINT [FK_ScreeningStatus_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningStatus CHECK CONSTRAINT [FK_ScreeningStatus_UserModifedBy]
GO
ALTER TABLE ECI.ScreeningStatus WITH CHECK ADD CONSTRAINT [FK_ScreeningStatus_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningStatus CHECK CONSTRAINT [FK_ScreeningStatus_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Screening Status', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the state of a screening', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningStatus;
GO;