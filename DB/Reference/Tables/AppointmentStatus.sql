-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AppointmentStatus]
-- Author:		John Crossen
-- Date:		07/21/2015
--
-- Purpose:		Lookup for Appointment Status details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/10/2016	John Crossen	TFS# 7684 - Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AppointmentStatus](
	[AppointmentStatusID] [int] IDENTITY (1,1) NOT NULL,
	[AppointmentStatus] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AppointmentStatus_AppointmentStatusID] PRIMARY KEY CLUSTERED 
(
	[AppointmentStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[AppointmentStatus] ADD CONSTRAINT IX_AppointmentStatus UNIQUE([AppointmentStatus])
GO
ALTER TABLE Reference.AppointmentStatus WITH CHECK ADD CONSTRAINT [FK_AppointmentStatus_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AppointmentStatus CHECK CONSTRAINT [FK_AppointmentStatus_UserModifedBy]
GO
ALTER TABLE Reference.AppointmentStatus WITH CHECK ADD CONSTRAINT [FK_AppointmentStatus_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AppointmentStatus CHECK CONSTRAINT [FK_AppointmentStatus_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Appointment Status', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AppointmentStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating if someone attended an appointment', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AppointmentStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AppointmentStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AppointmentStatus;
GO;
