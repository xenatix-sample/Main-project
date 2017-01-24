
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServiceRecordingSource]
-- Author:		Sumana Sangapu
-- Date:		01/28/2016
--
-- Purpose:		Lookup Table for ServiceRecordingSource. 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/28/2016	Sumana Sangapu	 Initial creation.
-- 06/29/2016	Gurpreet Singh	Added DisplayText
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[ServiceRecordingSource](
	[ServiceRecordingSourceID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceRecordingSource] [nvarchar](200) NOT NULL,
	[DisplayText] [nvarchar](200) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ServiceRecordingSource_ServiceRecordingSourceID] PRIMARY KEY CLUSTERED 
(
	[ServiceRecordingSourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ServiceRecordingSource] UNIQUE NONCLUSTERED 
(
	[ServiceRecordingSource] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ServiceRecordingSource WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingSource_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ServiceRecordingSource CHECK CONSTRAINT [FK_ServiceRecordingSource_UserModifedBy]
GO
ALTER TABLE Reference.ServiceRecordingSource WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingSource_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ServiceRecordingSource CHECK CONSTRAINT [FK_ServiceRecordingSource_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'ServiceRecordingSource', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceRecordingSource;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Internal values that indicate what the service is associated with', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceRecordingSource;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceRecordingSource;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceRecordingSource;
GO;
