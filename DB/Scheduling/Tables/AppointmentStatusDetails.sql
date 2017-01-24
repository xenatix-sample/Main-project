-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[AppointmentStatusDetails]
-- Author:		Sumana Sangapu
-- Date:		02/11/2016
--
-- Purpose:		Holds the Appointment Status for each of the resources
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/11/2016	Sumana Sangapu	Initial Creation
-- 03/17/2016	Sumana SAngapu Added Cancel fields
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[AppointmentStatusDetails](
	[AppointmentStatusDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[AppointmentResourceID] [bigint] NOT NULL,
	[AppointmentStatusID] int NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[IsCancelled] [bit] NULL,
	[CancelReasonID] [int] NULL,
	[Comments] [nvarchar](1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AppointmentStatus] PRIMARY KEY CLUSTERED 
(
	[AppointmentStatusDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE  [Scheduling].[AppointmentStatusDetails]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentStatusDetails_ResourceID] FOREIGN KEY([AppointmentResourceID])
REFERENCES [Scheduling].[AppointmentResource]  ([AppointmentResourceID])
GO

ALTER TABLE  [Scheduling].[AppointmentStatusDetails] CHECK CONSTRAINT [FK_AppointmentStatusDetails_ResourceID] 
GO

ALTER TABLE  [Scheduling].[AppointmentStatusDetails]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentStatusDetails_StatusID] FOREIGN KEY([AppointmentStatusID])
REFERENCES Reference.[AppointmentStatus]  ([AppointmentStatusID])
GO

ALTER TABLE  [Scheduling].[AppointmentStatusDetails] CHECK CONSTRAINT [FK_AppointmentStatusDetails_StatusID] 
GO
