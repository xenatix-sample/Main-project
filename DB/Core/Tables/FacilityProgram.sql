-----------------------------------------------------------------------------------------------------------------------
-- Table:		[FacilityProgram]
-- Author:		John Crossen
-- Date:		09/10/2015
--
-- Purpose:		Facility Program mapping table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/10/2015	John Crossen	TFS# 2229 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[FacilityProgram](
	[FacilityProgramID] [BIGINT] NOT NULL IDENTITY(1,1),
	[FacilityID] [INT] NOT NULL,
	[ProgramID] [INT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_FacilityProgramID] PRIMARY KEY CLUSTERED 
(
	[FacilityProgramID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [Core].[FacilityProgram]  WITH CHECK ADD  CONSTRAINT [FK_FacilityProgram_Program] FOREIGN KEY([ProgramID])
REFERENCES [Reference].[Program] ([ProgramID])
GO

ALTER TABLE [Core].[FacilityProgram] CHECK CONSTRAINT [FK_FacilityProgram_Program]
GO

ALTER TABLE [Core].[FacilityProgram]  WITH CHECK ADD  CONSTRAINT [FK_FacilityProgram_Facility] FOREIGN KEY([FacilityID])
REFERENCES [Reference].[Facility] ([FacilityID])
GO

ALTER TABLE [Core].[FacilityProgram] CHECK CONSTRAINT [FK_FacilityProgram_Facility]
GO

ALTER TABLE Core.FacilityProgram WITH CHECK ADD CONSTRAINT [FK_FacilityProgram_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.FacilityProgram CHECK CONSTRAINT [FK_FacilityProgram_UserModifedBy]
GO
ALTER TABLE Core.FacilityProgram WITH CHECK ADD CONSTRAINT [FK_FacilityProgram_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.FacilityProgram CHECK CONSTRAINT [FK_FacilityProgram_UserCreatedBy]
GO
