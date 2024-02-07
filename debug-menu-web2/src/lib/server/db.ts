import sqlite from "better-sqlite3";

export const db = sqlite(":memory:");

db.exec(`CREATE TABLE IF NOT EXISTS user (
    id TEXT NOT NULL PRIMARY KEY,
    email TEXT,
    github_id INTEGER UNIQUE,
    google_id INTEGER UNIQUE,
    username TEXT NOT NULL
)`);

db.exec(`CREATE TABLE IF NOT EXISTS session (
    id TEXT NOT NULL PRIMARY KEY,
    expires_at INTEGER NOT NULL,
    user_id TEXT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES user(id)
)`);

export interface DatabaseUser {
    id: string;
    username: string;
    email: string;
    github_id: number | undefined;
    google_id: number | undefined;
}
