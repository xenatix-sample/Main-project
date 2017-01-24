CREATE FUNCTION [Core].[fn_GenerateHash] (@Password nvarchar(128))
RETURNS nvarchar(128)
AS
BEGIN

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	fn_GenerateHash
-- Author:		demetrios.christopher@xenatix.com
-- Date:		07/23/2015
--
-- Purpose:		Generated a cryptographically-strong hash of the supplied password for safe storage in the database
--
-- Notes:		The very simple hash approach below is equivalent to the way passwords are stored in SQL Server 2012.
--				The algorithm used (SHA-512, aka SHA-2) is inferior only to the latest specification (SHA-3) which was
--				introduced in the last few years and not yet available natively in SQL Server.
--				Although the early versions of SHA algorithms are increasingly under attack and their cryptographic
--				strength is in jeopardy, SHA-512 is still considered unbreakable.
--
-- Depends:		dbo.vw_CryptoRandomNumberSalt
--
-- Used by:		usp_AuthenticateUser
--				usp_SetLoginData
--				usp_UpdateUserPassword
--				usp_AddUser
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Demetrios Christopher:		TFS# 797 - Initial creation; input and output size tentatively set to nv(128)
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/29/1016	RAV: - Reviewed the code as was causing error while publishing: Msg 4121, Level 16, State 1, Line 635: Cannot find either column "Core" or the user-defined function or aggregate "Core.fn_GenerateHash", or the name is ambiguous.

-------------------------------------------------e----------------------------------------------------------------------

	-- Generate a random 32-bit salt
	DECLARE @Salt VARBINARY(4) = (SELECT Salt FROM Core.vw_CryptoRandomNumberSalt)

	-- Compute the hashh
	DECLARE @Hash VARBINARY(MAX) = 0x0200 + @Salt + HASHBYTES('SHA2_512', CAST(@Password AS VARBINARY(MAX)) + @Salt); 

	RETURN CONVERT(nvarchar(128), @Hash)
END
GO
