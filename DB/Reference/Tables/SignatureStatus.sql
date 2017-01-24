-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[SignatureStatus]
-- Author:		Kyle Campbell
-- Date:		03/15/2016
--
-- Purpose:		Signature Status values  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/15/2016	Kyle Campbell	TFS #7237	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Reference].[SignatureStatus]
(
	[SignatureStatusID] INT NOT NULL IDENTITY(1,1),
	[SignatureStatus] NVARCHAR(50) NOT NULL,
	[SortOrder] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT (1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT (1),
	[CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),	
	CONSTRAINT [PK_SignatureStatus] PRIMARY KEY CLUSTERED ([SignatureStatusID] ASC) 
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Reference].[SignatureStatus] ADD CONSTRAINT IX_SignatureStatus UNIQUE(SignatureStatus)
GO
ALTER TABLE [Reference].[SignatureStatus] WITH CHECK ADD CONSTRAINT [FK_SignatureStatus_UserModifiedBy] FOREIGN KEY (ModifiedBy) REFERENCES Core.Users (UserID)
GO
ALTER TABLE [Reference].[SignatureStatus] WITH CHECK ADD CONSTRAINT [FK_SignatureStatus_UserCreatedBy] FOREIGN KEY (CreatedBy) REFERENCES Core.Users (UserID)
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Signature Status values', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SignatureStatus,
@level2type = N'COLUMN', @level2name = SignatureStatus;
GO

EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Signature Status', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SignatureStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating if an item is signed', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SignatureStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SignatureStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SignatureStatus;
GO;
