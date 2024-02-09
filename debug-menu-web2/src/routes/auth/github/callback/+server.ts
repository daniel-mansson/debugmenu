import { github, lucia, pool } from "$lib/server/auth";
import { OAuth2RequestError } from "arctic";
import { generateId } from "lucia";

import type { RequestEvent } from "@sveltejs/kit";
import type { DatabaseUser } from "$lib/server/db";

export async function GET(event: RequestEvent): Promise<Response> {
	const code = event.url.searchParams.get("code");
	const state = event.url.searchParams.get("state");
	const storedState = event.cookies.get("github_oauth_state") ?? null;
	if (!code || !state || !storedState || state !== storedState) {
		return new Response(null, {
			status: 400
		});
	}

	try {
		const tokens = await github.validateAuthorizationCode(code);
		const githubUserResponse = await fetch("https://api.github.com/user", {
			headers: {
				Authorization: `Bearer ${tokens.accessToken}`
			}
		});
		const githubUser: GitHubUser = await githubUserResponse.json();
		const existingUserResult = await pool.query('SELECT * FROM users WHERE "Provider" = $1 AND "ProviderAccountId" = $2', ['github', githubUser.id])
		const existingUser = existingUserResult.rows[0] as
			| DatabaseUser
			| undefined;

		console.log(githubUser)
		const emailsResponse = await fetch("https://api.github.com/user/emails", {
			headers: {
				Authorization: `Bearer ${tokens.accessToken}`
			}
		});
		const emails: GitHubUserEmail[] = await emailsResponse.json();
		console.log(emails)
		const primaryEmail = emails.find(e => e.primary);
		console.log(primaryEmail)

		if (!primaryEmail?.verified) {
			return new Response(null, {
				status: 400
			});
		}

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
				githubUser.id,
				githubUser.name,
				'github',
				primaryEmail.email,
				githubUser.avatar_url,
				new Date().toISOString()
			]);
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

function handleGithubUser(): boolean {
	return false;
}

interface GitHubUser {
	id: string;
	login: string;
	name: string;
	avatar_url: string;
}

interface GitHubUserEmail {
	email: string;
	primary: boolean;
	verified: boolean;
	visibility: string;
}
