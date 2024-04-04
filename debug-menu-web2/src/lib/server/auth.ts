import { Lucia } from "lucia";
import { NodePostgresAdapter } from "@lucia-auth/adapter-postgresql";
import { dev } from "$app/environment";
import pkg from 'pg';
const { Pool } = pkg;
import { GitHub } from "arctic";
import { Google } from "arctic";
import { GITHUB_CLIENT_ID, GITHUB_CLIENT_SECRET } from "$env/static/private";
import { GOOGLE_ID, GOOGLE_SECRET, DB_SECRET, DB_HOST, DB_NAME, DB_USER } from "$env/static/private";

console.log(DB_USER)
export const pool = new Pool({
	host: DB_HOST,
	database: DB_NAME,
	user: DB_USER,
	password: DB_SECRET,
	max: 20,
	idleTimeoutMillis: 30000,
	connectionTimeoutMillis: 2000
});

const adapter = new NodePostgresAdapter(pool, {
	user: "users",
	session: "sessions"
});

export const lucia = new Lucia(adapter, {
	sessionCookie: {
		attributes: {
			// set to `true` when using HTTPS
			secure: !dev
		}
	},
	getUserAttributes: (attributes) => {
		return {
			name: attributes.Name,
			image: attributes.Image,
			email: attributes.Email,
			provider: attributes.Provider,
		};
	}
});

declare module "lucia" {
	interface Register {
		Lucia: typeof lucia;
		DatabaseUserAttributes: DatabaseUserAttributes;
	}
}

interface DatabaseUserAttributes {
	Name: string;
	Email: string;
	Image: string;
	Provider: string;
}

export const github = new GitHub(GITHUB_CLIENT_ID, GITHUB_CLIENT_SECRET);
export const google = new Google(GOOGLE_ID, GOOGLE_SECRET, GOOGLE_CALLBACK);