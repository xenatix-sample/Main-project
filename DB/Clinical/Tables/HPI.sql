-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[[HPI]]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		HPI Header table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3665 - Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Clinical.[HPI](
	[HPIID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[EncounterID] [bigint] NULL,
	[TakenBy] [int] NOT NULL,
	[TakenTime] [datetime] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_HPI] PRIMARY KEY CLUSTERED 
(
	[HPIID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Clinical.[HPI] ADD  CONSTRAINT [DF_HPI_TakenTime]  DEFAULT (getutcdate()) FOR [TakenTime]
GO

ALTER TABLE Clinical.HPI WITH CHECK ADD CONSTRAINT [FK_HPI_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.HPI CHECK CONSTRAINT [FK_HPI_UserModifedBy]
GO
ALTER TABLE Clinical.HPI WITH CHECK ADD CONSTRAINT [FK_HPI_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.HPI CHECK CONSTRAINT [FK_HPI_UserCreatedBy]
GO
