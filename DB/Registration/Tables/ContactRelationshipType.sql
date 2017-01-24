-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[ContactRelationshipType]
-- Author:		Lokesh Singhal
-- Date:		06/07/2016
--
-- Purpose:		Store collateral relationship type data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/07/2016	Lokesh Singhal	Initial Creation
--09/14/2016    Arun Choudhary	Added EffectiveDate and ExpirationDate
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactRelationshipType]
(
	[ContactRelationshipTypeID] BIGINT NOT NULL IDENTITY(1,1), 
    [ContactRelationshipID] BIGINT NOT NULL, 
	[RelationshipTypeID] INT NOT NULL, 
	[IsPolicyHolder] BIT NULL,
	[OtherRelationship] NVARCHAR (200) NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemCreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [SystemModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),	
	CONSTRAINT [PK_ContactRelationshipTypeID] PRIMARY KEY(ContactRelationshipTypeID),
	CONSTRAINT [FK_ContactRelationshipType_ContactRelationshipID]	FOREIGN KEY ([ContactRelationshipID]) REFERENCES [Registration].[ContactRelationship] ([ContactRelationshipID]),
	CONSTRAINT [FK_ContactRelationshipType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ContactRelationshipType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ContactRelationshipType_RelationshipType] FOREIGN KEY([RelationshipTypeID]) REFERENCES [Reference].[RelationshipType] ([RelationshipTypeID])	
)
GO