-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[EligibilityDuration]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		ECI EligibilityDuration lookup
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu TFS:2700	Initial Creation
-- 10/13/2015   Justin Spalti - Added a SortOrder column to use in the order by clause
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[EligibilityDuration](
	[EligibilityDurationID] [int] IDENTITY(1,1) NOT NULL,
	[EligibilityDuration] [nvarchar](100) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_EligibilityDuration_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_EligibilityDuration_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_EligibilityDuration_EligibilityDurationID] PRIMARY KEY CLUSTERED 
(
	[EligibilityDurationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_EligibilityDuration] UNIQUE NONCLUSTERED 
(
	[EligibilityDuration] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ECI.EligibilityDuration WITH CHECK ADD CONSTRAINT [FK_EligibilityDuration_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.EligibilityDuration CHECK CONSTRAINT [FK_EligibilityDuration_UserModifedBy]
GO
ALTER TABLE ECI.EligibilityDuration WITH CHECK ADD CONSTRAINT [FK_EligibilityDuration_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.EligibilityDuration CHECK CONSTRAINT [FK_EligibilityDuration_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Eligibility Duration', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = EligibilityDuration;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating length of eligibility session', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = EligibilityDuration;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = EligibilityDuration;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = ECI, 
@level1type = N'TABLE',  @level1name = EligibilityDuration;
GO;