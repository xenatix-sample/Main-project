-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[CallCenterPriority]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Lookup Table for CallCenterPriority
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	John Crossen	TFS#5409 - Initial creation.
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [CallCenter].[CallCenterPriority](
[CallCenterPriorityID] [smallint] IDENTITY(1,1) NOT NULL,
[CallCenterPriority] [nvarchar](75) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
 CONSTRAINT [PK_CallCenterPriority] PRIMARY KEY CLUSTERED 
(
	[CallCenterPriorityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE CallCenter.CallCenterPriority WITH CHECK ADD CONSTRAINT [FK_CallCenterPriority_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.CallCenterPriority CHECK CONSTRAINT [FK_CallCenterPriority_UserModifedBy]
GO
ALTER TABLE CallCenter.CallCenterPriority WITH CHECK ADD CONSTRAINT [FK_CallCenterPriority_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.CallCenterPriority CHECK CONSTRAINT [FK_CallCenterPriority_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Call Center Priority', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterPriority;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the priority of call center calls', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterPriority;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterPriority;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterPriority;
GO;