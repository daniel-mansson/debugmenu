import { google, lucia, pool } from "$lib/server/auth";
import { OAuth2RequestError } from "arctic";
import { generateId } from "lucia";

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

		console.log("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR")
		console.log(response)

		const user = await response.json();
		console.log(user)

		if (!user.email_verified) {
			return new Response(null, {
				status: 400
			});
		}

		const existingUserResult = await pool.query('SELECT * FROM users WHERE "Provider" = $1 AND "ProviderAccountId" = $2', ['google', user.id])
		const existingUser = existingUserResult.rows[0] as
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

			await pool.query('INSERT INTO users ("id", "ProviderAccountId", "Name", "Provider", "Email", "Image", "EmailVerified") VALUES ($1, $2, $3, $4, $5, $6, $7)', [
				userId,
				user.sub,
				user.name,
				'google',
				user.email,
				user.picture,
				new Date().toISOString()
			]);

			const session = await lucia.createSession(userId, {});
			const sessionCookie = lucia.createSessionCookie(session.id);

			console.log(userId)
			console.log(session)
			console.log(sessionCookie)

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
