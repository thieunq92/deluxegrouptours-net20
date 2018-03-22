/*
 *  DDL Changes
 */
ALTER TABLE bitportal_moduletype
	ADD COLUMN autoactivate bool DEFAULT true;

UPDATE bitportal_moduletype
	SET autoactivate = true;

ALTER TABLE bitportal_moduletype
	ALTER COLUMN autoactivate SET NOT NULL;

		
/*
 *  Version
 */
UPDATE bitportal_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'CMS.Core';
