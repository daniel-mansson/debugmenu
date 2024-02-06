import { google, lucia } from "$lib/server/auth";
import { OAuth2RequestError } from "arctic";
import { generateId } from "lucia";
import { db } from "$lib/server/db";

import type { RequestEvent } from "@sveltejs/kit";
import type { DatabaseUser } from "$lib/server/db";

export async function GET(event: RequestEvent): Promise<Response> {

	console.log("HEEJ")
	const code = event.url.searchParams.get("code");
	const state = event.url.searchParams.get("state");
	const storedState = event.cookies.get("google_oauth_state") ?? null;
	if (!code || !state || !storedState || state !== storedState) {
		return new Response(null, {
			status: 400
		});
	}

	try {
		const tokens = await google.validateAuthorizationCode(code, "hej");
		const response = await fetch("https://openidconnect.googleapis.com/v1/userinfo", {
			headers: {
				Authorization: `Bearer ${tokens.accessToken}`
			}
		});
		const user = await response.json();
		console.log(user)

		const existingUser = db.prepare("SELECT * FROM user WHERE google_id = ?").get(user.id) as
			| DatabaseUser
			| undefined;

		if (existingUser) {
			const session = await lucia.createSession(existingUser.id, {});
			const sessionCookie = lucia.createSessionCookie(session.id);
			event.cookies.set(sessionCookie.name, sessionCookie.value, {
				path: ".",
				...sessionCookie.attributes
			});
		} else {
			const userId = generateId(15);
			db.prepare("INSERT INTO user (id, google_id, username) VALUES (?, ?, ?)").run(
				userId,
				user.id,
				user.name
			);
			const session = await lucia.createSession(userId, {});
			const sessionCookie = lucia.createSessionCookie(session.id);
			event.cookies.set(sessionCookie.name, sessionCookie.value, {
				path: ".",
				...sessionCookie.attributes
			});
		}
		return new Response(null, {
			status: 302,
			headers: {
				Location: "/"
			}
		});
	} catch (e) {
		console.log(e)
		if (e instanceof OAuth2RequestError && e.message === "bad_verification_code") {
			// invalid code
			return new Response(null, {
				status: 400
			});
		}
		return new Response(null, {
			status: 500
		});
	}
}

interface GoogleUser {
	id: string;
	login: string;
}
