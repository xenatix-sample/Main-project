 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[RecipientCode]
-- Author:		Sumana Sangapu
-- Date:		01/19/2016
--
-- Purpose:		Lookup for RecipientCode 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	Sumana Sangapu		Initial Creation.
-- 07/06/2016	Sumana Sangapu	Added LegacyCode for CMHC
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[RecipientCode](
	[CodeID] [smallint] IDENTITY(1,1) NOT NULL,
	[Code] [smallint] NOT NULL,
	[CodeDescription] [nvarchar](75) NOT NULL,
	[LegacyCode] nvarchar(10) NULL,
	[LegacyCodeDescription] nvarchar(100) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_RecipientCode] PRIMARY KEY CLUSTERED 
(
	[CodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.RecipientCode WITH CHECK ADD CONSTRAINT [FK_RecipientCode_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.RecipientCode CHECK CONSTRAINT [FK_RecipientCode_UserModifedBy]
GO
ALTER TABLE Reference.RecipientCode WITH CHECK ADD CONSTRAINT [FK_RecipientCode_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.RecipientCode CHECK CONSTRAINT [FK_RecipientCode_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Recipient Code', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = RecipientCode;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the recipient of a service', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = RecipientCode;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = RecipientCode;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = RecipientCode;
GO;
