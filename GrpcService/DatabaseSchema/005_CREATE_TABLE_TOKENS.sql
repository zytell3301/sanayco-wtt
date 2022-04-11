CREATE TABLE tokens
(
    id              INT,
    user_id         INT,
    token           CHAR,
    expiration_date DATETIME,
    created_at      DATETIME,
    PRIMARY KEY (id),
)