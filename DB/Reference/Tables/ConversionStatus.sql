-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ConversionStatus]
-- Author:		John Crossen
-- Date:		01/18/2016
--
-- Purpose:		Lookup Table for Conversion Status
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	John Crossen	TFS#5465 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[ConversionStatus](
[ConversionStatusID] [SMALLINT] IDENTITY(1,1) NOT NULL,
[ConversionStatus] [NVARCHAR](75) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ConversionStatus] PRIMARY KEY CLUSTERED 
(
	[ConversionStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ConversionStatus WITH CHECK ADD CONSTRAINT [FK_ConversionStatus_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ConversionStatus CHECK CONSTRAINT [FK_ConversionStatus_UserModifedBy]
GO
ALTER TABLE Reference.ConversionStatus WITH CHECK ADD CONSTRAINT [FK_ConversionStatus_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ConversionStatus CHECK CONSTRAINT [FK_ConversionStatus_UserCreatedBy]
GO

