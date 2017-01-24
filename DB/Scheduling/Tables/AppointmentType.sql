-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[AppointmentType] 
-- Author:		Sumana Sangapu
-- Date:		04/01/2016
--
-- Purpose:		Holds  AppointmentTypes 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu	Created new seed script file
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[AppointmentType](
	[AppointmentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentType] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[IsBlocked] [bit] NOT NULL DEFAULT ((0)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_AppointmentType1] PRIMARY KEY CLUSTERED 
(
	[AppointmentTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[AppointmentType]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentType_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Scheduling].[AppointmentType] CHECK CONSTRAINT [FK_AppointmentType_UserCreatedBy]
GO

ALTER TABLE [Scheduling].[AppointmentType]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentType_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Scheduling].[AppointmentType] CHECK CONSTRAINT [FK_AppointmentType_UserModifedBy]
GO


