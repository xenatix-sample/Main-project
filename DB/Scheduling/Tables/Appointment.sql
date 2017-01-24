
-- Table:		[Appointment]
-- Author:		John Crossen
-- Date:		10/02/2015
--
-- Purpose:		Appointment
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2565 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/11/2016	Satish Singh    Added colums CancelReasonID and CancelComment
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/02/2016	Scott Martin	Added Comments column
-- 04/01/2016   Justin Spalti   Added the IsInterpreterRequired and ServiceStatusID columns. Added a FK to the ServiceStatus table
-- 04/01/2016	Sumana Sangapu	Added NonMHMRAppointment
-- 04/11/2016	Sumana Sangapu	Added IsGroupAppointment and modified AppointmentStartTime to Time datatype
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[Appointment] (
    [AppointmentID]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [ProgramID]            BIGINT             NOT NULL,
    [FacilityID]           INT             NULL,
    [AppointmentTypeID]    INT             NOT NULL,
    [ServicesID]           INT             NULL,
	[ServiceStatusID]      SMALLINT        NULL,
    [AppointmentDate]      DATE            NOT NULL,
    [AppointmentStartTime] SMALLINT	       NOT NULL,
    [AppointmentLength]    SMALLINT        NOT NULL,
    [SupervisionVisit]     BIT             CONSTRAINT [DF_Appointment_SupervisionVisit] DEFAULT ((0)) NULL,
    [ReferredBy]           NVARCHAR (255)  NULL,
    [ReasonForVisit]       NVARCHAR (4000) NULL,
    [RecurrenceID]         BIGINT          NULL,
    [IsCancelled]          BIT             CONSTRAINT [DF_Appointment_IsCancelled] DEFAULT ((0)) NOT NULL,
    [CancelReasonID]       INT             NULL,
    [CancelComment]        NVARCHAR (1000) NULL,
    [Comments]             NVARCHAR (4000) NULL,
	[IsInterpreterRequired] bit			   NULL,
	[NonMHMRAppointment]   NVARCHAR(100)	NULL,
	[IsGroupAppointment]			 BIT NULL,
    [IsActive]             BIT             DEFAULT ((1)) NOT NULL,
    [ModifiedBy]           INT             NOT NULL,
    [ModifiedOn]           DATETIME        DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]            INT             DEFAULT ((1)) NOT NULL,
    [CreatedOn]            DATETIME        DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]      DATETIME        DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn]     DATETIME        DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED ([AppointmentID] ASC),
    CONSTRAINT [FK_Appointment_AppointmentType] FOREIGN KEY ([AppointmentTypeID]) REFERENCES [Scheduling].[AppointmentType] ([AppointmentTypeID]),
    CONSTRAINT [FK_Appointment_Facility] FOREIGN KEY ([FacilityID]) REFERENCES [Reference].[Facility] ([FacilityID]),
    CONSTRAINT [FK_Appointment_Program] FOREIGN KEY ([ProgramID]) REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID]),
    CONSTRAINT [FK_Appointment_Recurrence] FOREIGN KEY ([RecurrenceID]) REFERENCES [Scheduling].[Recurrence] ([RecurrenceID]),
    CONSTRAINT [FK_Appointment_Services] FOREIGN KEY ([ServicesID]) REFERENCES [Reference].[Services] ([ServicesID]),
    CONSTRAINT [FK_Appointment_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID]),
    CONSTRAINT [FK_Appointment_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_Appointment_ServiceStatusID] FOREIGN KEY ([ServiceStatusID]) REFERENCES [Reference].[ServiceStatus] ([ServiceStatusID])
);

GO

