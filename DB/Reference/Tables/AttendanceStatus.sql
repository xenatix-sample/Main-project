-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AttendanceStatus]
-- Author:		John Crossen
-- Date:		01/18/2016
--
-- Purpose:		Lookup Table for Attendance Status
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	John Crossen	TFS#5465 - Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AttendanceStatus](
[AttendanceStatusID] [SMALLINT] IDENTITY(1,1) NOT NULL,
[AttendanceCode]	[smallint] NOT NULL,
[AttendanceStatus] [NVARCHAR](75) NOT NULL,
[LegacyCode] nvarchar(10) NULL,
[LegacyCodeDescription] nvarchar(100) NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AttendanceStatus] PRIMARY KEY CLUSTERED 
(
	[AttendanceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.AttendanceStatus WITH CHECK ADD CONSTRAINT [FK_AttendanceStatus_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AttendanceStatus CHECK CONSTRAINT [FK_AttendanceStatus_UserModifedBy]
GO
ALTER TABLE Reference.AttendanceStatus WITH CHECK ADD CONSTRAINT [FK_AttendanceStatus_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AttendanceStatus CHECK CONSTRAINT [FK_AttendanceStatus_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Attendance Status', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AttendanceStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating if someone attended a service', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AttendanceStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AttendanceStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AttendanceStatus;
GO;
