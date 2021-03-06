-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[SuicideHomicide]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Lookup Table for SuicideHomicide
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	John Crossen	TFS#5409 - Initial creation.
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [CallCenter].[SuicideHomicide](
[SuicideHomicideID] [smallint] IDENTITY(1,1) NOT NULL,
[SuicideHomicide] [nvarchar](75) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 

 CONSTRAINT [PK_SuicideHomicide] PRIMARY KEY CLUSTERED 
(
	[SuicideHomicideID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE CallCenter.SuicideHomicide WITH CHECK ADD CONSTRAINT [FK_SuicideHomicide_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.SuicideHomicide CHECK CONSTRAINT [FK_SuicideHomicide_UserModifedBy]
GO
ALTER TABLE CallCenter.SuicideHomicide WITH CHECK ADD CONSTRAINT [FK_SuicideHomicide_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.SuicideHomicide CHECK CONSTRAINT [FK_SuicideHomicide_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Suicide/Homicide', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = SuicideHomicide;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating a person''s suicide/homicide risk', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = SuicideHomicide;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = SuicideHomicide;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = CallCenter, 
@level1type = N'TABLE',  @level1name = SuicideHomicide;
GO;