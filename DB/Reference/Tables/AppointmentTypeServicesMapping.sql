-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AppointmentTypeServicesMapping]
-- Author:		Sumana Sangapu
-- Date:		04/14/2016
--
-- Purpose:		Mapping between AppointmentTypes and Services  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/14/2016   Sumana Sangapu  Initial Creation
-- 04/16/2016	Sumana Sangapu	Created a mapping with Reference.ServicesModuleMapping
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AppointmentTypeServicesMapping](
	[AppointmentTypeServicesMappingID] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentTypeID] [int] NOT NULL,
	[ServicesID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_AppointmentTypeServicesMappingID] PRIMARY KEY CLUSTERED 
(
	[AppointmentTypeServicesMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_AppointmentTypeServicesMapping] UNIQUE NONCLUSTERED 
(
	[ServicesID] ASC,
	[AppointmentTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[AppointmentTypeServicesMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeServicesMapping_AppointmentTypeID] FOREIGN KEY([AppointmentTypeID])
REFERENCES [Scheduling].[AppointmentType] ([AppointmentTypeID])
GO

ALTER TABLE [Reference].[AppointmentTypeServicesMapping] CHECK CONSTRAINT [FK_AppointmentTypeServicesMapping_AppointmentTypeID]
GO

--ALTER TABLE [Reference].[AppointmentTypeServicesMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeServicesMapping_ServicesID] FOREIGN KEY([ServicesID])
--REFERENCES [Reference].[ServicesModuleMapping] ([ServicesID])
--GO

--ALTER TABLE [Reference].[AppointmentTypeServicesMapping] CHECK CONSTRAINT [FK_AppointmentTypeServicesMapping_ServicesID]
--GO

ALTER TABLE [Reference].[AppointmentTypeServicesMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeServicesMapping_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[AppointmentTypeServicesMapping] CHECK CONSTRAINT [FK_AppointmentTypeServicesMapping_UserCreatedBy]
GO

ALTER TABLE [Reference].[AppointmentTypeServicesMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeServicesMapping_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[AppointmentTypeServicesMapping] CHECK CONSTRAINT [FK_AppointmentTypeServicesMapping_UserModifedBy]
GO


