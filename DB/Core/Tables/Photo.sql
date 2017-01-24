-----------------------------------------------------------------------------------------------------------------------
-- Table:		Registration.[Photo]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Store Photo Data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/29/2015	Scott Martin		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/24/2016	Scott Martin		Moved from Registration to Core schema
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Core.Photo
(
	PhotoID					BIGINT IDENTITY(1,1) NOT NULL,
	PhotoBLOB				VARBINARY(MAX) NOT NULL,
	ThumbnailBLOB			VARBINARY(MAX) NOT NULL,
	TakenBy					INT NOT NULL,
	TakenTime				DATETIME NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Photo_PhotoID] PRIMARY KEY CLUSTERED 
(
	[PhotoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.Photo WITH CHECK ADD CONSTRAINT [FK_Photo_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Photo CHECK CONSTRAINT [FK_Photo_UserModifedBy]
GO
ALTER TABLE Core.Photo WITH CHECK ADD CONSTRAINT [FK_Photo_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Photo CHECK CONSTRAINT [FK_Photo_UserCreatedBy]
GO
