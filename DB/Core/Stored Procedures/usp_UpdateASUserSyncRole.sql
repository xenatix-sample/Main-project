

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateADUserSyncRole]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Update the UserRole from Users for AD Services. Used in ADUserSync SSIS package.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--03/01/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateADUserSyncRole]
@Name nvarchar(100),
@RoleGUID  nvarchar(500),
@Description nvarchar(50), 
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
		[Description] = @Description,
		IsActive = @IsActive, 
		ModifiedBy = @ModifiedBy, 
		ModifiedOn = @ModifiedOn, 
		SystemModifiedOn = GETUTCDATE()
	FROM Core.[Role] r
	WHERE	r.Name = @Name 
	AND		r.RoleGUID = @RoleGUID
 
END
