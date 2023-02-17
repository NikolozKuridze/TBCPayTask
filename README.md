# TBCPayTask
1. Build Project for Restore Packages
2. Execute SQL SCRIPT FOR CREATE DATABASE WITH TABLES.
-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- db_a94bee_tbcpaytask.dbo.Cities definition

-- Drop table

-- DROP TABLE db_a94bee_tbcpaytask.dbo.Cities;

CREATE TABLE db_a94bee_tbcpaytask.dbo.Cities (
	ID int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Cities PRIMARY KEY (ID)
);


-- db_a94bee_tbcpaytask.dbo.Persons definition

-- Drop table

-- DROP TABLE db_a94bee_tbcpaytask.dbo.Persons;

CREATE TABLE db_a94bee_tbcpaytask.dbo.Persons (
	ID int IDENTITY(1,1) NOT NULL,
	FirstName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LastName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Gender int NOT NULL,
	PrivateNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BirthDate datetime2 NOT NULL,
	CityID int NOT NULL,
	ImagePath nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Persons PRIMARY KEY (ID)
);


-- db_a94bee_tbcpaytask.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE db_a94bee_tbcpaytask.dbo.[__EFMigrationsHistory];

CREATE TABLE db_a94bee_tbcpaytask.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- db_a94bee_tbcpaytask.dbo.PhoneNumbers definition

-- Drop table

-- DROP TABLE db_a94bee_tbcpaytask.dbo.PhoneNumbers;

CREATE TABLE db_a94bee_tbcpaytask.dbo.PhoneNumbers (
	ID int IDENTITY(1,1) NOT NULL,
	PersonID int NOT NULL,
	[Number] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type] int NOT NULL,
	CONSTRAINT PK_PhoneNumbers PRIMARY KEY (ID),
	CONSTRAINT FK_PhoneNumbers_Persons_PersonID FOREIGN KEY (PersonID) REFERENCES db_a94bee_tbcpaytask.dbo.Persons(ID) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_PhoneNumbers_PersonID ON dbo.PhoneNumbers (  PersonID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a94bee_tbcpaytask.dbo.RelatedPersons definition

-- Drop table

-- DROP TABLE db_a94bee_tbcpaytask.dbo.RelatedPersons;

CREATE TABLE db_a94bee_tbcpaytask.dbo.RelatedPersons (
	ID int IDENTITY(1,1) NOT NULL,
	PersonID int NOT NULL,
	RelatedType int NOT NULL,
	RelatedPersonID int NOT NULL,
	CONSTRAINT PK_RelatedPersons PRIMARY KEY (ID),
	CONSTRAINT FK_RelatedPersons_Persons_PersonID FOREIGN KEY (PersonID) REFERENCES db_a94bee_tbcpaytask.dbo.Persons(ID) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_RelatedPersons_PersonID ON dbo.RelatedPersons (  PersonID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;

