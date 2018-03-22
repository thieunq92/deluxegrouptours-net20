/*
 * DDL Changes
 */
ALTER TABLE bitportal_moduletype
	ADD autoactivate bit NULL DEFAULT 1
go

UPDATE bitportal_moduletype
SET autoactivate = 1
go

ALTER TABLE bitportal_moduletype
	ALTER COLUMN autoactivate bit NOT NULL
go  


/*
 * Version
 */
UPDATE bitportal_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'CMS.Core'
go