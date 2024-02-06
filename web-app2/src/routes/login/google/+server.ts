import { google } from "$lib/server/auth";
import { generateState, type GoogleTokens } from "arctic";
import { redirect } from "@sveltejs/kit";

import type { RequestEvent } from "@sveltejs/kit";

export async function GET(event: RequestEvent): Promise<Response> {
	const state = generateState();
	const url = await google.createAuthorizationURL(state, "hej", {
		scopes: ["profile", "email"]
	});

	//const tokens: GoogleTokens = await google.validateAuthorizationCode("hej", "hej");

	console.log('google: ' + url)
	console.log('google: ' + state)

	event.cookies.set("google_oauth_state", state, {
		path: "/",
		secure: import.meta.env.PROD,
		httpOnly: true,
		maxAge: 60 * 10,
		sameSite: "lax"
	});

	return redirect(302, url.toString());
}
