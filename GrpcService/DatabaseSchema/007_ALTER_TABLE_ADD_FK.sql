/*
 * Relation definitions for tasks table
 */
ALTER TABLE tasks
    ADD CONSTRAINT FK_TASKS_USERS_USER_ID_ID FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE tasks
    ADD CONSTRAINT FK_TASKS_PROJECTS_PROJECT_ID_ID FOREIGN KEY (project_id) REFERENCES projects (id) ON DELETE CASCADE ON UPDATE CASCADE;

/**
  * Relation definitions for project_members table
 */
ALTER TABLE project_members
    ADD CONSTRAINT FK_PROJECT_MEMBERS_PROJECTS_PROJECT_ID_ID FOREIGN KEY (project_id) REFERENCES projects (id) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE project_members
    ADD CONSTRAINT FK_PROJECT_MEMBERS_USERS_USER_ID_ID FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;

/**
  * Relation definition for off_time table
 */
ALTER TABLE off_time
    ADD CONSTRAINT FK_OFF_TIME_USERS_USER_ID_ID FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;

/**
  * Relation definition for tokens table
 */
ALTER TABLE tokens
    ADD CONSTRAINT FK_TOKENS_USERS_USER_ID_ID FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;