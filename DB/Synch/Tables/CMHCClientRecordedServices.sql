----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[CMHCClientRecordedServices]
-- Author:		Sumana Sangapu
-- Date:		07/19/2016
--
-- Purpose:		Table to hold the Client Recorded services data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/19/2016	Sumana Sangapu		Initial creation
-- 08/22/2016	Sumana Sangapu		Add the Primary key and foregin key constraints
-- 10/14/2016	Sumana Sangapu		Altered the datatype for LocationCode.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[CMHCClientRecordedServices](
	[ClientRecordedServicesID] INT IDENTITY(1,1) NOT NULL,
	[ServiceRecordingID] BIGINT  NULL,
	[UserID] [nvarchar](100)  NULL,
	[ServiceStartDate] [varchar](30)  NULL,
	[ServiceStartTime] [nvarchar](5)  NULL,
	[MRN] [nvarchar](50)  NULL,
	[CMHCCode] [nvarchar](50)  NULL,
	[CMHCDescription] [nvarchar] (50)NULL,
	[CMHCRU] [varchar](100)  NULL,
	[Duration] [varchar](10)  NULL,
	[ClientDuration] [varchar](10)  NULL,
	[LOF] [int] NULL,
	[AttendanceCode] [smallint]  NULL,
	[CMHCRecipientCode] [smallint]  NULL,
	[LocationCode] nvarchar(10)  NULL,
	[PROJ] [int] NULL,
	[NoofAttendees] [int] NULL,
	BatchID BIGINT NOT NULL,
	ErrorMessage varchar(max) NULL,
	CONSTRAINT [PK_ClientRecordedServicesID] PRIMARY KEY CLUSTERED ([ClientRecordedServicesID] ASC)
	)
	GO 

	ALTER TABLE [Synch].[CMHCClientRecordedServices] WITH CHECK ADD CONSTRAINT [FK_CMHCClientRecordedServices_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
	GO 

