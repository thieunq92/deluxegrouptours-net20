
CREATE TABLE bitportal_user(
userid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_user1 PRIMARY KEY,
username varchar(50) NOT NULL,
password varchar(100) NOT NULL,
firstname varchar(100) NULL,
lastname varchar(100) NULL,
email varchar(100) NOT NULL,
website varchar(100) NULL,
timezone int DEFAULT 0 NOT NULL,
isactive bit NULL,
lastlogin datetime NULL,
lastip varchar(40) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_bitportal_user1 UNIQUE(username))
go


CREATE TABLE bitportal_role(
roleid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_role1 PRIMARY KEY,
name varchar(50) NOT NULL,
permissionlevel int DEFAULT 1 NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_bitportal_role1 UNIQUE(name))
go


CREATE TABLE bitportal_userrole(
userroleid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_userrole1 PRIMARY KEY,
userid int NOT NULL,
roleid int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_template(
templateid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_template1 PRIMARY KEY,
name varchar(100) NOT NULL,
basepath varchar(100) NOT NULL,
templatecontrol varchar(50) NOT NULL,
css varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_moduletype(
moduletypeid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_moduletype1 PRIMARY KEY,
name varchar(100) NOT NULL,
assemblyname varchar(100) NULL,
classname varchar(255) NOT NULL,
path varchar(255) NOT NULL,
editpath varchar(255) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_bitportal_moduletype1 UNIQUE(classname))
go


CREATE TABLE bitportal_modulesetting(
modulesettingid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_modulesetting1 PRIMARY KEY,
moduletypeid int NOT NULL,
name varchar(50) NOT NULL,
friendlyname varchar(50) NOT NULL,
settingdatatype varchar(100) NOT NULL,
iscustomtype bit NOT NULL,
isrequired bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_bitportal_modulesetting_1 ON bitportal_modulesetting (moduletypeid,name)
go

CREATE TABLE bitportal_site(
siteid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_site1 PRIMARY KEY,
templateid int NULL,
roleid int NOT NULL,
name varchar(100) NOT NULL,
homeurl varchar(100) NOT NULL,
defaultculture varchar(8) NOT NULL,
defaultplaceholder varchar(100) NULL,
webmasteremail varchar(100) NOT NULL,
usefriendlyurls bit NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_bitportal_site1 UNIQUE(name))
go


CREATE TABLE bitportal_node(
nodeid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_node1 PRIMARY KEY,
parentnodeid int NULL,
templateid int NULL,
siteid int NOT NULL,
title varchar(255) NOT NULL,
shortdescription varchar(255) NOT NULL,
position int DEFAULT 0 NOT NULL,
culture varchar(8) NOT NULL,
showinnavigation bit NOT NULL,
linkurl varchar(255) NULL,
linktarget int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go

CREATE UNIQUE INDEX IDX_bitportal_node_shortdescription_siteid ON bitportal_node (shortdescription,siteid)
go

CREATE TABLE bitportal_menu(
menuid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_menu1 PRIMARY KEY,
rootnodeid int NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_menunode(
menunodeid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_menunode1 PRIMARY KEY,
menuid int NOT NULL,
nodeid int NOT NULL,
position int NOT NULL)
go


CREATE TABLE bitportal_sitealias(
sitealiasid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_sitealias1 PRIMARY KEY,
siteid int NOT NULL,
nodeid int NULL,
url varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_section(
sectionid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_section1 PRIMARY KEY,
nodeid int NULL,
moduletypeid int NOT NULL,
title varchar(100) NOT NULL,
showtitle bit DEFAULT 1 NOT NULL,
placeholder varchar(100) NULL,
position int DEFAULT 0 NOT NULL,
cacheduration int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE bitportal_sectionsetting(
sectionsettingid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_sectionsetting1 PRIMARY KEY,
sectionid int NOT NULL,
name varchar(50) NOT NULL,
value varchar(100) NULL)
go

CREATE UNIQUE INDEX IDX_bitportal_sectionsetting_1 ON bitportal_sectionsetting (sectionid,name)
go

CREATE TABLE bitportal_noderole(
noderoleid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_noderole1 PRIMARY KEY,
nodeid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_bitportal_noderole_1 ON bitportal_noderole (nodeid,roleid)
go

CREATE TABLE bitportal_sectionrole(
sectionroleid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_sectionrole1 PRIMARY KEY,
sectionid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_bitportal_sectionrole_1 ON bitportal_sectionrole (roleid,sectionid)
go

CREATE TABLE bitportal_version(
versionid int identity(1,1) NOT NULL CONSTRAINT PK_bitportal_version PRIMARY KEY,
assembly varchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL)

go



ALTER TABLE bitportal_userrole
ADD CONSTRAINT FK_bitportal_userrole_1 
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go

ALTER TABLE bitportal_userrole
ADD CONSTRAINT FK_bitportal_userrole_2 
FOREIGN KEY (userid) REFERENCES bitportal_user (userid)
go




ALTER TABLE bitportal_modulesetting
ADD CONSTRAINT FK_bitportal_modulesetting_1 
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid)
go


ALTER TABLE bitportal_site
ADD CONSTRAINT FK_bitportal_site_1 
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go

ALTER TABLE bitportal_site
ADD CONSTRAINT FK_bitportal_site_2 
FOREIGN KEY (templateid) REFERENCES bitportal_template (templateid)
go


ALTER TABLE bitportal_node
ADD CONSTRAINT FK_bitportal_node_1 
FOREIGN KEY (parentnodeid) REFERENCES bitportal_node (nodeid)
go

ALTER TABLE bitportal_node
ADD CONSTRAINT FK_bitportal_node_2 
FOREIGN KEY (siteid) REFERENCES bitportal_site (siteid)
go

ALTER TABLE bitportal_node
ADD CONSTRAINT FK_bitportal_node_3 
FOREIGN KEY (templateid) REFERENCES bitportal_template (templateid)
go


ALTER TABLE bitportal_menu
ADD CONSTRAINT FK_bitportal_menu_1 
FOREIGN KEY (rootnodeid) REFERENCES bitportal_node (nodeid)
go


ALTER TABLE bitportal_menunode
ADD CONSTRAINT FK_bitportal_menunode_1 
FOREIGN KEY (menuid) REFERENCES bitportal_menu (menuid)
go

ALTER TABLE bitportal_menunode
ADD CONSTRAINT FK_bitportal_menunode_2 
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go


ALTER TABLE bitportal_sitealias
ADD CONSTRAINT FK_bitportal_sitealias_1 
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go

ALTER TABLE bitportal_sitealias
ADD CONSTRAINT FK_bitportal_sitealias_2 
FOREIGN KEY (siteid) REFERENCES bitportal_site (siteid)
go


ALTER TABLE bitportal_section
ADD CONSTRAINT FK_bitportal_section_1 
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid)
go

ALTER TABLE bitportal_section
ADD CONSTRAINT FK_bitportal_section_2 
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go


ALTER TABLE bitportal_sectionsetting
ADD CONSTRAINT FK_bitportal_sectionsetting_1 
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go


ALTER TABLE bitportal_noderole
ADD CONSTRAINT FK_bitportal_noderole_1 
FOREIGN KEY (nodeid) REFERENCES bitportal_node (nodeid)
go

ALTER TABLE bitportal_noderole
ADD CONSTRAINT FK_bitportal_noderole_2 
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go


ALTER TABLE bitportal_sectionrole
ADD CONSTRAINT FK_bitportal_sectionrole_1 
FOREIGN KEY (roleid) REFERENCES bitportal_role (roleid)
go

ALTER TABLE bitportal_sectionrole
ADD CONSTRAINT FK_bitportal_sectionrole_2 
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid)
go

-- DATA


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


INSERT INTO bitportal_version (assembly, major, minor, patch) VALUES ('CMS.Core', 0, 8, 2)
GO
