-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Reports]
-- Author:		Demetrios C. Christopher
-- Date:		11/04/2015
--
-- Purpose:		Holds the model details of Reports
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/04/2015	Demetrios C. Christopher - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 05/03/2016	Scott Martin	Added new columns for SSRS functionality
-- 01/03/2017	Arun			Added new column for report display text.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Reports] (
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ReportTypeID] [int] NOT NULL,
	[ReportName] [varchar](100) NOT NULL,
	[ReportDisplayName] [nvarchar](500) NULL,
	[ReportModel] [varbinary](max) NULL,
	[ReportPath] [nvarchar](500) NULL,
	[ReportGroupID] [int] NULL,
	[ModuleComponentID] [bigint] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED 
	(
		[ReportID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Core].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_ReportType] FOREIGN KEY([ReportTypeID]) REFERENCES [Reference].[ReportType] ([ReportTypeID])
GO
ALTER TABLE [Core].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_ReportGroup] FOREIGN KEY([ReportGroupID]) REFERENCES [Reference].[ReportGroup] ([ReportGroupID])
GO
ALTER TABLE Core.Reports WITH CHECK ADD CONSTRAINT [FK_Reports_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Reports CHECK CONSTRAINT [FK_Reports_UserModifedBy]
GO
ALTER TABLE Core.Reports WITH CHECK ADD CONSTRAINT [FK_Reports_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Reports CHECK CONSTRAINT [FK_Reports_UserCreatedBy]
GO
ALTER TABLE [Core].[Reports]  WITH CHECK ADD CONSTRAINT [FK_Reports_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO
