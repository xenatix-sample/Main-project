-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_InvokeADUserSyncProcess]
-- Author:		Sumana Sangapu
-- Date:		08/20/2016
--
-- Purpose:		Invokes the ADSync process. Syncs up Users, Addresses , Phone and Email details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/20/2016	Sumana Sangapu	Initial creation.
-- 09/03/2016	Rahul Vats		Reviewed the proc and make some changes to accomodate data conversion to happen properly
-- 10/13/2016	Rahul Vats		Added Configurable Verbosity
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_InvokeADUserSyncProcess]
	@Verbose INT = 1,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';
	BEGIN TRY
		-- @BatchStatusID 1	Requesting
		-- @BatchStatusID 2	InProgress
		-- @BatchStatusID 3	Completed
		-- @BatchStatusID 4	Successful
		-- @BatchStatusID 5	Failed
		DECLARE	
			@BatchStatusID int = 2, -- @BatchStatus = InProgress
			@BatchTypeID int = 1,  -- @BatchType = ADSynchService
			@ConfigID int = 1, --@ConfigID = LDAP - Should be @ConfigTypeID
			@USN int = 0, --We are not depending on USN now to detect changes -- Should consider removing it
			@IsActive bit = 1, --Bring users as IsActive? Update them if they are locked or disabled.
			@ModifiedBy int = 1, --HardCoded to 1 to show done by Import
			@PackageName nvarchar(100) = N'ADSynchService', --PackageName is ADSynchService
			@PackageTask nvarchar(50) = N'ADSynch', --PackageTask is ADSynch
			@ReturnValue int,
			@ID bigint,
			@ModifiedOn datetime = getutcdate(),
			@BatchID int
		--Create Batch -- Think if this is needed
		EXEC 
			@ReturnValue = [Synch].[usp_AddBatch]
			@BatchStatusID = @BatchStatusID, 
			@BatchTypeID = @BatchTypeID, 
			@ConfigID = @ConfigID,
			@USN = @USN, 
			@IsActive = @IsActive, 
			@ModifiedOn = @ModifiedOn, 
			@ModifiedBy = @ModifiedBy, 
			@ResultCode = @ResultCode OUTPUT, 
			@ResultMessage = @ResultMessage OUTPUT, 
			@ID = @ID OUTPUT
		Set @BatchID = @ID 
		--Create Batch Details-- Think if this is needed
		EXEC 
			@ReturnValue = [Synch].[usp_AddBatchDetails]
			@BatchID = @BatchID,
			@BatchStatusID = @BatchStatusID,
			@PackageName = @PackageName, --PackageName is ADSynchService
			@PackageTask = @PackageName, --PackageTask is ADSynch
			@ModifiedBy = @ModifiedBy,
			@CreatedBy = @ModifiedBy,
			@ResultCode = @ResultCode OUTPUT,
			@ResultMessage = @ResultMessage OUTPUT,
			@ID = @ID OUTPUT

		DECLARE @ActiveDirectoryPath varchar(max)
		DECLARE @ActiveDirectoryGroupName varchar(max)
		SELECT @ActiveDirectoryPath = convert(varchar(max),ConfigXML) from Synch.Config Where ConfigTypeID = 1 AND ConfigName = 'ActiveDirectoryPath' 
		SELECT @ActiveDirectoryGroupName = convert(varchar(max),ConfigXML) from Synch.Config Where ConfigTypeID = 1 AND ConfigName = 'ActiveDirectoryGroupName'

		--Fetch Data from The Active Directory and Dump into the Staging ADUser table.
		EXEC [Synch].[usp_RefreshActiveDirectoryRefData] @BatchID, @Verbose, @ActiveDirectoryPath, @ActiveDirectoryGroupName, @ResultCode OUTPUT,@ResultMessage OUTPUT
		
		--If the Fetch is successful without any AD Failure
		If @ResultCode = 0
		Begin
			-- Merge User demographic details into Core.Users 
			EXEC [Synch].[usp_MergeADUserSyncUsers] @ResultCode OUTPUT,@ResultMessage OUTPUT
			-- Merge User Address details into Core.Addresses and Core.UserAddress 
			EXEC [Synch].[usp_MergeADUserSyncUserAddress] @ResultCode OUTPUT,@ResultMessage OUTPUT
			-- Merge User Email details into Core.Email and Core.UserEmail
			EXEC [Synch].[usp_MergeADUserSyncUserEmail] @ResultCode OUTPUT,@ResultMessage OUTPUT
			-- Merge User Phone details into Core.Phone and Core.UserPhone
			EXEC [Synch].[usp_MergeADUserSyncUserPhones] @ResultCode OUTPUT,@ResultMessage OUTPUT
			--Update Batch Status on Batch and BatchDetails
			SET @BatchStatusID = 4 --Mark Successful
			EXEC	
				@ReturnValue = [Synch].[usp_UpdateBatch]
				@BatchID = @BatchID,
				@BatchStatusID = @BatchStatusID,
				@BatchTypeID = @BatchTypeID,
				@ConfigID = @ConfigID,
				@USN = @USN,
				@IsActive = @IsActive, 
				@ModifiedOn = @ModifiedOn,
				@ModifiedBy = @ModifiedBy,
				@ResultCode = @ResultCode OUTPUT,
				@ResultMessage = @ResultMessage OUTPUT,
				@ID = @ID OUTPUT
			EXEC 
				@ReturnValue = [Synch].[usp_AddBatchDetails]
				@BatchID = @BatchID,
				@BatchStatusID = @BatchStatusID,
				@PackageName = @PackageName, --PackageName is ADSynchService
				@PackageTask = @PackageName, --PackageTask is ADSynch
				@ModifiedBy = @ModifiedBy,
				@CreatedBy = @ModifiedBy,
				@ResultCode = @ResultCode OUTPUT,
				@ResultMessage = @ResultMessage OUTPUT,
				@ID = @ID OUTPUT
			update Core.Users set IsActive = 0, EffectiveToDate = GETDATE() 
			where UserName in (
				Select u.UserName from Core.Users u
				left join Synch.ADUserStage us on us.UserName = u.UserName
				where u.ADFlag = 1 and (us.UserName is null or (us.IsActive = 0)) )
			Declare @ValidationErrors int
			Declare @DuplicationErrors int
			--Get Validations Error Count on Data Imported
			SELECT
				@ValidationErrors=Count(ErrorMessage)
			FROM Synch.ActiveDirectoryRefData
			WHERE ErrorMessage IS NOT NULL OR LEN(ErrorMessage) > 0
			--Get Validations Error Count on Duplicate User
			SELECT
				@DuplicationErrors=Count(ErrorMessage)
			FROM Synch.ADUserStage
			WHERE ErrorMessage IS NOT NULL OR LEN(ErrorMessage) > 0
			If(@DuplicationErrors > 0 or @ValidationErrors > 0)
			Begin
				Set @ResultCode = -1
				Set @ResultMessage = 'There were Issues in AD User Import and Synch. Please Take A Look'
				Print CHAR(10) + CHAR(10)			
				Print 'ErrorCode: ' + CAST(@ResultCode as varchar)
				Print @ResultMessage
				Print CHAR(10) + CHAR(10)
				If (@ValidationErrors > 0)
				Begin
					Set @ResultMessage = ''
					SELECT
						@ResultMessage = COALESCE(@ResultMessage, '') + samaccountname + ': '+ ErrorMessage + CHAR(10) 
					FROM
						Synch.ActiveDirectoryRefData
					WHERE
						ErrorMessage IS NOT NULL OR LEN(ErrorMessage) > 0
					Set @ResultMessage = @ResultMessage + CAST(@ValidationErrors as nvarchar) + ' failed due to validation issues. ' + CHAR(10) 
					Print @ResultMessage + CHAR(10)
				End
				If (@DuplicationErrors > 0)
				Begin
					Set @ResultMessage = ''
					SELECT
						@ResultMessage = COALESCE(@ResultMessage, '') + UserName + ': '+ ErrorMessage + CHAR(10) 
					FROM
						Synch.ADUserStage
					WHERE
						ErrorMessage IS NOT NULL OR LEN(ErrorMessage) > 0
					Set @ResultMessage = @ResultMessage + CAST(@DuplicationErrors as nvarchar) + ' failed due to being duplicate.' + CHAR(10) 
					Print @ResultMessage + CHAR(10)
				End
				Set @ResultMessage = 'There were Issues in AD User Import and Synch. Please Take A Look'
				Select 
					ResultCode = @ResultCode,
					DuplicationErrors = @DuplicationErrors, 
					ValidationErrors = @ValidationErrors,
					ResultMessage = @ResultMessage
			End
		End
	END TRY
	BEGIN CATCH
		--Update Batch Status on Batch and BatchDetails
		SET @BatchStatusID = 5 --Mark Failed
		EXEC	
			@ReturnValue = [Synch].[usp_UpdateBatch]
			@BatchID = @BatchID,
			@BatchStatusID = @BatchStatusID,
			@BatchTypeID = @BatchTypeID,
			@ConfigID = @ConfigID,
			@USN = @USN,
			@IsActive = @IsActive, 
			@ModifiedOn = @ModifiedOn,
			@ModifiedBy = @ModifiedBy,
			@ResultCode = @ResultCode OUTPUT,
			@ResultMessage = @ResultMessage OUTPUT,
			@ID = @ID OUTPUT

		EXEC 
			@ReturnValue = [Synch].[usp_UpdateBatchDetails]
			@BatchID = @BatchID,
			@BatchStatusID = @BatchStatusID,
			@PackageName = @PackageName, --PackageName is ADSynchService
			@PackageTask = @PackageName, --PackageTask is ADSynch
			@ModifiedBy = @ModifiedBy,
			@CreatedBy = @ModifiedBy,
			@ResultCode = @ResultCode OUTPUT,
			@ResultMessage = @ResultMessage OUTPUT,
			@ID = @ID OUTPUT

		SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO