﻿CREATE TABLE IF NOT EXISTS users (
	id SERIAL PRIMARY KEY,
	email CHARACTER VARYING(128) NOT NULL,
	password CHARACTER VARYING(70) NOT NULL
	);
CREATE TABLE IF NOT EXISTS apps (
	id SERIAL PRIMARY KEY,
	name CHARACTER VARYING(100) NOT NULL,
	owner_id INTEGER NOT NULL,
	CONSTRAINT fk_apps_owner FOREIGN KEY(owner_id) REFERENCES users(id) ON DELETE CASCADE
	);
CREATE TABLE IF NOT EXISTS events (
	id SERIAL PRIMARY KEY,
	name CHARACTER VARYING(100) NOT NULL,
	description TEXT,
	app_id INTEGER NOT NULL,
	CONSTRAINT fk_events_app FOREIGN KEY(app_id) REFERENCES apps(id) ON DELETE CASCADE
	);