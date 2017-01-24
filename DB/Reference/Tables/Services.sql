-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Services]
-- Author:		John Crossen
-- Date:		10/01/2015
--
-- Purpose:		Services lookup table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	John Crossen	TFS# 2571 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 03/31/2016	Sumana Sangapu	Refactored Services and removed ProgramID to create a mapping Reference.ServicesOrgDetailsMapping
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-- 01/03/2017	Kyle Campbell	TFS #14007	Schema changes for Services Configuration
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[Services](
	[ServicesID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](255) NOT NULL,
	[ServiceCode] NVARCHAR(20) NULL,
	[ServiceConfigServiceTypeID] INT NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[ExpirationReason] NVARCHAR(100) NULL,
	[EncounterReportable] BIT,
	[ServiceDefinition] NVARCHAR(1000) NULL,
	[Notes] NVARCHAR(MAX) NULL,
	[IsInternal] bit NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Services_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_Services_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServicesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[Services] CHECK CONSTRAINT [FK_Services_UserCreatedBy]
GO
ALTER TABLE [Reference].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[Services] CHECK CONSTRAINT [FK_Services_UserModifedBy]
GO
ALTER TABLE [Reference].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_ServiceConfigServiceType] FOREIGN KEY([ServiceConfigServiceTypeID]) REFERENCES [Reference].[ServiceConfigServiceType] ([ServiceConfigServiceTypeID])
GO
ALTER TABLE [Reference].[Services] CHECK CONSTRAINT [FK_Services_ServiceConfigServiceType]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Services', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating service to be performed', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Legacy service code', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = ServiceCode;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID value for service type on service definition screen ', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = ServiceConfigServiceTypeID;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Effective date for this service record', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = EffectiveDate;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Expiration date for this service record', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = ExpirationDate;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Expiration reason for this service record', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = ExpirationReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Flag to identify if this service is a reportable encounter', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = EncounterReportable;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Definition description for this service', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = ServiceDefinition;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Notes for this service record', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Services,
@level2type = N'COLUMN', @level2name = Notes;
GO;