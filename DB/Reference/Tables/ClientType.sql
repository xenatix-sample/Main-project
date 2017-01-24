-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ClientType]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Lookup for ClientType  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TFS# 675 - Initial creation.
-- 07/24/2015   John Crossen    Change IsActive to NOT NULL and add default value
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 12/14/2015   Gaurav Gupta    4031 - Added new Column SortOrder
-- 01/05/2016   Justin Spalti   Added the RegistrationState columnn
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ClientType](
	[ClientTypeID] [int] IDENTITY (1,1) NOT NULL,
	[ClientType] [nvarchar](200) NOT NULL,
	[OrganizationDetailID] BIGINT NOT NULL,
	[RegistrationState] VARCHAR(100) NOT NULL DEFAULT('registration.initialdemographics'),
	[SortOrder] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ClientType_ClientTypeID] PRIMARY KEY CLUSTERED 
(
	[ClientTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [Reference].[ClientType] ADD CONSTRAINT IX_ClientType UNIQUE(ClientType)
GO

ALTER TABLE Reference.ClientType WITH CHECK ADD CONSTRAINT [FK_ClientType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ClientType CHECK CONSTRAINT [FK_ClientType_UserModifedBy]
GO
ALTER TABLE Reference.ClientType WITH CHECK ADD CONSTRAINT [FK_ClientType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ClientType CHECK CONSTRAINT [FK_ClientType_UserCreatedBy]
GO
ALTER TABLE Reference.ClientType WITH CHECK ADD CONSTRAINT [FK_ClientType_OrganizationDetailID] FOREIGN KEY([OrganizationDetailID]) REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO
ALTER TABLE Reference.ClientType CHECK CONSTRAINT [FK_ClientType_OrganizationDetailID]
GO
