-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetDocumentEntitySignatures]
-- Author:		Demetrios C. Christopher
-- Date:		11/02/2015
--
-- Purpose:		Get the list of signatures for a specific document (by type and ID)
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/02/2015	Demetrios C. Christopher - Initial Creation
-- 03/22/2016	Sumana SAngapu	Replaced DocumentID to ResponseDetailID
-- 04/16/2016	Rajiv Ranjan	Added SignatureBLOB
-- 05/23/2016	Gurmant Singh	Get the EntityName value for the Entity signatures
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_GetDocumentEntitySignatures]
	@DocumentID BIGINT,
	@DocumentTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT
			des.[DocumentEntitySignatureID], des.[DocumentID], des.[DocumentTypeID], des.[EntitySignatureID],
			des.[IsActive], des.[ModifiedBy], des.[ModifiedOn], es.[EntityID], es.[EntityName], es.[EntityTypeID], 
			es.[SignatureID], s.SignatureBLOB,
			es.CredentialID
		FROM [ESignature].[DocumentEntitySignatures] des
			INNER JOIN [ESignature].[EntitySignatures] es
				ON des.DocumentID = @DocumentID AND des.DocumentTypeID = @DocumentTypeID
			LEFT OUTER JOIN [ESignature].[Signatures] s
				ON s.SignatureID = es.SignatureID
		WHERE
			es.EntitySignatureID = des.EntitySignatureID AND des.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END