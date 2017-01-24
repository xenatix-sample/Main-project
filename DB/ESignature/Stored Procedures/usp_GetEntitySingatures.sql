-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		Update EntitySignatures
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	Sumana Sangapu  Initial creation.
-- 03/30/2016	Karl Jablonski  Added EntitySignatureID field to result set
-- 04/19/2016	Karl Jablonski	Adding SignatureTypeID
-- 09/03/2016	Rahul Vats		Corrected Typos and added note for the fact that the stored proc should be renamed.	
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_GetEntitySingatures]
	@EntityID int,
	@EntityTypeID int,
	@SignatureTypeID int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT 
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

  SELECT
		@ResultCode = 0,
		@ResultMessage = 'Executed Successfully'

  BEGIN TRY
	IF @SignatureTypeID IS NULL
	BEGIN
		SELECT  EntityID, s.SignatureID, EntityTypeID, SignatureTypeID, SignatureBLOB, EntitySignatureID, CredentialID
		FROM	[ESignature].[EntitySignatures] es
		LEFT JOIN [ESignature].[Signatures] s
		ON		es.SignatureID = s.SignatureID
		WHERE	es.EntityID = @EntityID
		AND		es.EntityTypeID = @EntityTypeID
		AND		es.IsActive = 1 
	END
	ELSE
	BEGIN
		SELECT  EntityID, s.SignatureID, EntityTypeID, SignatureTypeID, SignatureBLOB, EntitySignatureID, CredentialID
		FROM	[ESignature].[EntitySignatures] es
		LEFT JOIN [ESignature].[Signatures] s
		ON		es.SignatureID = s.SignatureID
		WHERE	es.EntityID = @EntityID
		AND		es.EntityTypeID = @EntityTypeID
		AND		es.SignatureTypeID = @SignatureTypeID
		AND		es.IsActive = 1 
	END
		
  END TRY

  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END
