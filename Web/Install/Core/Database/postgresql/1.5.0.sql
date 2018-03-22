/*
 * DDL Changes
 */
ALTER TABLE bitportal_site
	 ADD COLUMN metadescription varchar(500);

ALTER TABLE bitportal_site
	ADD COLUMN metakeywords varchar(500);

ALTER TABLE bitportal_node
	 ADD COLUMN metadescription varchar(500);

ALTER TABLE bitportal_node
	ADD COLUMN metakeywords varchar(500); 
 
CREATE TABLE bitportal_moduleservice(
moduleserviceid serial NOT NULL CONSTRAINT PK_moduleservice PRIMARY KEY,
moduletypeid int4 NOT NULL,
servicekey varchar(50) NOT NULL,
servicetype varchar(255) NOT NULL,
classtype varchar(255) NOT NULL,
lifestyle varchar(10),
CONSTRAINT FK_moduleservice_moduletype_moduletypeid FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid));

CREATE UNIQUE INDEX IX_moduleservice_moduletypeid_servicekey ON bitportal_moduleservice (moduletypeid,servicekey);


/*
 * Version
 */
UPDATE bitportal_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'CMS.Core';
