-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[OrganizationDetailsMapping]
-- Author:		Sumana Sangapu
-- Date:		01/15/2016
--
-- Purpose:		Define the Parent/child relation 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	Sumana Sangapu		Initial Creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 12/12/2016	Kyle Campbell	TFS #17998	Add EffectiveDate and ExpirationDate columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationDetailsMapping](
	[MappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[DetailID] [bigint] NOT NULL,
	[ParentID] [bigint] NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_OrganizationDetailsMapping] PRIMARY KEY CLUSTERED 
(
	[MappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE [Core].[OrganizationDetailsMapping] WITH CHECK ADD  CONSTRAINT [FK_OrganizationDetailsMapping_DetailID] FOREIGN KEY([DetailID])
REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO

ALTER TABLE [Core].[OrganizationDetailsMapping] CHECK CONSTRAINT [FK_OrganizationDetailsMapping_DetailID]
GO

ALTER TABLE Core.OrganizationDetailsMapping WITH CHECK ADD CONSTRAINT [FK_OrganizationDetailsMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationDetailsMapping CHECK CONSTRAINT [FK_OrganizationDetailsMapping_UserModifedBy]
GO
ALTER TABLE Core.OrganizationDetailsMapping WITH CHECK ADD CONSTRAINT [FK_OrganizationDetailsMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationDetailsMapping CHECK CONSTRAINT [FK_OrganizationDetailsMapping_UserCreatedBy]
GO



