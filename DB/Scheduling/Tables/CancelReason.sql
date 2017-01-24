
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[CancelReason]
-- Author:		Satish Singh
-- Date:		02/11/2016
--
-- Purpose:		Appointment cancel Reason
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/11/2016	Satish Singh	 Initial creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[CancelReason]
(
	[ReasonID] INT NOT NULL IDENTITY ,
	[Reason] [nvarchar](250) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_ReasonID] PRIMARY KEY CLUSTERED 
	(
		[ReasonID] ASC
	)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Scheduling.CancelReason WITH CHECK ADD CONSTRAINT [FK_CancelReason_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.CancelReason CHECK CONSTRAINT [FK_CancelReason_UserModifedBy]
GO
ALTER TABLE Scheduling.CancelReason WITH CHECK ADD CONSTRAINT [FK_CancelReason_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.CancelReason CHECK CONSTRAINT [FK_CancelReason_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Cancel Reason', 
@level0type = N'SCHEMA', @level0name = Scheduling, 
@level1type = N'TABLE',  @level1name = CancelReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the reason for cancelling an appointment', 
@level0type = N'SCHEMA', @level0name = Scheduling, 
@level1type = N'TABLE',  @level1name = CancelReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Scheduling, 
@level1type = N'TABLE',  @level1name = CancelReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Scheduling, 
@level1type = N'TABLE',  @level1name = CancelReason;
GO;