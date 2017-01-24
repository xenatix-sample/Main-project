-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[CallCenterType]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Lookup Table for Call Center Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	John Crossen	TFS#5299 - Initial creation.
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE CallCenter.[CallCenterType](
	[CallCenterTypeID] [SMALLINT] IDENTITY(1,1) NOT NULL,
	[CallCenterType] [NVARCHAR](255) NOT NULL,
	[IsActive] [BIT] NOT NULL CONSTRAINT [DF_CallCenterTypeIsActive]  DEFAULT ((1)),
	[ModifiedBy] [INT] NOT NULL,
	[ModifiedOn] [DATETIME] NOT NULL CONSTRAINT [DF_CallCenterTypeModifiedOn]  DEFAULT (GETUTCDATE()),
	[CreatedBy] [INT] NOT NULL,
	[CreatedOn] [DATETIME] NOT NULL CONSTRAINT [DF_CallCenterTypeCreatedOn]  DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] [DATETIME] NOT NULL CONSTRAINT [DF_CallCenterTypeSystemCreatedOn]  DEFAULT (GETUTCDATE()),
	[SystemModifiedOn] [DATETIME] NOT NULL CONSTRAINT [DF_CallCenterTypeSystemModifiedOn]  DEFAULT (GETUTCDATE()),
 CONSTRAINT [PK_CallCenterType] PRIMARY KEY CLUSTERED 
(
	[CallCenterTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE CallCenter.CallCenterType WITH CHECK ADD CONSTRAINT [FK_CallCenterType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.CallCenterType CHECK CONSTRAINT [FK_CallCenterType_UserModifedBy]
GO
ALTER TABLE CallCenter.CallCenterType WITH CHECK ADD CONSTRAINT [FK_CallCenterType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.CallCenterType CHECK CONSTRAINT [FK_CallCenterType_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Call Center Type', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the section of the call center module', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = CallCenterType;
GO;