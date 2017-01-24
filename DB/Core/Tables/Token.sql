-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Token]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn	
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Token] (
    [TokenID]     INT IDENTITY(1,1) NOT NULL,
    [Token]       NVARCHAR (100) NOT NULL,
    [IP]          NVARCHAR (15)  NOT NULL,
    [UserID]      INT           NOT NULL,
    [EffectiveTo] DATETIME      NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Token_TokenID] PRIMARY KEY CLUSTERED ([TokenID] ASC)
);
GO

ALTER TABLE Core.Token WITH CHECK ADD CONSTRAINT [FK_Token_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Token CHECK CONSTRAINT [FK_Token_UserModifedBy]
GO
ALTER TABLE Core.Token WITH CHECK ADD CONSTRAINT [FK_Token_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Token CHECK CONSTRAINT [FK_Token_UserCreatedBy]
GO

