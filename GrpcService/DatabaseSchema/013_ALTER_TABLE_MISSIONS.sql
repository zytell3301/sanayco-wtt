USE
wtt;

ALTER TABLE missions
    ADD title VARCHAR(128) NOT NULL;
ALTER TABLE missions
    ADD location VARCHAR(128) NOT NULL;
ALTER TABLE missions
    ADD is_verified BIT NULL;