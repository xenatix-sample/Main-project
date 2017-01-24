-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ESignature].[SignatureTypes]
-- Author:		Sumana Sangapu
-- Date:		11/16/2015
--
-- Purpose:		Holds the Signature Types
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Sumana Sangapu		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ESignature].[SignatureTypes](
	[SignatureTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SignatureType] [nvarchar](50) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_SignatureTypes] PRIMARY KEY CLUSTERED 
(
	[SignatureTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ESignature.SignatureTypes WITH CHECK ADD CONSTRAINT [FK_SignatureTypes_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.SignatureTypes CHECK CONSTRAINT [FK_SignatureTypes_UserModifedBy]
GO
ALTER TABLE ESignature.SignatureTypes WITH CHECK ADD CONSTRAINT [FK_SignatureTypes_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ESignature.SignatureTypes CHECK CONSTRAINT [FK_SignatureTypes_UserCreatedBy]
GO
