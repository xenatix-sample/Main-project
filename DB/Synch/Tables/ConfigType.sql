-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE [Synch].[ConfigType] (
    [ConfigTypeID]     INT           IDENTITY (1, 1) NOT NULL,
    [ConfigType]       NVARCHAR (50) NULL,
    [IsActive]         BIT           DEFAULT ((1)) NOT NULL,
    [ModifiedBy]       INT           NOT NULL,
    [ModifiedOn]       DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT           DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME      DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ConfigType_ConfigTypeID] PRIMARY KEY CLUSTERED ([ConfigTypeID] ASC),
    CONSTRAINT [IX_ConfigType] UNIQUE NONCLUSTERED ([ConfigType] ASC)
);
GO

ALTER TABLE Synch.ConfigType WITH CHECK ADD CONSTRAINT [FK_ConfigType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.ConfigType CHECK CONSTRAINT [FK_ConfigType_UserModifedBy]
GO
ALTER TABLE Synch.ConfigType WITH CHECK ADD CONSTRAINT [FK_ConfigType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.ConfigType CHECK CONSTRAINT [FK_ConfigType_UserCreatedBy]
GO


