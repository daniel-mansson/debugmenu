import { Lucia } from "lucia";
import { NodePostgresAdapter } from "@lucia-auth/adapter-postgresql";
import { dev } from "$app/environment";
import pkg from 'pg';
const { Pool } = pkg;
import { GitHub } from "arctic";
import { Google } from "arctic";
import { GITHUB_CLIENT_ID, GITHUB_CLIENT_SECRET } from "$env/dynamic/private";
import { GOOGLE_ID, GOOGLE_SECRET } from "$env/dynamic/private";

export const pool = new Pool({
	host: "localhost",
	database: "debugmenu2",
	user: "postgres",
	password: "postgres",
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
export const google = new Google(GOOGLE_ID, GOOGLE_SECRET, "http://localhost:5173/auth/google/callback");