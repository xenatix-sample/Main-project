-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[AllergySymptom]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Lookup for AllergySymptoms
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/27/2015	John Crossen	TFS# 2891 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Clinical.[AllergySymptom](
	[AllergySymptomID] [int] IDENTITY(1,1) NOT NULL,
	[AllergySymptom] [nvarchar](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AllergySymptom] PRIMARY KEY CLUSTERED 
(
	[AllergySymptomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AllergySymptom_AllergySymptom] ON Clinical.[AllergySymptom]
(
	[AllergySymptom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Clinical.AllergySymptom WITH CHECK ADD CONSTRAINT [FK_AllergySymptom_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.AllergySymptom CHECK CONSTRAINT [FK_AllergySymptom_UserModifedBy]
GO
ALTER TABLE Clinical.AllergySymptom WITH CHECK ADD CONSTRAINT [FK_AllergySymptom_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.AllergySymptom CHECK CONSTRAINT [FK_AllergySymptom_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Allergy Symptom', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = AllergySymptom;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating allergy symptoms', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = AllergySymptom;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = AllergySymptom;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = AllergySymptom;
GO;