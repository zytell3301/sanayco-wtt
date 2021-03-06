USE
wtt;

CREATE TABLE missions
(
    id          INT IDENTITY (1,1) PRIMARY KEY,
    member_id   INT           NOT NULL,
    project_id  INT           NOT NULL,
    from_date   DATETIME      NOT NULL,
    to_date     DATETIME      NOT NULL,
    description VARCHAR(1024) NOT NULL,
);

ALTER TABLE missions
    ADD CONSTRAINT FK_MISSIONS_MEMBER_ID_ID FOREIGN KEY (member_id) REFERENCES users (id) ON UPDATE CASCADE ON DELETE CASCADE

ALTER TABLE missions
    ADD CONSTRAINT FK_MISSIONS_PROJECT_ID_ID FOREIGN KEY (project_id) REFERENCES projects (id) ON DELETE CASCADE ON UPDATE CASCADE;