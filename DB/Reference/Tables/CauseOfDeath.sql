CREATE TABLE [Reference].[CauseOfDeath] (
    [CauseOfDeathID]   INT            IDENTITY (1, 1) NOT NULL,
    [CauseOfDeathName] NVARCHAR (200) NOT NULL,
    [IsActive]         BIT            NOT NULL,
    [ModifiedBy]       INT            NOT NULL,
    [ModifiedOn]       DATETIME       CONSTRAINT [DF_CauseOfDeath_ModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            CONSTRAINT [DF_CauseOfDeath_CreatedBy] DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME       CONSTRAINT [DF_CauseOfDeath_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME       CONSTRAINT [DF_CauseOfDeath_SystemCreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME       CONSTRAINT [DF_CauseOfDeath_SystemModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Reference.CauseOfDeath] PRIMARY KEY CLUSTERED ([CauseOfDeathID] ASC),
    CONSTRAINT [FK_CauseOfDeath_UserCreatedBy] FOREIGN KEY ([CauseOfDeathID]) REFERENCES [Core].[Users] ([UserID]),
    CONSTRAINT [FK_CauseOfDeath_UserModifiedBy] FOREIGN KEY ([CauseOfDeathID]) REFERENCES [Core].[Users] ([UserID])
);

