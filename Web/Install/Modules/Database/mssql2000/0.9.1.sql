/*
 *  Profile module
 */
INSERT INTO bitportal_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('UserProfile', 'CMS.Modules', 'CMS.Modules.User.ProfileModule', 'Modules/User/EditProfile.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324')
go

/*
 *  Version
 */
UPDATE bitportal_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'CMS.Modules'
go