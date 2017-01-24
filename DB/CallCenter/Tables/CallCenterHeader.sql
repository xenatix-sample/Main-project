-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[CallCenterHeader]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Header Table for Call Center Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	John Crossen	TFS#5299 - Initial creation.
-- 02/16/2016	Rajiv Ranjan	Added providerID
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 05/10/2016   Lokesh Singhal  Added IsLinkedToContact

CREATE TABLE [CallCenter].[CallCenterHeader](
	[CallCenterHeaderID] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentCallCenterHeaderID] [bigint] NULL,
	[CallCenterTypeID] [smallint] NOT NULL,
	[CallerID] [bigint] NULL,
	[ContactID] [bigint] NULL,
	[ContactTypeID] [int] NULL,
	[ProviderID] BIGINT NULL,
	[CallStartTime] [datetime] NULL,
	[CallEndTime] [datetime] NULL,
	[CallStatusID] [smallint] NULL,
	ProgramUnitID BIGINT NULL,
	CountyID INT NULL,
	IsLinkedToContact BIT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_CallCenterHeader] PRIMARY KEY CLUSTERED 
(
	[CallCenterHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [CallCenter].[CallCenterHeader] ADD  CONSTRAINT [DF_CallCenterType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [CallCenter].[CallCenterHeader] ADD  CONSTRAINT [DF_CallCenterType_ModifiedOn]  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [CallCenter].[CallCenterHeader] ADD  CONSTRAINT [DF_CallCenterType_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [CallCenter].[CallCenterHeader] ADD  CONSTRAINT [DF_CallCenterType_SystemCreatedOn]  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [CallCenter].[CallCenterHeader] ADD  CONSTRAINT [DF_CallCenterType_SystemModifiedOn]  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO

ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_CallCenterType] FOREIGN KEY([CallCenterTypeID])
REFERENCES [CallCenter].[CallCenterType] ([CallCenterTypeID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_CallCenterType]
GO

ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_Contact]
GO

ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_Contact1] FOREIGN KEY([CallerID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_Contact1]
GO


ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_CallStatus] FOREIGN KEY([CallStatusID])
REFERENCES [CallCenter].CallStatus ([CallStatusID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_CallStatus]
GO

ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_ProgramUnit] FOREIGN KEY([ProgramUnitID])
REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_ProgramUnit]
GO

ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_CountyID] FOREIGN KEY([CountyID])
REFERENCES Reference.County ([CountyID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_CountyID]
GO

ALTER TABLE [CallCenter].[CallCenterHeader]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterHeader_ContactType] FOREIGN KEY([ContactTypeID]) REFERENCES [Reference].[ContactType] ([ContactTypeID])
GO

ALTER TABLE [CallCenter].[CallCenterHeader] CHECK CONSTRAINT [FK_CallCenterHeader_ContactType]
GO
ALTER TABLE CallCenter.CallCenterHeader WITH CHECK ADD CONSTRAINT [FK_CallCenterHeader_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.CallCenterHeader CHECK CONSTRAINT [FK_CallCenterHeader_UserModifedBy]
GO
ALTER TABLE CallCenter.CallCenterHeader WITH CHECK ADD CONSTRAINT [FK_CallCenterHeader_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.CallCenterHeader CHECK CONSTRAINT [FK_CallCenterHeader_UserCreatedBy]
GO

CREATE NONCLUSTERED INDEX [IX_CallCenterHeader_CallStartTime] ON [CallCenter].[CallCenterHeader]
(
	[CallStartTime] ASC
)
INCLUDE ( 	[CallCenterHeaderID],
	[CallCenterTypeID],
	[IsActive]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO