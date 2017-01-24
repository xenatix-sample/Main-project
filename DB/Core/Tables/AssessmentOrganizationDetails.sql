-----------------------------------------------------------------------------------------------------------------------
-- Table:		Core.AssessmentOrganizationDetails
-- Author:		Kyle Campbell
-- Date:		03/15/2016
--
-- Purpose:		Maps Assessments to OrganizationDetails  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/15/2016	Kyle Campbell	TFS #7237	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE Core.AssessmentOrganizationDetails
(
	[AssessmentOrganizationDetailsID] BIGINT NOT NULL IDENTITY(1,1),
	[AssessmentID] BIGINT NOT NULL,
	[DetailID] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT (1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT (1),
	[CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),	
	CONSTRAINT [PK_AssessmentOrganizationDetails] PRIMARY KEY CLUSTERED ([AssessmentOrganizationDetailsID] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.AssessmentOrganizationDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentOrganizationDetails_AssessmentID] FOREIGN KEY (AssessmentID) REFERENCES Core.Assessments (AssessmentID)
GO

ALTER TABLE Core.AssessmentOrganizationDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentOrganizationDetails_UserModifiedBy] FOREIGN KEY (ModifiedBy) REFERENCES Core.Users (UserID)
GO

ALTER TABLE Core.AssessmentOrganizationDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentOrganizationDetails_UserCreatedBy] FOREIGN KEY (CreatedBy) REFERENCES Core.Users (UserID)
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References AssessmentID from Core.Assessments', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = Assessments,
@level2type = N'COLUMN', @level2name = AssessmentID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References DetailsID from Core.OrganizationDetails', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = OrganizationDetails,
@level2type = N'COLUMN', @level2name = DetailID;
GO