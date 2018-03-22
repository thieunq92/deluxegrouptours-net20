/*
 * DDL Changes
 */
ALTER TABLE bitportal_moduletype
	ADD COLUMN autoactivate TINYINT DEFAULT 1;

UPDATE bitportal_moduletype
	SET autoactivate = 1;

ALTER TABLE bitportal_moduletype
	CHANGE COLUMN autoactivate autoactivate TINYINT NOT NULL DEFAULT 1;
 
/*
 *  Version
 */
UPDATE bitportal_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'CMS.Core';
