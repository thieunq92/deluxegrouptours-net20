-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('bitportal_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM bitportal_modulesetting));

/*
 *  Article sort order settings
 */
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SORT_BY', 'Sort by', 'CMS.Modules.Articles.SortBy', true, true 
	FROM bitportal_moduletype 
	WHERE name = 'Articles';

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SORT_DIRECTION', 'Sort direction', 'CMS.Modules.Articles.SortDirection', true, true 
	FROM bitportal_moduletype 
	WHERE name = 'Articles';

INSERT INTO bitportal_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SORT_BY', 'DateOnline'
	FROM bitportal_section s
		INNER JOIN bitportal_moduletype m ON m.moduletypeid = s.moduletypeid
	WHERE m.name = 'Articles';

INSERT INTO bitportal_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SORT_DIRECTION', 'DESC'
	FROM bitportal_section s
		INNER JOIN bitportal_moduletype m ON m.moduletypeid = s.moduletypeid
	WHERE m.name = 'Articles';
	
/*
 *  Login control (user) settings
 */
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_REGISTER', 'Show register link', 'System.Boolean', false, false 
	FROM bitportal_moduletype 
	WHERE name = 'User';

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_RESET_PASSWORD', 'Show reset password link', 'System.Boolean', false, false 
	FROM bitportal_moduletype 
	WHERE name = 'User';

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_EDIT_PROFILE', 'Show edit profile link', 'System.Boolean', false, false 
	FROM bitportal_moduletype 
	WHERE name = 'User';

/*
 *  Version
 */
UPDATE bitportal_version SET major = 0, minor = 9, patch = 0 WHERE assembly = 'CMS.Modules';
