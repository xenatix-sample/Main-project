-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServiceStatus]
-- Author:		John Crossen
-- Date:		01/18/2016
--
-- Purpose:		Lookup Table for Service Status
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	John Crossen	TFS#5465 - Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ServiceStatus](
[ServiceStatusID] [SMALLINT] IDENTITY(1,1) NOT NULL,
[ServiceStatus] [NVARCHAR](75) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ServiceStatus] PRIMARY KEY CLUSTERED 
(
	[ServiceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ServiceStatus WITH CHECK ADD CONSTRAINT [FK_ServiceStatus_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ServiceStatus CHECK CONSTRAINT [FK_ServiceStatus_UserModifedBy]
GO
ALTER TABLE Reference.ServiceStatus WITH CHECK ADD CONSTRAINT [FK_ServiceStatus_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ServiceStatus CHECK CONSTRAINT [FK_ServiceStatus_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Service Status', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating if state of the service', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceStatus;
GO;
