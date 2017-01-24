CREATE TABLE [Synch].[CMHCAxisAttendanceCodeMapping](
	[CMHCAxisAttendanceCodeMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[AttendanceStatusID] SMALLINT NULL,
	[CMCHAttendanceCode] SMALLINT NULL,
	[CMHCDescription] [nvarchar](75) NULL,
 CONSTRAINT [PK_CMHCAxisAttendanceCodeMappingID] PRIMARY KEY CLUSTERED 
(
	[CMHCAxisAttendanceCodeMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Synch].[CMHCAxisAttendanceCodeMapping] WITH CHECK ADD CONSTRAINT [FK_CMHCAxisAttendanceCodeMapping_AttendanceStatusID] FOREIGN KEY ([AttendanceStatusID]) REFERENCES [Reference].[AttendanceStatus] ([AttendanceStatusID])
GO
ALTER TABLE [Synch].[CMHCAxisAttendanceCodeMapping] CHECK CONSTRAINT [FK_CMHCAxisAttendanceCodeMapping_AttendanceStatusID] 
GO