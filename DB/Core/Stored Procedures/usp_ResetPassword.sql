﻿-----------------------------------------------------------------------------------------------------------------------
-- PROCEDURE:	[USP_RESETPASSWORD]
-- AUTHOR:		RAJIV RANJAN
-- DATE:		08/26/2015
--
-- PURPOSE:		RESET PASSWORD
--
-- NOTES:		N/A (OR ANY ADDITIONAL NOTES)
--
-- DEPENDS:		N/A (OR ANY DEPENDENCIES SUCH AS OTHER PROCS OR FUNCTIONS)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/26/2015	RAJIV RANJAN		INITIAL CREATION.
-- 08/27/2015	RAJIV RANJAN		ADDED REQUESTORIPADDRESS & ALSO APPLIED EXPIRY CHECK
--									DEACTIVATE RESET LINK AFTER PASSWORD HAS BEEN RESET SUCCESSFULLY.
-- 09/23/2015   JUSTIN SPALTI       ADDED THE MODIFIEDON TO THE UPDATE STATEMENT
-- 01/14/2016	SCOTT MARTIN		ADDED MODIFIEDON PARAMETER, ADDED SYSTEMMODIFIEDON FIELD
-- 02/25/2016 - JUSTIN SPALTI       REMOVED THE MODIFIEDBY PARAMETER SINCE IT IS NOT KNOWN AT THIS POINT OF EXECUTION
-- 06/07/2016	SCOTT MARTIN		ADDED CODE FOR THE MINIMUM NUMBER OF UNIQUE PASSWORDS A USER CAN HAVE BEFORE REUSING ONE
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CORE].[usp_ResetPassword]
	@NEWPASSWORD NVARCHAR(128),
	@RESETIDENTIFIER UNIQUEIDENTIFIER,
	@RESETUSERID INT=0,
	@REQUESTORIPADDRESS  NVARCHAR(30),
	@MODIFIEDON DATETIME,
	@RESULTCODE INT OUTPUT,
	@RESULTMESSAGE NVARCHAR(500) OUTPUT
AS
BEGIN

	DECLARE @AUDITDETAILID BIGINT,
			@PROCNAME VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID BIGINT,
			@PASSWORDLOGID BIGINT,
			@MINUNIQUEPASSWORD INT = 12;

	SELECT @RESULTCODE = 0,
		   @RESULTMESSAGE = 'EXECUTED SUCCESSFULLY'

	BEGIN TRY		
		DECLARE @USERID INT;
		IF @RESETUSERID=0 
			BEGIN
				SELECT @USERID = U.USERID 
				FROM  
					CORE.USERS U 
					INNER JOIN CORE.RESETPASSWORD RP ON U.USERID = RP.USERID 
				WHERE
					RP.RESETIDENTIFIER = @RESETIDENTIFIER AND
					RP.REQUESTORIPADDRESS = @REQUESTORIPADDRESS AND
					DATEDIFF(DAY, RP.EXPIRESON, GETUTCDATE()) <= 0 AND
					RP.ISACTIVE = 1
			END
		ELSE
			BEGIN

				SET @USERID=@RESETUSERID
		END
		IF (ISNULL(@USERID, 0) > 0)
			BEGIN
			
				SELECT @MINUNIQUEPASSWORD = SV.VALUE FROM CORE.SETTINGVALUES SV INNER JOIN CORE.SETTINGS S ON SV.SETTINGSID = S.SETTINGSID WHERE S.SETTINGS = 'MINUNIQUEPASSWORD';

				SELECT @PASSWORDLOGID = MAX(PASSWORDLOGID) FROM CORE.PASSWORDLOG WHERE USERID = @USERID AND PWDCOMPARE(@NEWPASSWORD, [PASSWORD]) = 1
			
		IF ((SELECT COUNT(*) FROM CORE.PASSWORDLOG WHERE USERID = @USERID AND PASSWORDLOGID >= @PASSWORDLOGID) <= @MINUNIQUEPASSWORD) AND @PASSWORDLOGID IS NOT NULL
			BEGIN
				
				RAISERROR ('YOU CANNOT REUSE THAT PASSWORD YET. PLEASE CHOOSE A DIFFERENT PASSWORD.', -- MESSAGE TEXT.
				   16, -- SEVERITY.
				   1 -- STATE.
				   );
			END

			
			EXEC CORE.USP_ADDPREAUDITLOG @PROCNAME, 'UPDATE', 'CORE', 'USERS', @USERID, NULL, @MODIFIEDON, @USERID, @RESULTCODE OUTPUT, @RESULTMESSAGE OUTPUT, @AUDITDETAILID OUTPUT;
						
			UPDATE 
				[CORE].USERS 
			SET
				[PASSWORD] = [CORE].[FN_GENERATEHASH](@NEWPASSWORD),
				MODIFIEDBY = @USERID,
				MODIFIEDON = @MODIFIEDON,
				SYSTEMMODIFIEDON = GETUTCDATE()
			WHERE
				USERID = @USERID
		
			EXEC Auditing.usp_AddPostAuditLog 'UPDATE', 'CORE', 'USERS', @AUDITDETAILID, @USERID, NULL, @MODIFIEDON, @USERID, @RESULTCODE OUTPUT, @RESULTMESSAGE OUTPUT, @AUDITDETAILID OUTPUT;
			
			SELECT @ID = (SELECT TOP 1 RESETPASSWORDID FROM CORE.RESETPASSWORD WHERE RESETIDENTIFIER = @RESETIDENTIFIER  );
				
			IF @ID IS NOT NULL
				BEGIN
				EXEC CORE.USP_ADDPREAUDITLOG @PROCNAME, 'UPDATE', 'CORE', 'RESETPASSWORD', @ID, NULL, @MODIFIEDON, @USERID, @RESULTCODE OUTPUT, @RESULTMESSAGE OUTPUT, @AUDITDETAILID OUTPUT;
				
				-- DEACTIVATE RESET LINK AFTER PASSWORD HAS BEEN CHANGED
				UPDATE
					[CORE].RESETPASSWORD
				SET
					ISACTIVE = 0
				WHERE
					RESETIDENTIFIER = @RESETIDENTIFIER

				EXEC Auditing.usp_AddPostAuditLog 'UPDATE', 'CORE', 'RESETPASSWORD', @AUDITDETAILID, @ID, NULL, @MODIFIEDON, @USERID, @RESULTCODE OUTPUT, @RESULTMESSAGE OUTPUT, @AUDITDETAILID OUTPUT;
				END
		END
		ELSE
			BEGIN
				
				RAISERROR ('INVALID RESET LINK OR HAS BEEN EXPIRED.', -- MESSAGE TEXT.
				   16, -- SEVERITY.
				   1 -- STATE.
				   );
			END

	END TRY
	BEGIN CATCH
		SELECT @RESULTCODE = ERROR_SEVERITY(),
			   @RESULTMESSAGE = ERROR_MESSAGE()
	END CATCH
END