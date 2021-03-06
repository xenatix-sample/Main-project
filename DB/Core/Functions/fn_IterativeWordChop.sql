-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[fn_IterativeWordChop]
-- Author:		Sumana Sangapu
-- Date:		09/30/2015
--
-- Purpose:		This Table-valued function takes any text as a parameter and splits it into its constituent words, passing back the order in which they occured and their location in the text.  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	Sumana Sangapu TFS# 1511 - Initial creation.
-- 07/06/2016	Kyle Campbell	TFS #12193	Modified to accept / as part of the string (without splitting it up) for searching dates
-----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION [Core].[fn_IterativeWordChop] 
(  
	@string VARCHAR(MAX)
)  

RETURNS
@Results TABLE
(
	Item		VARCHAR(255),
	Location	INT,
	SortOrder	INT IDENTITY (1,1) PRIMARY KEY
)

AS

BEGIN

		DECLARE @Len INT, @Start INT, @end INT, @Cursor INT,@length INT

		SELECT @Cursor=1, @len=LEN(@string)

		WHILE @cursor<=@len

			BEGIN

				SELECT @start=PATINDEX('%[^A-Za-z0-9][A-Za-z0-9%]%',' '+SUBSTRING (@string,@cursor,50))-1

				if @start<0 break                

				SELECT @length=PATINDEX('%[^A-Z''a-z0-9-/%]%',SUBSTRING (@string,@cursor+@start+1,50)+' ')   

				INSERT INTO @results(Item, Location) 

				SELECT  SUBSTRING(@string,@cursor+@start,@length), @cursor+@start

				SELECT @Cursor=@Cursor+@Start+@length+1

			END

		RETURN

END

