USE
wtt;

CREATE TABLE off_time
(
    id          INT IDENTITY (1,1),
    user_id     INT,
    status      VARCHAR(32),
    from_date   DATETIME,
    to_date     DATETIME,
    description VARCHAR(1024),
    created_at  DATETIME,
    PRIMARY KEY (id),
)