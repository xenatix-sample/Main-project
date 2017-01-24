-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[Screening]
-- Author:		John Crossen
-- Date:		09/30/2015
--
-- Purpose:		ECI Screening Table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	John Crossen     TFS:2542		Created .
-- 10/12/2015	Sumana Sangapu	 TFS:2620		Added a column ScreeningStatusID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE  TABLE [ECI].[Screening](
	[ScreeningID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProgramID] [int] NOT NULL,
	[InitialContactDate] [date] NOT NULL,
	[InitialServiceCoordinatorID] [int] NULL,
	[PrimaryServiceCoordinatorID] [int] NULL,
	[ScreeningDate] [date] NOT NULL,
	[ScreeningTypeID] [smallint] NULL,
	[AssessmentID] [bigint] NULL,
	[ScreeningResultsID] [smallint] NULL,
	[ScreeningScore] [int] NULL,
	[ScreeningStatusID] [smallint] NULL,
	[SubmittedByID] INT NULL,
	[ResponseID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIScreening] PRIMARY KEY CLUSTERED 
(
	[ScreeningID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[Screening] ADD  CONSTRAINT [DF_ECIScreening_ScreeningDate]  DEFAULT (getutcdate()) FOR [ScreeningDate]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_ECIScreening_ECIScreeningType] FOREIGN KEY([ScreeningTypeID])
REFERENCES [ECI].[ScreeningType] ([ScreeningTypeID])
GO

ALTER TABLE [ECI].[Screening] CHECK CONSTRAINT [FK_ECIScreening_ECIScreeningType]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_ECIScreening_Program] FOREIGN KEY([ProgramID])
REFERENCES [Reference].[Program] ([ProgramID])
GO

ALTER TABLE [ECI].[Screening] CHECK CONSTRAINT [FK_ECIScreening_Program]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_ECIScreening_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [ECI].[Screening] CHECK CONSTRAINT [FK_ECIScreening_AssessmentID]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_ECIScreening_ScreeningResults] FOREIGN KEY([ScreeningResultsID])
REFERENCES [ECI].[ScreeningResults] ([ScreeningResultsID])
GO

ALTER TABLE [ECI].[Screening] CHECK CONSTRAINT [FK_ECIScreening_ScreeningResults]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_Screening_Users] FOREIGN KEY([InitialServiceCoordinatorID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[Screening] CHECK CONSTRAINT [FK_Screening_Users]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_Screening_Users1] FOREIGN KEY([PrimaryServiceCoordinatorID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[Screening] CHECK CONSTRAINT [FK_Screening_Users1]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_ECIScreening_ScreeningStatus] FOREIGN KEY([ScreeningStatusID])
REFERENCES [ECI].[ScreeningStatus] ([ScreeningStatusID])
GO

ALTER TABLE [ECI].[ScreeningStatus] CHECK CONSTRAINT [FK_ECIScreening_ScreeningStatus]
GO

ALTER TABLE [ECI].[Screening]  WITH CHECK ADD  CONSTRAINT [FK_ECIScreening_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE [ECI].[ScreeningStatus] CHECK CONSTRAINT [FK_ECIScreening_ResponseID]
GO

ALTER TABLE ECI.Screening WITH CHECK ADD CONSTRAINT [FK_Screening_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Screening CHECK CONSTRAINT [FK_Screening_UserModifedBy]
GO
ALTER TABLE ECI.Screening WITH CHECK ADD CONSTRAINT [FK_Screening_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Screening CHECK CONSTRAINT [FK_Screening_UserCreatedBy]
GO
