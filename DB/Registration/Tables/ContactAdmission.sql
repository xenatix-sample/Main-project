-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Registration].[ContactAdmission]
-- Author:		Scott Martin
-- Date:		03/21/2016
--
-- Purpose:		Link Between Contact and Organization
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Scott Martin		Initial creation.
-- 03/25/2016	Scott Martin		Removed DischargeDate and IsDischarged
-- 03/29/2016	Scott Martin		Merged EffectiveDate and EffectiveStartTime
-- 08/26/2016	Scott Martin	Added index
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactAdmission](
	[ContactAdmissionID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ContactID] [BIGINT] NOT NULL,
	[OrganizationID] [BIGINT] NOT NULL,
	[EffectiveDate] [DATETIME] NULL,
	[UserID] [INT] NOT NULL,
	[IsDocumentationComplete] [BIT] NOT NULL DEFAULT(0),
	[Comments] NVARCHAR(255) NULL,
	[AdmissionReasonID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactAdmission] PRIMARY KEY CLUSTERED 
(
	[ContactAdmissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_ContactAdmission_ContactID] ON [Registration].[ContactAdmission]
(
	ContactID ASC,
	EffectiveDate ASC
)
INCLUDE (SystemCreatedOn, SystemModifiedOn) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Registration].[ContactAdmission]  WITH CHECK ADD  CONSTRAINT [FK_ContactAdmission_Contact] FOREIGN KEY([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE [Registration].[ContactAdmission] CHECK CONSTRAINT [FK_ContactAdmission_Contact]
GO
ALTER TABLE [Registration].[ContactAdmission]  WITH CHECK ADD  CONSTRAINT [FK_ContactAdmission_OrganizationID] FOREIGN KEY([OrganizationID]) REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
GO
ALTER TABLE [Registration].[ContactAdmission] CHECK CONSTRAINT [FK_ContactAdmission_OrganizationID]
GO
ALTER TABLE Registration.ContactAdmission WITH CHECK ADD CONSTRAINT [FK_ContactAdmission_UserID] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactAdmission CHECK CONSTRAINT [FK_ContactAdmission_UserID]
GO
ALTER TABLE Registration.ContactAdmission WITH CHECK ADD CONSTRAINT [FK_ContactAdmission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactAdmission CHECK CONSTRAINT [FK_ContactAdmission_UserModifedBy]
GO
ALTER TABLE Registration.ContactAdmission WITH CHECK ADD CONSTRAINT [FK_ContactAdmission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactAdmission CHECK CONSTRAINT [FK_ContactAdmission_UserCreatedBy]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'PK of Registration.Contact table', 
@level0type = N'SCHEMA', @level0name = 'Registration', 
@level1type = N'TABLE',  @level1name = 'ContactAdmission',
@level2type = N'COLUMN', @level2name = 'ContactID';
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'PK of Core.OrganizationDetailsMapping table', 
@level0type = N'SCHEMA', @level0name = 'Registration', 
@level1type = N'TABLE',  @level1name = 'ContactAdmission',
@level2type = N'COLUMN', @level2name = 'OrganizationID';
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Date contact was admitted', 
@level0type = N'SCHEMA', @level0name = 'Registration', 
@level1type = N'TABLE',  @level1name = 'ContactAdmission',
@level2type = N'COLUMN', @level2name = 'EffectiveDate';
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Admitting User: PK of Core.Users table', 
@level0type = N'SCHEMA', @level0name = 'Registration', 
@level1type = N'TABLE',  @level1name = 'ContactAdmission',
@level2type = N'COLUMN', @level2name = 'UserID';
GO
