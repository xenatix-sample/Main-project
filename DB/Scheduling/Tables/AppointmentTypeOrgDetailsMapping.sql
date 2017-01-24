 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[AppointmentTypeOrgDetailsMapping]
-- Author:		Sumana Sangapu
-- Date:		04/01/2016
--
-- Purpose:		Holds Appointment Type mapping with Org Details Mapping ID ( Program Units)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping](
	[AppointmentTypeOrgDetailsMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationDetailsMappingID] [bigint] NOT NULL,
	[AppointmentTypeID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_AppointmentTypeOrgDetailsMapping] PRIMARY KEY CLUSTERED 
(
	[AppointmentTypeOrgDetailsMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] ADD  DEFAULT (GETUTCDATE()) FOR [ModifiedOn]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] ADD  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] ADD  DEFAULT (GETUTCDATE()) FOR [CreatedOn]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] ADD  DEFAULT (GETUTCDATE()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] ADD  DEFAULT (GETUTCDATE()) FOR [SystemModifiedOn]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_OrganizationDetails] FOREIGN KEY([OrganizationDetailsMappingID])
REFERENCES Core.[OrganizationDetailsMapping] (MappingID)
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] CHECK CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_OrganizationDetails]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_AppointmentTypeID] FOREIGN KEY([AppointmentTypeID])
REFERENCES Scheduling.AppointmentType ([AppointmentTypeID])
GO


ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] CHECK CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_AppointmentTypeID]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] CHECK CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_UserCreatedBy]
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Scheduling].[AppointmentTypeOrgDetailsMapping] CHECK CONSTRAINT [FK_AppointmentTypeOrgDetailsMapping_UserModifedBy]
GO


