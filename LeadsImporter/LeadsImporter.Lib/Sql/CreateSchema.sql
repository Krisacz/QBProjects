/*==============================================================================================*/
/*==============================================================================================*/
/*==============================================================================================*/
/*MAKE SURE TO CREATE "LeadsImporterDB" before running that script                              */
/*==============================================================================================*/
/*==============================================================================================*/
/*==============================================================================================*/
USE [LeadsImporterDB]
GO
/****** Object:  Table [dbo].[Data]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Data](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[LeadId] [varchar](50) NOT NULL,
	[CustomerId] [varchar](50) NOT NULL,
	[LenderId] [varchar](50) NULL,
	[LoanDate] [date] NULL,
	[LeadCreated] [date] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Exceptions]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Exceptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[LeadId] [varchar](50) NOT NULL,
	[CustomerId] [varchar](50) NOT NULL,
	[LenderId] [varchar](50) NULL,
	[LoanDate] [date] NULL,
	[LeadCreated] [date] NULL,
	[ExceptionType] [varchar](50) NOT NULL,
	[ExceptionDescription] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DuplicatesCheck]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		KS
-- Create date: 25-05-2015
-- Description:	Duplicates check
-- =============================================
CREATE PROCEDURE [dbo].[DuplicatesCheck] 
	@CustomerId as varchar(50),
	@LenderId as varchar(50),
	@LoanDate as date,
	@Result as int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Result = (SELECT TOP 1 Id FROM [dbo].[Data] D WHERE D.CustomerId = @CustomerId AND D.LenderId = @LenderId AND D.LoanDate = @LoanDate) 
END

GO
/****** Object:  StoredProcedure [dbo].[GetAllData]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		KS
-- Create date: 06-06-2016
-- Description:	Get All Data list
-- =============================================
CREATE PROCEDURE [dbo].[GetAllData] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Data]
END

GO
/****** Object:  StoredProcedure [dbo].[GetAllExceptions]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		KS
-- Create date: 06-06-2016
-- Description:	Get All Exceptions list
-- =============================================
CREATE PROCEDURE [dbo].[GetAllExceptions] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Exceptions]
END

GO
/****** Object:  StoredProcedure [dbo].[InsertException]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		KS
-- Create date: 25-05-2015
-- Description:	Insert new exception record
-- =============================================
CREATE PROCEDURE [dbo].[InsertException] 
	@Type as varchar(50),
	@LeadId as varchar(50),
	@CustomerId as varchar(50),
	@LenderId as varchar(50),
	@LoanDate as date,
	@LeadCreated as date,
	@ExceptionType as varchar(50),
	@ExceptionDescription as varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Exceptions] ([DateTime], [Type], [LeadId], [CustomerId], [LenderId], [LoanDate], [LeadCreated], [ExceptionType], [ExceptionDescription])
	VALUES (GETDATE(), @Type, @LeadId, @CustomerId, @LenderId, @LoanDate, @LeadCreated, @ExceptionType, @ExceptionDescription); 
END



GO
/****** Object:  StoredProcedure [dbo].[InsertRecord]    Script Date: 15/06/2016 11:08:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		KS
-- Create date: 25-05-2015
-- Description:	Insert new data record
-- =============================================
CREATE PROCEDURE [dbo].[InsertRecord] 
	@Type as varchar(50),
	@LeadId as varchar(50),
	@CustomerId as varchar(50),
	@LenderId as varchar(50),
	@LoanDate as date,
	@LeadCreated as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Data] ([DateTime], [Type], [LeadId], [CustomerId], [LenderId], [LoanDate], [LeadCreated])
	VALUES (GETDATE(), @Type, @LeadId, @CustomerId, @LenderId, @LoanDate, @LeadCreated); 
END


GO
