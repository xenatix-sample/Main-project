-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ESignature].[Signatures]
-- Author:		Sumana Sangapu
-- Date:		08/18/2015
--
-- Purpose:		Stores the Signatures  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/18/2015	Sumana Sangapu	914 - Initial Creation
-- 08/20/2015   Justin Spalti - Updated the schema to ESignature
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ESignature].[Signatures](
	[SignatureID] BIGINT IDENTITY(1,1) NOT NULL,
	[SignatureBLOB] [varbinary] (max) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Signatures_SID] PRIMARY KEY CLUSTERED 
(
	[SignatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO

ALTER TABLE ESignature.Signatures WITH CHECK ADD CONSTRAINT [FK_Signatures_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.Signatures CHECK CONSTRAINT [FK_Signatures_UserModifedBy]
GO
ALTER TABLE ESignature.Signatures WITH CHECK ADD CONSTRAINT [FK_Signatures_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.Signatures CHECK CONSTRAINT [FK_Signatures_UserCreatedBy]
GO
