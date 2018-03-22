
CREATE TABLE bitportal_user(
userid int identity(1,1) NOT NULL CONSTRAINT PK_user PRIMARY KEY,
username nvarchar(50) NOT NULL,
password nvarchar(100) NOT NULL,
firstname nvarchar(100) NULL,
lastname nvarchar(100) NULL,
email nvarchar(100) NOT NULL,
website nvarchar(100) NULL,
timezone int DEFAULT 0 NOT NULL,
isactive bit NULL,
lastlogin datetime NULL,
lastip nvarchar(40) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_user_username UNIQUE(username))
go


CREATE TABLE bitportal_role(
roleid int identity(1,1) NOT NULL CONSTRAINT PK_role PRIMARY KEY,
name nvarchar(50) NOT NULL,
permissionlevel int DEFAULT 1 NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_role_name UNIQUE(name))
go


CREATE TABLE bitportal_userrole(
userroleid int identity(1,1) NOT NULL CONSTRAINT PK_userrole PRIMARY KEY,
userid int NOT NULL,
roleid int NOT NULL)
go


CREATE TABLE bitportal_template(
templateid int identity(1,1) NOT NULL CONSTRAINT PK_template PRIMARY KEY,
name nvarchar(100) NOT NULL,
basepath nvarchar(100) NOT NULL,
templatecontrol nvarchar(50) NOT NULL,
css nvarchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_moduletype(
moduletypeid int identity(1,1) NOT NULL CONSTRAINT PK_moduletype PRIMARY KEY,
name nvarchar(100) NOT NULL,
friendlyname nvarchar(200) NULL,
assemblyname nvarchar(100) NULL,
classname nvarchar(255) NOT NULL,
path nvarchar(255) NOT NULL,
editpath nvarchar(255) NULL,
autoactivate bit NOT NULL DEFAULT 1,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_moduletype_classname UNIQUE(classname))
go


CREATE TABLE bitportal_modulesetting(
modulesettingid int identity(1,1) NOT NULL CONSTRAINT PK_modulesetting PRIMARY KEY,
moduletypeid int NOT NULL,
name nvarchar(50) NOT NULL,
friendlyname nvarchar(50) NOT NULL,
settingdatatype nvarchar(100) NOT NULL,
iscustomtype bit NOT NULL,
isrequired bit NOT NULL)
go

CREATE UNIQUE INDEX IX_modulesetting_moduletypeid_name ON bitportal_modulesetting (moduletypeid,name)
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


CREATE TABLE bitportal_site(
siteid int identity(1,1) NOT NULL CONSTRAINT PK_site PRIMARY KEY,
templateid int NULL,
roleid int NOT NULL,
name nvarchar(100) NOT NULL,
homeurl nvarchar(100) NOT NULL,
defaultculture nvarchar(8) NOT NULL,
defaultplaceholder nvarchar(100) NULL,
webmasteremail nvarchar(100) NOT NULL,
usefriendlyurls bit NULL,
metakeywords nvarchar(500) NULL,
metadescription nvarchar(500) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_site_name UNIQUE(name))
go


CREATE TABLE bitportal_node(
nodeid int identity(1,1) NOT NULL CONSTRAINT PK_node PRIMARY KEY,
parentnodeid int NULL,
templateid int NULL,
siteid int NOT NULL,
title nvarchar(255) NOT NULL,
shortdescription nvarchar(255) NOT NULL,
position int DEFAULT 0 NOT NULL,
culture nvarchar(8) NOT NULL,
showinnavigation bit NOT NULL,
linkurl nvarchar(255) NULL,
linktarget int NULL,
metakeywords nvarchar(500) NULL,
metadescription nvarchar(500) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go

CREATE UNIQUE INDEX IX_node_shortdescription_siteid ON bitportal_node (shortdescription,siteid)
go

CREATE TABLE bitportal_menu(
menuid int identity(1,1) NOT NULL CONSTRAINT PK_menu PRIMARY KEY,
rootnodeid int NOT NULL,
name nvarchar(50) NOT NULL,
placeholder nvarchar(50) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_menunode(
menunodeid int identity(1,1) NOT NULL CONSTRAINT PK_menunode PRIMARY KEY,
menuid int NOT NULL,
nodeid int NOT NULL,
position int NOT NULL)
go


CREATE TABLE bitportal_sitealias(
sitealiasid int identity(1,1) NOT NULL CONSTRAINT PK_sitealias PRIMARY KEY,
siteid int NOT NULL,
nodeid int NULL,
url nvarchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_section(
sectionid int identity(1,1) NOT NULL CONSTRAINT PK_section PRIMARY KEY,
nodeid int NULL,
moduletypeid int NOT NULL,
title nvarchar(100) NOT NULL,
showtitle bit DEFAULT 1 NOT NULL,
placeholder nvarchar(100) NULL,
position int DEFAULT 0 NOT NULL,
cacheduration int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_sectionsetting(
sectionsettingid int identity(1,1) NOT NULL CONSTRAINT PK_sectionsetting PRIMARY KEY,
sectionid int NOT NULL,
name nvarchar(50) NOT NULL,
value nvarchar(100) NULL)
go

CREATE UNIQUE INDEX IX_sectionsetting_sectionid_name ON bitportal_sectionsetting (sectionid,name)
go

CREATE TABLE bitportal_sectionconnection(
sectionconnectionid int identity(1,1) NOT NULL CONSTRAINT PK_sectionconnection PRIMARY KEY,
sectionidfrom int NOT NULL,
sectionidto int NOT NULL,
actionname nvarchar(50) NOT NULL)
go

CREATE UNIQUE INDEX IX_sectionconnection_sectionidfrom_actionname ON bitportal_sectionconnection (sectionidfrom, actionname)
go

CREATE TABLE bitportal_templatesection(
templatesectionid int identity(1,1) NOT NULL CONSTRAINT PK_templatesection PRIMARY KEY,
templateid int NOT NULL,
sectionid int NOT NULL,
placeholder nvarchar(100) NOT NULL)
go

CREATE UNIQUE INDEX IX_templatesection_templateidid_placeholder ON bitportal_templatesection (templateid, placeholder)
go

CREATE TABLE bitportal_noderole(
noderoleid int identity(1,1) NOT NULL CONSTRAINT PK_noderole PRIMARY KEY,
nodeid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IX_noderole_nodeid_roleid ON bitportal_noderole (nodeid,roleid)
go

CREATE TABLE bitportal_sectionrole(
sectionroleid int identity(1,1) NOT NULL CONSTRAINT PK_sectionrole PRIMARY KEY,
sectionid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL,
[adminallowed] bit NOT NULL,
[insertallowed] bit NOT NULL,
[deleteallowed] bit NULL)
go

CREATE UNIQUE INDEX IX_sectionrole_roleid_sectionid ON bitportal_sectionrole (roleid,sectionid)
go


CREATE TABLE bitportal_version(
versionid int identity(1,1) NOT NULL CONSTRAINT PK_version PRIMARY KEY,
assembly nvarchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL)

go


ALTER TABLE bitportal_userrole
ADD CONSTRAINT FK_userrole_role_roleid
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go

ALTER TABLE bitportal_userrole
ADD CONSTRAINT FK_user_userid
FOREIGN KEY (userid) REFERENCES bitportal_user (userid)
go


ALTER TABLE bitportal_modulesetting
ADD CONSTRAINT FK_modulesetting_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid)
go


ALTER TABLE bitportal_moduleservice
ADD CONSTRAINT FK_moduleservice_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid)
go


ALTER TABLE bitportal_site
ADD CONSTRAINT FK_site_role_roleid
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go

ALTER TABLE bitportal_site
ADD CONSTRAINT FK_site_template_templateid
FOREIGN KEY (templateid) REFERENCES bitportal_template (templateid)
go


ALTER TABLE bitportal_node
ADD CONSTRAINT FK_node_node_parentnodeid 
FOREIGN KEY (parentnodeid) REFERENCES bitportal_node (nodeid)
go

ALTER TABLE bitportal_node
ADD CONSTRAINT FK_node_site_siteid
FOREIGN KEY (siteid) REFERENCES bitportal_site (siteid)
go

ALTER TABLE bitportal_node
ADD CONSTRAINT FK_node_template_templateid
FOREIGN KEY (templateid) REFERENCES bitportal_template (templateid)
go


ALTER TABLE bitportal_menu
ADD CONSTRAINT FK_menu_node_rootnodeid
FOREIGN KEY (rootnodeid) REFERENCES bitportal_node (nodeid)
go


ALTER TABLE bitportal_menunode
ADD CONSTRAINT FK_menunode_menu_menuid
FOREIGN KEY (menuid) REFERENCES bitportal_menu (menuid)
go

ALTER TABLE bitportal_menunode
ADD CONSTRAINT FK_menunode_node_nodeid
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go


ALTER TABLE bitportal_sitealias
ADD CONSTRAINT FK_sitealias_node_nodeid
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go

ALTER TABLE bitportal_sitealias
ADD CONSTRAINT FK_sitealias_site_siteid
FOREIGN KEY (siteid) REFERENCES bitportal_site (siteid)
go


ALTER TABLE bitportal_section
ADD CONSTRAINT FK_section_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid)
go

ALTER TABLE bitportal_section
ADD CONSTRAINT FK_section_node_nodeid
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go


ALTER TABLE bitportal_sectionsetting
ADD CONSTRAINT FK_sectionsetting_section_sectionid
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go

ALTER TABLE bitportal_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidfrom
FOREIGN KEY (sectionidfrom) REFERENCES bitportal_section (sectionid)
go

ALTER TABLE bitportal_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidto
FOREIGN KEY (sectionidto) REFERENCES bitportal_section (sectionid)
go

ALTER TABLE bitportal_templatesection
ADD CONSTRAINT FK_templatesection_template_templateid
FOREIGN KEY (templateid) REFERENCES bitportal_template (templateid)
go

ALTER TABLE bitportal_templatesection
ADD CONSTRAINT FK_templatesection_section_sectionid
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go

ALTER TABLE bitportal_noderole
ADD CONSTRAINT FK_noderole_node_nodeid
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go

ALTER TABLE bitportal_noderole
ADD CONSTRAINT FK_noderole_role_roleid
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go


ALTER TABLE bitportal_sectionrole
ADD CONSTRAINT FK_sectionrole_role_roleid
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go

ALTER TABLE bitportal_sectionrole
ADD CONSTRAINT FK_sectionrole_section_sectionid
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go

-- DATA
SET DATEFORMAT ymd


SET IDENTITY_INSERT bitportal_role ON

GO

INSERT INTO bitportal_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (3, 'Authenticated user', '2004-01-04 16:34:50.271', '2004-06-25 00:59:02.822', 2)
INSERT INTO bitportal_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (2, 'Editor', '2004-01-04 16:34:25.669', '2004-06-25 00:59:08.256', 6)
INSERT INTO bitportal_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (1, 'Administrator', '2004-01-04 16:33:42.255', '2004-09-19 17:08:47.248', 14)
INSERT INTO bitportal_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (4, 'Anonymous user', '2004-01-04 16:35:10.766', '2004-07-16 21:18:09.017', 1)

GO

SET IDENTITY_INSERT bitportal_role OFF

GO

SET IDENTITY_INSERT bitportal_template ON

GO

INSERT INTO bitportal_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (1, 'Cuyahoga Home', 'Templates/Classic', 'CuyahogaHome.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO bitportal_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (2, 'Cuyahoga Standard', 'Templates/Classic', 'CuyahogaStandard.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO bitportal_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (3, 'Cuyahoga New', 'Templates/Default', 'CuyahogaNew.ascx', 'red-new.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO bitportal_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (4, 'Another Red', 'Templates/AnotherRed', 'CMS.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')

GO

SET IDENTITY_INSERT bitportal_template OFF

GO


INSERT INTO bitportal_version (assembly, major, minor, patch) VALUES ('CMS.Core', 1, 5, 2)
GO
