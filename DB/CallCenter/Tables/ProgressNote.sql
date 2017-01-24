-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[ProgressNote]
-- Author:		Scott Martin
-- Date:		04/03/2016
--
-- Purpose:		Stores progress note data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/03/2016	Scott Martin	Initial creation.
-- 06/06/2016	Scott Martin	Increased the length of several fields
-- 07/07/2016	Rajiv Ranjan	Increased length of comments and disposition
-- 07/07/2016	Lokesh Singhal	Added IsCallerSame & NewCallerID fields
-- 07/16/2016	Rajiv Ranjan	Increased length of FollowupPlan and NatureofCall
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [CallCenter].[ProgressNote](
	[ProgressNoteID] [BIGINT] NOT NULL IDENTITY(1,1),
	[CallCenterHeaderID] [BIGINT] NOT NULL,
	[NoteHeaderID] bigint NULL,
	[Disposition] [NVARCHAR](MAX) NULL,
	[Comments] [NVARCHAR](MAX) NULL,
	[CallTypeID] SMALLINT NULL,
	[CallTypeOther] NVARCHAR (100) NULL,
	[FollowupPlan] NVARCHAR(MAX) NULL,
	[NatureofCall] NVARCHAR(MAX) NULL,
	[IsCallerSame] BIT NULL,
	[NewCallerID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
	    
 CONSTRAINT [PK_ProgressNote_ProgressNoteID] PRIMARY KEY CLUSTERED 
(
	[ProgressNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [CallCenter].[ProgressNote]  WITH CHECK ADD  CONSTRAINT [FK_ProgressNote_CallCenterHeader] FOREIGN KEY([CallCenterHeaderID])
REFERENCES [CallCenter].[CallCenterHeader] ([CallCenterHeaderID])
GO

ALTER TABLE [CallCenter].[ProgressNote] CHECK CONSTRAINT [FK_ProgressNote_CallCenterHeader]
GO

ALTER TABLE CallCenter.ProgressNote WITH CHECK ADD CONSTRAINT [FK_ProgressNote_UserModifiedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.ProgressNote CHECK CONSTRAINT [FK_ProgressNote_UserModifiedBy]
GO
ALTER TABLE CallCenter.ProgressNote WITH CHECK ADD CONSTRAINT [FK_ProgressNote_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.ProgressNote CHECK CONSTRAINT [FK_ProgressNote_UserCreatedBy]
GO
ALTER TABLE CallCenter.ProgressNote WITH CHECK ADD CONSTRAINT [FK_ProgressNote_CallerContactID] FOREIGN KEY([NewCallerID]) REFERENCES [Registration].[Contact] ([ContactID])
GO