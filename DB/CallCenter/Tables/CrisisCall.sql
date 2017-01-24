-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[Crisis Call]
-- Author:		John Crossen
-- Date:		01/21/2016
--
-- Purpose:		Header Table for Call Center Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	John Crossen	TFS#5409 - Initial creation.
-- 02/16/2016	Rajiv Ranjan	Added dateOfIncident
-- 03/03/2016	Gaurav Gupta	Added ReferralAgencyID
-- 04/27/2016                   Added OtherReferralAgency
-- 05/31/2016	Scott Martin	Increased length of data type for several columns
-- 07/07/2016	Rajiv Ranjan	Increased length of comments and disposition

CREATE TABLE [CallCenter].[CrisisCall](
	[CallCenterHeaderID] [BIGINT] NOT NULL,
	[CallCenterPriorityID] [SMALLINT] NULL,
	[SuicideHomicideID] [SMALLINT] NULL,
	[DateOfIncident] DATETIME NULL,
	[ReasonCalled] [NVARCHAR](4000) NULL,
	[Disposition] [NVARCHAR](MAX) NULL,
	[OtherInformation] [NVARCHAR](4000) NULL,
	[Comments] [NVARCHAR](MAX) NULL,
	[FollowUpRequired] [BIT] NULL DEFAULT (0),
	CallTypeID SMALLINT NULL,
	CallTypeOther NVARCHAR (100) NULL,
	FollowupPlan NVARCHAR(4000) NULL,
	NatureofCall NVARCHAR(4000) NULL,
	ClientStatusID SMALLINT NULL,
	[ClientProviderID] INT NULL,
	BehavioralCategoryID SMALLINT NULL,
	[NoteHeaderID] bigint NULL,
	[ReferralAgencyID] INT NULL,
	[OtherReferralAgency] NVARCHAR (500) NULL,
	[SearchableFields] AS (((((coalesce(isnull(CAST([CallCenterHeaderID] AS NVARCHAR(20)),'') + ':', '') + coalesce(isnull(ReasonCalled,'')+':','')) + coalesce(isnull(Disposition,'')+':','')) + coalesce(isnull(OtherInformation,'')+':','')) + coalesce(isnull(Comments,'')+':',''))) PERSISTED,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
	    
 CONSTRAINT [PK_CrisisCall] PRIMARY KEY CLUSTERED 
(
	[CallCenterHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [CallCenter].[CrisisCall]  WITH CHECK ADD  CONSTRAINT [FK_CrisisCall_CallCenterHeader] FOREIGN KEY([CallCenterHeaderID])
REFERENCES [CallCenter].[CallCenterHeader] ([CallCenterHeaderID])
GO

ALTER TABLE [CallCenter].[CrisisCall] CHECK CONSTRAINT [FK_CrisisCall_CallCenterHeader]
GO

ALTER TABLE [CallCenter].[CrisisCall]  WITH CHECK ADD  CONSTRAINT [FK_CrisisCall_CallCenterPriority] FOREIGN KEY([CallCenterPriorityID])
REFERENCES [CallCenter].[CallCenterPriority] ([CallCenterPriorityID])
GO

ALTER TABLE [CallCenter].[CrisisCall] CHECK CONSTRAINT [FK_CrisisCall_CallCenterPriority]
GO


ALTER TABLE [CallCenter].[CrisisCall]  WITH CHECK ADD  CONSTRAINT [FK_CrisisCall_SuicideHomicide] FOREIGN KEY([SuicideHomicideID])
REFERENCES [CallCenter].[SuicideHomicide] ([SuicideHomicideID])
GO

ALTER TABLE [CallCenter].[CrisisCall] CHECK CONSTRAINT [FK_CrisisCall_SuicideHomicide]
GO


ALTER TABLE [CallCenter].[CrisisCall]  WITH CHECK ADD  CONSTRAINT [FK_CrisisCall_BehavioralCategory] FOREIGN KEY(BehavioralCategoryID)
REFERENCES [CallCenter].[BehavioralCategory] ([BehavioralCategoryID])
GO

ALTER TABLE [CallCenter].[CrisisCall] CHECK CONSTRAINT [FK_CrisisCall_BehavioralCategory]
GO
ALTER TABLE [CallCenter].[CrisisCall]  WITH CHECK ADD  CONSTRAINT [FK_CrisisCall_ReferralAgency] FOREIGN KEY(ReferralAgencyID)
REFERENCES [Reference].[ReferralAgency] ([ReferralAgencyID])
GO
ALTER TABLE [CallCenter].[CrisisCall] CHECK CONSTRAINT [FK_CrisisCall_ReferralAgency]
GO

CREATE FULLTEXT STOPLIST [CrisisCallEmptyStopList];
GO

CREATE FULLTEXT INDEX ON CallCenter.CrisisCall
	(SearchableFields) KEY INDEX [PK_CrisisCall] WITH STOPLIST = [CrisisCallEmptyStopList], CHANGE_TRACKING = AUTO;
GO