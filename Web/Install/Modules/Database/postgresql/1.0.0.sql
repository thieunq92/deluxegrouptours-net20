-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('bitportal_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM bitportal_moduletype));
SELECT setval('bitportal_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM bitportal_modulesetting));

/*
 *  SearchInput module
 */
INSERT INTO bitportal_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('SearchInput', 'CMS.Modules', 'CMS.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');

/*
 *  Search results settings
 */
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', false, true
	FROM bitportal_moduletype 
	WHERE name = 'Search';

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', false, false
	FROM bitportal_moduletype 
	WHERE name = 'Search';

INSERT INTO bitportal_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'RESULTS_PER_PAGE', '10'
	FROM bitportal_section s
		INNER JOIN bitportal_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Search';
/*
 *  Version
 */
UPDATE bitportal_version SET major = 1, minor = 0, patch = 0 WHERE assembly = 'CMS.Modules';
