-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateADUserSyncUsersHierarchy]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Update the User hierarchy from Users for AD Services. Used in ADUserSync SSIS package.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--03/01/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateADUserSyncUsersHierarchy]
@UserID  int,
@ParentID bigint, 
@IsActive bit, 
@ModifiedBy int,
@ModifiedOn datetime,
@SystemModifiedOn datetime

AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE r
	SET 
		ParentID = @ParentID ,
		IsActive = @IsActive, 
		ModifiedBy = @ModifiedBy, 
		ModifiedOn = @ModifiedOn, 
		SystemModifiedOn = GETUTCDATE()
	FROM Core.[UsersHierarchyMapping] r
	WHERE	UserID = @UserID
	AND		ParentID = @ParentID 
END
