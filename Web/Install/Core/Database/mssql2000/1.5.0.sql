/*
 * DDL Changes
 */
ALTER TABLE bitportal_site
	 ADD metadescription nvarchar(500) NULL
go

ALTER TABLE bitportal_site
	ADD metakeywords nvarchar(500) NULL
go

ALTER TABLE bitportal_node
	 ADD metadescription nvarchar(500) NULL
go

ALTER TABLE bitportal_node
	ADD metakeywords nvarchar(500) NULL
go
 
 
CREATE TABLE bitportal_moduleservice(
moduleserviceid int identity(1,1) NOT NULL CONSTRAINT PK_moduleservice PRIMARY KEY,
moduletypeid int NOT NULL,
servicekey nvarchar(50) NOT NULL,
servicetype nvarchar(255) NOT NULL,
classtype nvarchar(255) NOT NULL,
lifestyle nvarchar(10) NULL)
go

CREATE UNIQUE INDEX IX_moduleservice_moduletypeid_servicekey ON bitportal_moduleservice (moduletypeid,servicekey)
go

ALTER TABLE bitportal_moduleservice
ADD CONSTRAINT FK_moduleservice_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid)
go



/*
 * Version
 */
UPDATE bitportal_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'CMS.Core'
go