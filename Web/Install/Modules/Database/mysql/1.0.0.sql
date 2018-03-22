/*
 *  DDL changes.
 */
ALTER TABLE cm_statichtml 
	MODIFY COLUMN content MEDIUMTEXT NOT NULL;


ALTER TABLE cm_feeditem
	MODIFY COLUMN content MEDIUMTEXT;


ALTER TABLE cm_article
	MODIFY COLUMN content MEDIUMTEXT NOT NULL;

/*
 *  SearchInput module
 */
INSERT INTO bitportal_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('SearchInput', 'CMS.Modules', 'CMS.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');

/*
 *  Search results settings
 */
SELECT @moduletypeid := moduletypeid FROM bitportal_moduletype WHERE name = 'Search';

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', 0, 1);

INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', 0, 0);

INSERT INTO bitportal_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'RESULTS_PER_PAGE', '10'
	FROM bitportal_section s
		INNER JOIN bitportal_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Search';
	
/*
 *  Version
 */
UPDATE bitportal_version SET major = 1, minor = 0, patch = 0 WHERE assembly = 'CMS.Modules';
