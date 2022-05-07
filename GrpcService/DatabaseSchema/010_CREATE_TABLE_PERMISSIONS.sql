USE
wtt;

/* It is recommended to log every permission. Maybe we want trace that who granted the permission to current user*/
CREATE TABLE permissions
(
    id         INT IDENTITY (1,1) PRIMARY KEY,
    user_id    INT, /* The id of the user that has the permission*/
    title      VARCHAR(32),
    granted_by INT, /* The if of the user that granted the permission */
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
);

ALTER TABLE permissions
    ADD CONSTRAINT FK_PERMISSIONS_USERS_USER_ID_ID FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE permissions
    ADD CONSTRAINT FK_PERMISSIONS_USERS_GRANTED_BY_ID FOREIGN KEY (granted_by) REFERENCES users (id) ON DELETE NO ACTION;