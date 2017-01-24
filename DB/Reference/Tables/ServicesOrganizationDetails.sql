-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ServicesOrganizationDetails]
-- Author:		Scott Martin
-- Date:		12/27/2016
--
-- Purpose:		Services mapped to Organization DetailID lookup table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/27/2016	Scott Martin	Initital Creation .
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ServicesOrganizationDetails](
	[ServicesOrganizationDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[ServicesID] [int] NOT NULL,
	[DetailID] [bigint] NOT NULL,
	[EffectiveDate] [date] NULL,
	[ExpirationDate] [date] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ServicesOrganizationDetailsID] PRIMARY KEY CLUSTERED 
(
	[ServicesOrganizationDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ServicesOrganizationDetails] UNIQUE NONCLUSTERED 
(
	[ServicesID] ASC,
	[DetailID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ServicesOrganizationDetails]  WITH CHECK ADD  CONSTRAINT [FK_ServicesOrganizationDetails_Services] FOREIGN KEY([ServicesID])
REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails] CHECK CONSTRAINT [FK_ServicesOrganizationDetails_Services] 
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails]  WITH CHECK ADD  CONSTRAINT [FK_ServicesOrganizationDetails_DetailID] FOREIGN KEY([DetailID])
REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails] CHECK CONSTRAINT [FK_ServicesOrganizationDetails_DetailID]
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails]  WITH CHECK ADD  CONSTRAINT [FK_ServicesOrganizationDetails_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails] CHECK CONSTRAINT [FK_ServicesOrganizationDetails_UserCreatedBy]
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails]  WITH CHECK ADD  CONSTRAINT [FK_ServicesOrganizationDetails_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ServicesOrganizationDetails] CHECK CONSTRAINT [FK_ServicesOrganizationDetails_UserModifedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Services Organization Details', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServicesOrganizationDetails;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the services associated with Organization Details', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServicesOrganizationDetails;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServicesOrganizationDetails;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServicesOrganizationDetails;
GO;