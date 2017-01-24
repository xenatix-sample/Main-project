-----------------------------------------------------------------------------------------------------------------------
-- Table:	[Registration].[ContactRelationship]
-- Author:		John Crossen
-- Date:		08/19/2015
--
-- Purpose:		Contact Relationship Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/19/2015	John Crossen		TFS#705  - Initial creation.
-- 09/04/2015	Gurpreet Singh		TFS#1860  - Updating IsLivingWithClient datatype to INT
-- 09/21/2015	Avikal				TFS#1963  - Added LivingWithClientStatus,ReceiveCorrespondenceID, removed LivingWithClientID
-- 01/12/2016	Gaurav Gupta		TFS#n/a  - Added EducationStatusID,EmploymentStatusID
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/17/2016   Arun Choudhary  Added SchoolAttended,SchoolBeginDate,SchoolEndDate
-- 04/12/2016   Arun Choudhary	Added IsPolicyHolder
-- 06/07/2016   Lokesh Singhal	Removed IsPolicyHolder,OtherRelationship,RelationshipTypeID and related foreign key constraint
--09/14/2016    Arun Choudhary	Added CollateralEffectiveDate and CollateralExpirationDate
-- 01/18/2017	Sumana Sangapu	Added ReferralHeaderID 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactRelationship](
	[ContactRelationshipID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ParentContactID] [BIGINT] NOT NULL,
	[ChildContactID] [BIGINT] NOT NULL,
	[ContactTypeID] [INT] NOT NULL,
	[PhonePermissionID] [INT] NULL,
	[EmailPermissionID] [INT] NULL,
	[ReceiveCorrespondenceID] INT NULL,
	[IsEmergency] BIT NOT NULL DEFAULT(0),
	[EducationStatusID]	INT NULL ,
	[SchoolAttended] NVARCHAR(250) NULL,
	[SchoolBeginDate] DATE NULL,
	[SchoolEndDate] DATE NULL,
	[EmploymentStatusID] INT NULL ,
	[VeteranStatusID] [int] NULL,
    [LivingWithClientStatus] BIT NULL,
	[CollateralEffectiveDate] DATE NULL,
	[CollateralExpirationDate] DATE NULL,
	[ReferralHeaderID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ContactRelationship] PRIMARY KEY CLUSTERED 
(
	[ContactRelationshipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--CREATE UNIQUE NONCLUSTERED INDEX [IX_ContactRelationship_ParentContactID] ON [Registration].[ContactRelationship]
--(
--	[ParentContactID] ASC,
--	[ChildContactID] ASC,
--	[RelationshipTypeID] ASC,
--	[ContactTypeID] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_ContactChild] FOREIGN KEY([ChildContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_ContactChild]
GO


ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_ContactParent] FOREIGN KEY([ParentContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_ContactParent]
GO


ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_ContactType] FOREIGN KEY([ContactTypeID])
REFERENCES [Reference].[ContactType] ([ContactTypeID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_ContactType]
GO


ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_EmailPermission] FOREIGN KEY([EmailPermissionID])
REFERENCES [Reference].[EmailPermission] ([EmailPermissionID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_EmailPermission]
GO


ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_PhonePermission] FOREIGN KEY([PhonePermissionID])
REFERENCES [Reference].[PhonePermission] ([PhonePermissionID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_PhonePermission]
GO



ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_Users] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_Users]
GO

ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_LivingWithClientStatusID] FOREIGN KEY([ReceiveCorrespondenceID])
REFERENCES [Reference].[LivingWithClientStatus] ([LivingWithClientStatusID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_LivingWithClientStatusID]
GO

ALTER TABLE Registration.ContactRelationship WITH CHECK ADD CONSTRAINT [FK_ContactRelationship_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactRelationship CHECK CONSTRAINT [FK_ContactRelationship_UserModifedBy]
GO
ALTER TABLE Registration.ContactRelationship WITH CHECK ADD CONSTRAINT [FK_ContactRelationship_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactRelationship CHECK CONSTRAINT [FK_ContactRelationship_UserCreatedBy]
GO

ALTER TABLE [Registration].[ContactRelationship]  WITH CHECK ADD  CONSTRAINT [FK_ContactRelationship_ReferralHeaderID] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader] ([ReferralHeaderID])
GO

ALTER TABLE [Registration].[ContactRelationship] CHECK CONSTRAINT [FK_ContactRelationship_ReferralHeaderID]
GO