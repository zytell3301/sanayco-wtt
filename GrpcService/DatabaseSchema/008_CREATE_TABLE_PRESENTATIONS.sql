USE wtt;
CREATE TABLE presentations
(
    id      INT IDENTITY (1,1),
    user_id INT,
    start   DATETIME,
    "end"   DATETIME,
    PRIMARY KEY (id),
)

ALTER TABLE presentations
    ADD CONSTRAINT PRESENTATIONS_USERS_USER_ID_ID FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE;