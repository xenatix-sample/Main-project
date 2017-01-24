-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServiceRecordingDuration]
-- Author:		Sumana Sangapu
-- Date:		12/15/2016
--
-- Purpose:		Lookup Table for Service Recording Duration
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Reference.ServiceDuration(
	[ServiceDurationID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceDurationStart] [int] NULL,
	[ServiceDurationEnd] [int] NULL,
	[ServiceDurationDisplay] [nvarchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ServiceDuration] PRIMARY KEY CLUSTERED 
(
	[ServiceDurationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ServiceDuration] WITH CHECK ADD CONSTRAINT [FK_ServiceDuration_UserModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[ServiceDuration] CHECK CONSTRAINT [FK_ServiceDuration_UserModifiedBy]
GO
ALTER TABLE [Reference].[ServiceDuration] WITH CHECK ADD CONSTRAINT [FK_ServiceDuration_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[ServiceDuration] CHECK CONSTRAINT [FK_ServiceDuration_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'ServiceDuration', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceDuration;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Internal values that indicate the duration dropdown values', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceDuration;
GO

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceDuration;
GO

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceDuration;
GO
