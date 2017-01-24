-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[Drugs]
-- Author:		Sumana Sangapu
-- Date:		11/16/2015
--
-- Purpose:		Basic Drug Information
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Sumana Sangapu		Initial Creation.
-- 11/25/2015   Justin Spalti - Added the three default columns needed on every table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[Drugs](
	[DrugID] [int] IDENTITY(1,1) NOT NULL,
	[SetId] [uniqueidentifier] NULL,
	[Category] [varchar](100) NULL,
	[Packager] [varchar](100) NULL,
	[ndcCode] [varchar](100) NULL,
	[ProductName] [varchar](100) NULL,
	[ProductForm] [varchar](100) NULL,
	[GenericMedicine] [varchar](255) NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Drugs] PRIMARY KEY CLUSTERED 
(
	[DrugID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE Clinical.Drugs WITH CHECK ADD CONSTRAINT [FK_Drugs_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Drugs CHECK CONSTRAINT [FK_Drugs_UserModifedBy]
GO
ALTER TABLE Clinical.Drugs WITH CHECK ADD CONSTRAINT [FK_Drugs_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Drugs CHECK CONSTRAINT [FK_Drugs_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Drugs', 
@value = N'Drug Ingredients List', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = Drugs;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating drugs', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = Drugs;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = Drugs;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = Drugs;
GO;