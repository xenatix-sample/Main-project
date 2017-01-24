-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Phone]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Store Phone data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 07/12/2016	Scott Martin		Increased length of extension field
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[Phone] (
    [PhoneID]     BIGINT        NOT NULL	IDENTITY (1, 1) ,
    [PhoneTypeID] INT           NULL,
    [Number]      NVARCHAR (50) NOT NULL,
    [Extension]   NVARCHAR (10)  NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Phone_PhoneID]				PRIMARY KEY CLUSTERED ([PhoneID] ASC),
    CONSTRAINT [FK_Phone_PhoneTypeID]			FOREIGN KEY ([PhoneTypeID]) REFERENCES [Reference].[PhoneType] ([PhoneTypeID])
)
GO

ALTER TABLE Core.Phone WITH CHECK ADD CONSTRAINT [FK_Phone_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Phone CHECK CONSTRAINT [FK_Phone_UserModifedBy]
GO
ALTER TABLE Core.Phone WITH CHECK ADD CONSTRAINT [FK_Phone_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Phone CHECK CONSTRAINT [FK_Phone_UserCreatedBy]
GO




