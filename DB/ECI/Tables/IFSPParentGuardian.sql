-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE [ECI].[IFSPParentGuardian] (
    [ParentGuardianID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [IFSPID]           BIGINT   NOT NULL,
    [ContactID]        BIGINT   NOT NULL,
    [IsActive]         BIT      CONSTRAINT [DF_IFSPParentGuardian_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]       INT      NOT NULL,
    [ModifiedOn]       DATETIME CONSTRAINT [DF__IFSPParen__Modif__3A8CA01F] DEFAULT (getutcdate()) NOT NULL,
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK__IFSPPare__5C38C0FA27DDB8E2] PRIMARY KEY CLUSTERED ([ParentGuardianID] ASC),
    CONSTRAINT [FK_IFSPParentGuardian_IFSP] FOREIGN KEY ([IFSPID]) REFERENCES [ECI].[IFSP] ([IFSPID]),
    CONSTRAINT [FK_IFSPParentGuardian_Contact] FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
)

GO

ALTER TABLE ECI.IFSPParentGuardian WITH CHECK ADD CONSTRAINT [FK_IFSPParentGuardian_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.IFSPParentGuardian CHECK CONSTRAINT [FK_IFSPParentGuardian_UserModifedBy]
GO
ALTER TABLE ECI.IFSPParentGuardian WITH CHECK ADD CONSTRAINT [FK_IFSPParentGuardian_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.IFSPParentGuardian CHECK CONSTRAINT [FK_IFSPParentGuardian_UserCreatedBy]
GO






