-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[NoteHeaderVoidReason]
-- Author:		Scott Martin
-- Date:		04/05/2016
--
-- Purpose:		Lookup Table for Note Header Void Reasons
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	Scott Martin	Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[NoteHeaderVoidReason](
	[NoteHeaderVoidReasonID] [SMALLINT] IDENTITY(1,1) NOT NULL,
	[NoteHeaderVoidReason] [NVARCHAR](75) NOT NULL,
	[SortOrder] INT NOT NULL,
	[IsSystem] BIT NOT NULL DEFAULT(0),
	[IsActive] BIT NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_NoteHeaderVoidReason] PRIMARY KEY CLUSTERED 
(
	[NoteHeaderVoidReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.NoteHeaderVoidReason WITH CHECK ADD CONSTRAINT [FK_NoteHeaderVoidReason_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.NoteHeaderVoidReason CHECK CONSTRAINT [FK_NoteHeaderVoidReason_UserModifedBy]
GO
ALTER TABLE Reference.NoteHeaderVoidReason WITH CHECK ADD CONSTRAINT [FK_NoteHeaderVoidReason_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.NoteHeaderVoidReason CHECK CONSTRAINT [FK_NoteHeaderVoidReason_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Note Header Void Reason', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = NoteHeaderVoidReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the reason for voiding a note', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = NoteHeaderVoidReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = NoteHeaderVoidReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = NoteHeaderVoidReason;
GO;
