/*
 * Module Connections
 */
CREATE TABLE bitportal_sectionconnection(
sectionconnectionid int identity(1,1) NOT NULL CONSTRAINT PK_sectionconnection PRIMARY KEY,
sectionidfrom int NOT NULL,
sectionidto int NOT NULL,
actionname nvarchar(50) NOT NULL)
go

CREATE UNIQUE INDEX IX_sectionconnection_sectionidfrom_actionname ON bitportal_sectionconnection (sectionidfrom, actionname)
go

ALTER TABLE bitportal_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidfrom
FOREIGN KEY (sectionidfrom) REFERENCES bitportal_section (sectionid)
go

ALTER TABLE bitportal_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidto
FOREIGN KEY (sectionidto) REFERENCES bitportal_section (sectionid)
go

/*
 * Template sections
 */
CREATE TABLE bitportal_templatesection(
templatesectionid int identity(1,1) NOT NULL CONSTRAINT PK_templatesection PRIMARY KEY,
templateid int NOT NULL,
sectionid int NOT NULL,
placeholder nvarchar(100) NOT NULL)
go

CREATE UNIQUE INDEX IX_templatesection_templateidid_placeholder ON bitportal_templatesection (templateid, placeholder)
go

ALTER TABLE bitportal_templatesection
ADD CONSTRAINT FK_templatesection_template_templateid
FOREIGN KEY (templateid) REFERENCES bitportal_template (templateid)
go

ALTER TABLE bitportal_templatesection
ADD CONSTRAINT FK_templatesection_section_sectionid
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go

/*
 * Version
 */
UPDATE bitportal_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'CMS.Core'
go