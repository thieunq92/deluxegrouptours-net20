/*
 * DDL Changes
 */
ALTER TABLE bitportal_site
	 ADD COLUMN metadescription MEDIUMTEXT;

ALTER TABLE bitportal_site
	ADD COLUMN metakeywords MEDIUMTEXT;

ALTER TABLE bitportal_node
	 ADD COLUMN metadescription MEDIUMTEXT;

ALTER TABLE bitportal_node
	ADD COLUMN metakeywords MEDIUMTEXT;


CREATE TABLE bitportal_moduleservice(
moduleserviceid INT NOT NULL AUTO_INCREMENT,
moduletypeid INT NOT NULL,
servicekey VARCHAR(50) NOT NULL,
servicetype VARCHAR(255) NOT NULL,
classtype VARCHAR(255) NOT NULL,
lifestyle VARCHAR(10),
FOREIGN KEY (moduletypeid) REFERENCES bitportal_moduletype (moduletypeid),
PRIMARY KEY (moduleserviceid),
UNIQUE IX_moduleservice_moduletypeid_servicekey (moduletypeid,servicekey));


/*
 * Version
 */
UPDATE bitportal_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'CMS.Core';
