USE wtt;

ALTER TABLE users
    ADD username VARCHAR(32);

CREATE UNIQUE INDEX USERS_USERNAME ON users (username);