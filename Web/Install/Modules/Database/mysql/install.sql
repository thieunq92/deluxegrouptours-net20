

CREATE TABLE cm_statichtml(
statichtmlid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
createdby INT NOT NULL,
modifiedby INT,
title VARCHAR(255),
content MEDIUMTEXT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES bitportal_section (sectionid),
FOREIGN KEY (createdby) REFERENCES bitportal_user (userid),
FOREIGN KEY (modifiedby) REFERENCES bitportal_user (userid),
PRIMARY KEY (statichtmlid));

-- DATA --

INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (1, 'StaticHtml', 'CMS.Modules', 'CMS.Modules.StaticHtml.StaticHtmlModule', 'Modules/StaticHtml/StaticHtml.ascx', 'Modules/StaticHtml/EditHtml.aspx', 1, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (3, 'User', 'CMS.Modules', 'CMS.Modules.User.UserModule', 'Modules/User/User.ascx', NULL, 1, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (4, 'Search', 'CMS.Modules', 'CMS.Modules.Search.SearchModule', 'Modules/Search/Search.ascx', NULL, 1, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (5, 'LanguageSwitcher', 'CMS.Modules', 'CMS.Modules.LanguageSwitcher.LanguageSwitcherModule', 'Modules/LanguageSwitcher/LanguageSwitcher.ascx', NULL, 1, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (7, 'UserProfile', 'CMS.Modules', 'CMS.Modules.User.ProfileModule', 'Modules/User/EditProfile.ascx', NULL, 1, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');
INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (8, 'SearchInput', 'CMS.Modules', 'CMS.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, 1, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');
INSERT INTO bitportal_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (9, 'Sitemap', 'CMS.Modules', 'CMS.Modules.Sitemap.SitemapModule', 'Modules/Sitemap/SitemapControl.ascx', NULL, 1, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');


INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 'SHOW_REGISTER', 'Show register link', 'System.Boolean', 0, 0);
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 'SHOW_RESET_PASSWORD', 'Show reset password link', 'System.Boolean', 0, 0);
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 'SHOW_EDIT_PROFILE', 'Show edit profile link', 'System.Boolean', 0, 0);
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', 0, 1);
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', 0, 0);
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 'DISPLAY_MODE', 'Display mode', 'CMS.Modules.LanguageSwitcher.DisplayMode', 1, 1);
INSERT INTO bitportal_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 'REDIRECT_TO_USER_LANGUAGE', 'Redirect user to browser language when possible?', 'System.Boolean', 0, 1);

INSERT INTO bitportal_version (assembly, major, minor, patch) VALUES ('CMS.Modules', 1, 5, 2);
