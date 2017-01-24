-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Activity]
-- Author:		Rajiv Ranjan
-- Date:		07/30/2015
--
-- Purpose:		Common for activity details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/30/2015	Rajiv Ranjan	TFS# - Initial creation.
-- 07/30/2015	Suresh Pandey	TFS# - Add IsActive column, modified by not null, changed Message from max to 1000
--								[Source] from max to 250,[Comments] from max to 250,[IpAddress] from max to 200,[PageUrl] from max to 250
--								[[ReferrerUrl]] from max to 250, Change the primary key naming conventions
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Activity](
	[ActivityID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](250) NULL,
	[FunctionName] [nvarchar](250) NULL,
	[Comments] [nvarchar](1000) NULL,
	[IpAddress] [nvarchar](200) NULL,
	[PageUrl] [nvarchar](250) NULL,
	[ReferrerUrl] [nvarchar](250) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),	
 CONSTRAINT [PK_Activity_ActivityID] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.Activity WITH CHECK ADD CONSTRAINT [FK_Activity_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Activity CHECK CONSTRAINT [FK_Activity_UserModifedBy]
GO
ALTER TABLE Core.Activity WITH CHECK ADD CONSTRAINT [FK_Activity_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Activity CHECK CONSTRAINT [FK_Activity_UserCreatedBy]
GO



