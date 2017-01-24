-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[ClientStatus]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Lookup Table for Client Status
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/08/2016	John Crossen	TFS#6105 - Initial creation.
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [CallCenter].[ClientStatus](
[ClientStatusID] [SMALLINT] IDENTITY(1,1) NOT NULL,
[ClientStatus] [NVARCHAR](75) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ClientStatus] PRIMARY KEY CLUSTERED 
(
	[ClientStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE CallCenter.ClientStatus WITH CHECK ADD CONSTRAINT [FK_ClientStatus_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.ClientStatus CHECK CONSTRAINT [FK_ClientStatus_UserModifedBy]
GO
ALTER TABLE CallCenter.ClientStatus WITH CHECK ADD CONSTRAINT [FK_ClientStatus_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.ClientStatus CHECK CONSTRAINT [FK_ClientStatus_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Client Status', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = ClientStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating client''s current activity within the system', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = ClientStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = ClientStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = ClientStatus;
GO;