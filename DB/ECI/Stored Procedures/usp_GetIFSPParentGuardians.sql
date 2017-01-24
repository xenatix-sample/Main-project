CREATE PROCEDURE [ECI].[usp_GetIFSPParentGuardians]
       @ContactID BIGINT,
       @ResultCode int OUTPUT,
       @ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
		SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

		BEGIN TRY

			SELECT
				ifsp.[IFSPID], 
				ipg.[ContactID],
				c.FirstName + ' ' + c.LastName AS Name
			FROM [ECI].[IFSP] ifsp
			INNER JOIN	[ECI].[IFSPParentGuardian] ipg ON ifsp.[IFSPID] = ipg.[IFSPID]
			INNER JOIN	[Registration].Contact c ON c.ContactID = ipg.ContactID
			WHERE ifsp.ContactID = @ContactID
			AND ifsp.IsActive = 1 AND  ipg.IsActive = 1
			GROUP BY 
				ifsp.[ContactID], ifsp.[IFSPID], ipg.[ContactID], c.FirstName, c.LastName		

       END TRY
       BEGIN CATCH
              SELECT @ResultCode = ERROR_SEVERITY(),
                     @ResultMessage = ERROR_MESSAGE()
       END CATCH
END
GO

