-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ECIIFSP]
-- Author:		John Crossen
-- Date:		09/03/2015
--
-- Purpose:		ECIIFSP functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/03/2015	John Crossen		TFS# 1277 - Initial creation.
-- 10/16/2015   John Crossen        TFS#2763 -- Change table name and some column names
-- 10/20/2015   John Crossen        Add Meeting Delayed
-- 10/27/2015	Sumana Sangapu		Add AssessmentID 
-- 10/30/2015   John Crossen        Add ResponseID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[IFSP](
	[IFSPID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[IFSPTypeID] [int] NOT NULL,
	Comments NVARCHAR(2000) NULL,
	[IFSPMeetingDate] [date] NOT NULL,
	[IFSPFamilySignedDate] [date] NULL,
	[MeetingDelayed] [BIT] NOT NULL DEFAULT(0),
	[ReasonForDelayID] [int] NULL,
	[AssistiveTechnologyNeeded] BIT  NULL DEFAULT(0),
	[AssessmentID] bigint NULL,
	[ResponseID] bigint NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIIFSP] PRIMARY KEY CLUSTERED 
(
	[IFSPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[IFSP]  WITH CHECK ADD  CONSTRAINT [FK_ECIIFSP_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [ECI].[IFSP] CHECK CONSTRAINT [FK_ECIIFSP_Contact]
GO

ALTER TABLE [ECI].[IFSP]  WITH CHECK ADD  CONSTRAINT [FK_IFSP_ReasonForDelay] FOREIGN KEY([ReasonForDelayID])
REFERENCES [ECI].[ReasonForDelay] ([ReasonForDelayID])
GO

ALTER TABLE [ECI].[IFSP] CHECK CONSTRAINT [FK_IFSP_ReasonForDelay]
GO

ALTER TABLE [ECI].[IFSP]  WITH CHECK ADD  CONSTRAINT [FK_IFSP_IFSPType] FOREIGN KEY([IFSPTypeID])
REFERENCES [ECI].[IFSPType] ([IFSPTypeID])
GO

ALTER TABLE [ECI].[IFSP] CHECK CONSTRAINT [FK_IFSP_IFSPType]
GO

ALTER TABLE [ECI].[IFSP]  WITH CHECK ADD  CONSTRAINT [FK_IFSP_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [ECI].[IFSP] CHECK CONSTRAINT [FK_IFSP_AssessmentID]
GO

ALTER TABLE [ECI].[IFSP]  WITH CHECK ADD  CONSTRAINT [FK_ECIIFSP_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE ECI.IFSP WITH CHECK ADD CONSTRAINT [FK_IFSP_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.IFSP CHECK CONSTRAINT [FK_IFSP_UserModifedBy]
GO
ALTER TABLE ECI.IFSP WITH CHECK ADD CONSTRAINT [FK_IFSP_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.IFSP CHECK CONSTRAINT [FK_IFSP_UserCreatedBy]
GO
