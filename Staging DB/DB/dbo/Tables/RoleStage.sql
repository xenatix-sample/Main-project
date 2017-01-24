-----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[RoleStage]
-- Author:		Sumana Sangapu
-- Date:		
--
-- Purpose:		Staging table to hold the roles from active directory 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/29/2016	Sumana Sangapu	Initial creation. 
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [dbo].[RoleStage] (
    [RoleID]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [BatchID]          BIGINT           NOT NULL,
    [RoleGUID]         VARBINARY (5000) NULL,
    [Name]             NVARCHAR (250)   NULL,
    [Description]      NVARCHAR (1000)  NULL,
    [IsActive]         BIT              DEFAULT ((1)) NULL,
    [ModifiedBy]       INT              NOT NULL,
    [ModifiedOn]       DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT              DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME         DEFAULT (getutcdate()) NOT NULL,
	[RoleGUIDConverted] [nvarchar](500) NULL,
    CONSTRAINT [PK_RoleStage_RoleID] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);


GO

