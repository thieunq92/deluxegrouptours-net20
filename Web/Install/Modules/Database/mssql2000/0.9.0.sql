
/*
 *  Sort order of articles
 */
DECLARE @moduletypeid int

SELECT @moduletypeid = moduletypeid FROM bitportal_moduletype WHERE [name] = 'Articles'

INSERT INTO bitportal_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SORT_BY', 'Sort by', 'CMS.Modules.Articles.SortBy', 1, 1)

INSERT INTO bitportal_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SORT_DIRECTION', 'Sort direction', 'CMS.Modules.Articles.SortDirection', 1, 1)
GO

INSERT INTO bitportal_sectionsetting (sectionid, [name], value)
	SELECT sectionid, 'SORT_BY', 'DateOnline'
	FROM bitportal_section s
		INNER JOIN bitportal_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.name = 'Articles'

GO

INSERT INTO bitportal_sectionsetting (sectionid, [name], value)
	SELECT sectionid, 'SORT_DIRECTION', 'DESC'
	FROM bitportal_section s
		INNER JOIN bitportal_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.name = 'Articles'

GO

/*
 *  Login control (user) settings
 */
DECLARE @moduletypeid int

SELECT @moduletypeid = moduletypeid FROM bitportal_moduletype WHERE [name] = 'User'

INSERT INTO bitportal_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_REGISTER', 'Show register link', 'System.Boolean', 0, 0)

INSERT INTO bitportal_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_RESET_PASSWORD', 'Show reset password link', 'System.Boolean', 0, 0)

INSERT INTO bitportal_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_EDIT_PROFILE', 'Show edit profile link', 'System.Boolean', 0, 0)
GO

/*
 *  Version
 */
UPDATE bitportal_version SET major = 0, minor = 9, patch = 0 WHERE assembly = 'CMS.Modules'
go