USE
wtt;

CREATE TABLE tokens
(
    id              INT IDENTITY (1,1),
    user_id         INT,
    token           VARCHAR(64), /* This value is variable depends on the length of the tokens that are being inserted */
    expiration_date DATETIME,
    created_at      DATETIME,
    PRIMARY KEY (id),
)