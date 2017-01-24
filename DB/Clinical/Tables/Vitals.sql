-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[Vitals]
-- Author:		John Crossen
-- Date:		11/05/2015
--
-- Purpose:		Vitals
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/05/2015	John Crossen	TFS# 2894 - Initial creation.
-- 11/24/2015	Scott Martin	TFS 3703	Added Waist Circumference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Clinical.[Vitals](
	[VitalID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	EncounterID BIGINT NULL,
	[HeightFeet] [smallint] NULL,
	[HeightInches] [smallint] NULL,
	[WeightLbs] [smallint] NULL,
	[WeightOz] [smallint] NULL,
	[BMI] [decimal](8, 1) NULL,
	[LMP] [date] NULL,
	[TakenTime] [datetime] NULL,
	[TakenBy] [INT] NULL,
	[BPMethodID] [smallint] NULL,
	[Systolic] [smallint] NULL,
	[Diastolic] [smallint] NULL,
	[OxygenSaturation] [smallint] NULL,
	[RespiratoryRate] [smallint] NULL,
	[Pulse] [smallint] NULL,
	[Temperature] [decimal](4, 1) NULL,
	[Glucose] [smallint] NULL,
	[WaistCircumference] [smallint] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Vitals] PRIMARY KEY CLUSTERED 
(
	[VitalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Clinical.[Vitals]  WITH CHECK ADD  CONSTRAINT [FK_Vitals_BPMethod] FOREIGN KEY([BPMethodID])
REFERENCES Clinical.[BPMethod] ([BPMethodID])
GO

ALTER TABLE Clinical.[Vitals] CHECK CONSTRAINT [FK_Vitals_BPMethod]
GO

ALTER TABLE Clinical.[Vitals]  WITH CHECK ADD  CONSTRAINT [FK_Vitals_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE Clinical.[Vitals] CHECK CONSTRAINT [FK_Vitals_Contact]
GO

ALTER TABLE [Clinical].[Vitals]  WITH CHECK ADD  CONSTRAINT [FK_Vitals_Encounter] FOREIGN KEY([EncounterID])
REFERENCES [Clinical].[Encounter] ([EncounterID])
GO

ALTER TABLE [Clinical].[Vitals] CHECK CONSTRAINT [FK_Vitals_Encounter]
GO

ALTER TABLE Clinical.Vitals WITH CHECK ADD CONSTRAINT [FK_Vitals_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Vitals CHECK CONSTRAINT [FK_Vitals_UserModifedBy]
GO
ALTER TABLE Clinical.Vitals WITH CHECK ADD CONSTRAINT [FK_Vitals_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Vitals CHECK CONSTRAINT [FK_Vitals_UserCreatedBy]
GO
