-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ScreeningName]
-- Author:		John Crossen
-- Date:		09/30/2015
--
-- Purpose:		ECI ScreeninName Table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	John Crossen     TFS:2542		Created .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ScreeningName](
	[ScreeningNameID] BIGINT IDENTITY(1,1) NOT NULL,
	[ScreeningTypeID] [SMALLINT] NOT NULL,
	[ScreeningName] [NVARCHAR](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ScreeningName] PRIMARY KEY CLUSTERED 
(
	[ScreeningNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ScreeningName]  WITH CHECK ADD  CONSTRAINT [FK_ScreeningName_ScreeningType] FOREIGN KEY([ScreeningTypeID])
REFERENCES [ECI].[ScreeningType] ([ScreeningTypeID])
GO

ALTER TABLE [ECI].[ScreeningName] CHECK CONSTRAINT [FK_ScreeningName_ScreeningType]
GO
ALTER TABLE ECI.ScreeningName WITH CHECK ADD CONSTRAINT [FK_ScreeningName_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningName CHECK CONSTRAINT [FK_ScreeningName_UserModifedBy]
GO
ALTER TABLE ECI.ScreeningName WITH CHECK ADD CONSTRAINT [FK_ScreeningName_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningName CHECK CONSTRAINT [FK_ScreeningName_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Screening Name', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningName;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating name of screening', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningName;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningName;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = ScreeningName;
GO;