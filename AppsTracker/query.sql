﻿CREATE TABLE IF NOT EXISTS users (
	id SERIAL PRIMARY KEY,
	email CHARACTER VARYING(128) NOT NULL,
	password CHARACTER VARYING(70) NOT NULL
	);
CREATE TABLE IF NOT EXISTS apps (
	id SERIAL PRIMARY KEY,
	name CHARACTER VARYING(100) NOT NULL,
	owner_id INTEGER NOT NULL,
	date DATE NOT NULL,
	CONSTRAINT fk_apps_owner FOREIGN KEY(owner_id) REFERENCES users(id) ON DELETE CASCADE
	);
CREATE TABLE IF NOT EXISTS events (
	id SERIAL PRIMARY KEY,
	name CHARACTER VARYING(100) NOT NULL,
	description TEXT,
	app_id INTEGER NOT NULL,
	date TIMESTAMP NOT NULL,
	CONSTRAINT fk_events_app FOREIGN KEY(app_id) REFERENCES apps(id) ON DELETE CASCADE
	);

CREATE OR REPLACE FUNCTION u_event_new (
	app_id INTEGER,
	event_name CHARACTER VARYING(100),
	event_description TEXT,
	event_date TIMESTAMP)
RETURNS BOOLEAN AS $$
BEGIN
	IF EXISTS (SELECT FROM apps WHERE id = app_id) THEN
		INSERT INTO events (name, description, app_id, date) VALUES ($2, $3, $1, $4);
		RETURN TRUE;
	ELSE
		RETURN FALSE;
	END IF;
END;
$$ LANGUAGE plpgsql;