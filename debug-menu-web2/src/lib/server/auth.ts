import { Lucia } from "lucia";
import { NodePostgresAdapter } from "@lucia-auth/adapter-postgresql";
import { dev } from "$app/environment";
import pkg from 'pg';
const { Pool } = pkg;
import { GitHub } from "arctic";
import { Google } from "arctic";
import { GITHUB_CLIENT_ID, GITHUB_CLIENT_SECRET } from "$env/static/private";
import { GOOGLE_ID, GOOGLE_SECRET } from "$env/static/private";
import type { DatabaseUser } from "./db";
import { BetterSqlite3Adapter } from "@lucia-auth/adapter-sqlite";


// const pool = new Pool({
// 	host: "localhost",
// 	database: "debugmenu",
// 	user: "postgres",
// 	password: "postgres",
// 	max: 20,
// 	idleTimeoutMillis: 30000,
// 	connectionTimeoutMillis: 2000
// });

// const adapter = new NodePostgresAdapter(pool, {
// 	user: "auth_user",
// 	session: "user_session"
// });
import { db } from "./db";

const adapter = new BetterSqlite3Adapter(db, {
	user: "user",
	session: "session"
});


export const lucia = new Lucia(adapter, {
	sessionCookie: {
		attributes: {
			// set to `true` when using HTTPS
			secure: !dev
		}
	}
});

declare module "lucia" {
	interface Register {
		Lucia: typeof lucia;
	}
}

export const github = new GitHub(GITHUB_CLIENT_ID, GITHUB_CLIENT_SECRET);
export const google = new Google(GOOGLE_ID, GOOGLE_SECRET, "http://localhost:5173/auth/callback/google");