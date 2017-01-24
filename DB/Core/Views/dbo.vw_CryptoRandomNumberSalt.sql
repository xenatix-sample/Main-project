-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	vw_CryptoRandomNumberSalt
-- Author:		demetrios.christopher@xenatix.com
-- Date:		07/23/2015
--
-- Purpose:		Generates a 32-bit salt for use in hash algorithms
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- Used by:		dbo.fn_GenerateHash
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	demetrios.christopher@xenatix.com		TFS# 797 - Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW [Core].[vw_CryptoRandomNumberSalt]
AS
	SELECT CRYPT_GEN_RANDOM(4) As Salt
GO
