CREATE TABLE off_time
(
    id          INT,
    user_id     INT,
    status      CHAR,
    from        DATETIME,
    to          DATETIME,
    description VARCHAR,
    created_at  DATETIME,
    PRIMARY KEY (id),
)